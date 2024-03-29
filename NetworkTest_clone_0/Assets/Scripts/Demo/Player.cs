using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;

    [Networked] private TickTimer Delay { get; set; }
    [SerializeField] private Ball _prefabBall;
    private Vector3 _forward;
    private Renderer Rend => gameObject.GetComponent<Renderer>();
    [SerializeField] private Color color1;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * Runner.DeltaTime * data.direction);

            if (data.direction.sqrMagnitude > 0)
                _forward = data.direction;
            if (HasStateAuthority && Delay.ExpiredOrNotRunning(Runner))
            {
                if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0)
                {
                    // Runner.Spawn(_prefabBall,
                    // transform.position + _forward, Quaternion.LookRotation(_forward),
                    // Object.InputAuthority);
                    Rend.material.color = color1;
                }
            }
        }
    }
}
