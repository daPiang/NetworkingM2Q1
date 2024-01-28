using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;

public class CustomNetworkHandler : MonoBehaviour {
    [Networked] public string LobbyName {get; set;}
    [Networked] public int MaxPlayers {get; set;}
    [Networked] public List<string> PlayerNames {get; set;}
    private List<NetworkRunner> LobbyManagerRunner;

    // public override void FixedUpdateNetwork()
    // {
    //     // foreach(var player in LobbyManagerRunner)
    //     // {
    //     //     string name = player.GetComponent<LobbyUI>().PlayerName;
    //     //     PlayerNames.Add(name);
    //     // }
    // }
}