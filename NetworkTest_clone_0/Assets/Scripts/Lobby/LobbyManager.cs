using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;
using AYellowpaper.SerializedCollections;

public class LobbyManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkRunner _runner;
    public NetworkPrefabRef _playerPrefab;

    [SerializeField] private LobbyUI UI;

    public Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new();
    [SerializedDictionary("Player Reference", "Network Object")]
    public SerializableDictionary<PlayerRef, NetworkObject> _serializedSpawnedCharacters = new();
    private bool _mouseButton0;

    public List<string> connectedPlayers = new();

    private void Awake()
    {
        UI.CreateBtn.onClick.AddListener(() => StartGame(GameMode.Host));
        UI.JoinBtn.onClick.AddListener(() => StartGame(GameMode.Client));
    }

    private void Update()
    {
        _mouseButton0 |= Input.GetMouseButton(0);
        Debug.Log(connectedPlayers.Count);
    }

    public async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Start or join (depends on gamemode) a session with a specific name
        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = UI.RoomName,
            PlayerCount = UI.MaxPlayers,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

        if(_runner.IsServer) Debug.Log($"{_runner.UserId} is Hosting a game");
    }

    public void CreatePlayers()
    {
        foreach(var player in _runner.ActivePlayers)
        {
            //// Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % _runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = _runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
            _serializedSpawnedCharacters.Add(player, networkPlayerObject);
        }

        UI.RoomPanel.SetActive(false);
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log($"Connected {runner.IsRunning}");
        // Debug.Log(runner.GetComponent<Player>().playerName);
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
            data.direction += Vector3.forward;

        if (Input.GetKey(KeyCode.S))
            data.direction += Vector3.back;

        if (Input.GetKey(KeyCode.A))
            data.direction += Vector3.left;

        if (Input.GetKey(KeyCode.D))
            data.direction += Vector3.right;

        if (_mouseButton0)
            data.buttons |= NetworkInputData.MOUSEBUTTON1;

        _mouseButton0 = false;

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if(runner.IsServer)
        {
            connectedPlayers.Add(UI.PlayerName);
        }

        if(UI.displayPrefab != null && connectedPlayers.Count > 0) UI.PopulateDisplay(connectedPlayers);

        // Debug.Log(runner.GetComponent<Player>().playerName);
        if (runner.IsServer)
        {
            UI.LobbyPanel.SetActive(false);
            UI.RoomPanel.SetActive(true);
            UI.NumberOfPlayers = $"{UI.RoomName}({runner.ActivePlayers.ToList().Count}/{UI.MaxPlayers})";
            
        }
        else
        {
            UI.LobbyPanel.SetActive(false);
            UI.RoomPanel.SetActive(true);
            UI.NumberOfPlayers = $"{UI.RoomName}({runner.ActivePlayers.ToList().Count}/{UI.MaxPlayers})";
        }

        UI.StartGameButton.gameObject.SetActive(!runner.IsClient);
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            _spawnedCharacters.Remove(player);
            _serializedSpawnedCharacters.Remove(player);
        }

        UI.NumberOfPlayers = $"{UI.RoomName}({runner.ActivePlayers.ToList().Count}/{UI.MaxPlayers})";
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }
}
