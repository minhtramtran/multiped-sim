using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Button nextButton;
    public TMP_Text sceneNameDebug;
    private static SceneSwitcher instance;
    public static SceneSwitcher Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next Scene";
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        if (currentSceneName == "LobbyScene") {
            sceneNameDebug.text = "";
        } else {
            sceneNameDebug.text = currentSceneName;    
        }
    }

    public void ChangeToTutorialScene()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void ChangeToLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void StartBlock(int blockNumber, List<string> sceneNames)
    {
        // Store block information for later use
        PlayerPrefs.SetInt("CurrentBlockNumber", blockNumber);
        PlayerPrefs.SetString("CurrentBlockScenes", string.Join(",", sceneNames.ToArray()));
    }
    

    public void LoadNextScene()
    {
        // Retrieve block information from PlayerPrefs
        int blockNumber = PlayerPrefs.GetInt("CurrentBlockNumber");
        string[] sceneNames = PlayerPrefs.GetString("CurrentBlockScenes").Split(',');

        // Check if there are more scenes to load in the block
        if (sceneNames.Length > 1) // Check if there are at least 2 scenes
        {
            // Skip the first scene and start from the second scene
             string nextSceneName = sceneNames[1];

            // Remove the loaded scene from the list
            List<string> remainingScenes = new List<string>(sceneNames);
            remainingScenes.RemoveAt(1); // Remove the second scene (index 1)
            PlayerPrefs.SetString("CurrentBlockScenes", string.Join(",", remainingScenes.ToArray()));

            // Set the next scene as the active scene
            Photon.Pun.PhotonNetwork.LoadLevel(nextSceneName);
        }
        else
        {
            // Block completed, load the Lobby
            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Lobby";
            PhotonNetwork.AutomaticallySyncScene = true;
            Photon.Pun.PhotonNetwork.LoadLevel("LobbyScene");
            Debug.Log("Block " + blockNumber + " completed!");
        }
    }

    // Maybe use for troubleshotting
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

}
