using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    
    public NetworkPlayerSpawner playerSpawner;  // reference to the player spawner
    
    //////////////////////////////////////////////
    //////////// CONNECTING TO SERVER ////////////
    //////////////////////////////////////////////
    
    void Start()
    {
        ConnectToServer();
    }

    
    // Connecting 
    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true; // to call PhotonNetwork.LoadLevel()
        Debug.Log("Try Connect To Server...");
    }


    // Connected
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected To Server.");
        
        // Room options
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Design Lab", roomOptions, TypedLobby.Default);
    }

    // Disconnected
    public override void OnDisconnected(DisconnectCause cause){
        base.OnDisconnected(cause);
        Debug.Log(cause);
    }
    
    
    //////////////////////////////////////////////
    //////////// PLAYER JOINING ROOM /////////////
    //////////////////////////////////////////////


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined a Room");
        Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);

        playerSpawner.SpawnPlayer(); // Spawn the player when we have joined the room
        playerSpawner.SpawnCar(); // Use this when testing on PC
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("A new participant joined the room");

        playerSpawner.SpawnCar(); // Use this when testing on both Quest and PC
    }


}


