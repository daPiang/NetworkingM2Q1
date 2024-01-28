using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;

public class LobbyUI : MonoBehaviour
{
    
    [Header("Lobby Components")]
    public GameObject LobbyPanel;
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private TMP_InputField playerName; //CUSTOM
    [SerializeField] private TextMeshProUGUI numberOfPlayers;
    public Button CreateBtn;
    public Button JoinBtn;
    [SerializeField] private TMP_Dropdown maxPlayers;

    [Header("Room Components")]
    public GameObject RoomPanel;
    public Button StartGameButton;
    [Header("Player Name Display")]
    public GameObject displayPrefab;
    public Transform displayParent;

    [Header("Lobby Data")]
    public int MaxPlayers;
    public string NumberOfPlayers
    {
        get { return numberOfPlayers.text; }
        set { numberOfPlayers.text = value; }
    }
    public string RoomName => roomName.text;
    public string PlayerName => playerName.text;

    public void PopulateDisplay<T>(List<T> list)
    {
        if(transform.childCount > 0)
            foreach(Transform child in displayParent)
            {
                Destroy(child.gameObject);
            }
        
        
        foreach(T item in list)
        {
            GameObject displayItem = Instantiate(displayPrefab, displayParent.position, Quaternion.identity);
            displayItem.GetComponent<PlayerNameDisplayItem>().nameToDisplay = item.ToString();
            displayItem.transform.SetParent(displayParent);
        }
    }

    // public override void FixedUpdateNetwork() {
    //     int.TryParse(maxPlayers.options[maxPlayers.value].text, out int dropdownValue);
    //     MaxPlayers = dropdownValue;

    //     //Input Field Null Handler
    //     if(roomName.text == "" || roomName.text == null)
    //     {
    //         RoomName = "Lobby";
    //     }

    //     if(playerName.text == "" || playerName.text == null)
    //     {
    //         PlayerName = "player";
    //     }
    // }

    private void Update() {
        int.TryParse(maxPlayers.options[maxPlayers.value].text, out int dropdownValue);
        MaxPlayers = dropdownValue;

        //Input Field Null Handler
        // if(RoomName == "" || RoomName == null)
        // {
        //     // roomName.text = "Lobby";
        // }

        // if(PlayerName == "" || PlayerName == null)
        // {
        //     // playerName.text = "player";
        // }
    }
}
