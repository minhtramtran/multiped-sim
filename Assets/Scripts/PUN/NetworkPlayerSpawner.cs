using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;


public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject[] playerPrefabs;  // array of all possible avatars
    private GameObject playerInstance;  // reference to keep track of the instantiated player

    public Vector3[] xrRigPositions;  // array of XR Rig positions for each avatar
    public Vector3[] avatarScales;  // array of scales for each avatar

    public GameObject autonomousCarPrefab;
    public GameObject manualCarPrefab;

    public Vector3 carSpawnPosition;

    public ScenarioManager scenarioManager;
    public int lastSpawnPositionIndex;
    public static bool IsPositionSwapped = false;  // Add this static variable

    //////////////////////////////////////////////
    //////////////// SPAWN PLAYER ////////////////
    //////////////////////////////////////////////

    public void SpawnPlayer() // 26th June Backup
{
    // Load the selected avatar index
    int avatarIndex = PlayerPrefs.GetInt(MultiplayerVRConstants.SELECTED_AVATAR_INDEX); // 0 is a default value

    // Ensure the loaded index is within the range of available prefabs
    if (avatarIndex < 0 || avatarIndex >= playerPrefabs.Length)
    {
        Debug.LogError("Invalid avatar index");
        return;
    }

    Vector3 spawnPosition;

    // Get the last spawn position
    lastSpawnPositionIndex = PlayerPrefs.GetInt(MultiplayerVRConstants.LAST_SPAWN_POSITION_INDEX, avatarIndex);

    // Ensure the loaded index is within the range of available XR Rig positions
    if (lastSpawnPositionIndex < 0 || lastSpawnPositionIndex >= xrRigPositions.Length)
    {
        Debug.LogError("Invalid XR Rig position index");
        return;
    }

    spawnPosition = xrRigPositions[lastSpawnPositionIndex];

    // Swap the last spawn position index between 0 and 1 for the next scene
    PlayerPrefs.SetInt(MultiplayerVRConstants.LAST_SPAWN_POSITION_INDEX, 1 - lastSpawnPositionIndex);

    // Instantiate the player at the specified spawn position
    playerInstance = PhotonNetwork.Instantiate(playerPrefabs[avatarIndex].name, new Vector3(0,0,0), Quaternion.identity);

    // Add the AvatarCustom script and set the XR Rig position and avatar index
    AvatarCustom avatarCustom = playerInstance.AddComponent<AvatarCustom>();
    avatarCustom.xrRigPosition = spawnPosition;
    avatarCustom.avatarScale = avatarScales[avatarIndex];  // pass the avatar scale to the AvatarCustom script
}


    //////////////////////////////////////////////
    //////////////// SPAWN CAR ///////////////////
    //////////////////////////////////////////////

    public void SpawnCar()
    {
        // Check if there are 2 players in the room
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Quaternion carSpawnRotation = Quaternion.Euler(0f, 90f, 0f);
            if(scenarioManager.vehicleHMI){
                GameObject carInstance = PhotonNetwork.InstantiateSceneObject(autonomousCarPrefab.name, carSpawnPosition, carSpawnRotation);
            } else {
                GameObject carInstance = PhotonNetwork.InstantiateSceneObject(manualCarPrefab.name, carSpawnPosition, carSpawnRotation);
            }

            Debug.Log("The AV has been spawned.");

        }
    }


    //////////////////////////////////////////////
    /////////////// SCENE RE-SPAWN ///////////////
    //////////////////////////////////////////////


    // Spawn player whenever there is a scene load
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("A scene has been loaded: " + scene.name);
        if (PhotonNetwork.InRoom)
        {
            SpawnPlayer();
            SpawnCar(); 
        }
    }


    //////////////////////////////////////////////
    /////////////// LEFT ROOM ////////////////////
    //////////////////////////////////////////////

    // Handle player destruction when leaving the room
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if (playerInstance)  // check if the player instance exists
        {
            PhotonNetwork.Destroy(playerInstance);
        }
    }


    
}
