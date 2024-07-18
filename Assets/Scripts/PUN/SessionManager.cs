using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SessionManager : MonoBehaviour
{
    public TMP_Text sessionSequenceText;
    public Button[] blockButtons;
    public Button tutorialButton;
    public TMP_Dropdown sessionDropdown;
    public SceneSwitcher sceneSwitcher;

    private Dictionary<int, int[]> sessionSequenceMap;
    private Dictionary<int, string> sessionCityMap;
    private Dictionary<int, List<string>> blockSceneMap;

    private void Start()
    {
        // Initialize the session sequence mappings
        sessionSequenceMap = new Dictionary<int, int[]>()
        {
            {1, new int[] {1, 2, 4, 3}}, // Sequence for session 1
            {2, new int[] {2, 3, 1, 4}}, // Sequence for session 2
            {3, new int[] {3, 4, 2, 1}}, // Sequence for session 3
            {4, new int[] {4, 1, 3, 2}}, // Sequence for session 4
        };

        // Initialize the session city mappings
        sessionCityMap = new Dictionary<int, string>()
        {
            {1, "Amsterdam"},
            {2, "Beijing"},
            {3, "Cairo"},
            {4, "Dubai"}
        };

         // Initialize the block scene mappings
        blockSceneMap = new Dictionary<int, List<string>>()
        {
            {1, new List<string>() {"Baseline_StopP1a", "Baseline_StopP1b", "Baseline_StopP2a", "Baseline_StopP2b", "Baseline_Nonstop"}},
            {2, new List<string>() {"Vehicle_StopP1a", "Vehicle_StopP1b", "Vehicle_StopP2a", "Vehicle_StopP2b", "Vehicle_Nonstop"}},
            {3, new List<string>() {"Street_StopP1a", "Street_StopP1b", "Street_StopP2a", "Street_StopP2b", "Street_Nonstop"}},
            {4, new List<string>() {"Pedestrian_StopP1a", "Pedestrian_StopP1b", "Pedestrian_StopP2a", "Pedestrian_StopP2b", "Pedestrian_Nonstop"}},
        };

        // Hide all block buttons
        foreach (Button blockButton in blockButtons)
        {
            blockButton.gameObject.SetActive(false);
        }
        tutorialButton.gameObject.SetActive(false);

        // Populate the session dropdown options
        List<string> sessionOptions = new List<string>();
        for (int i = 1; i <= sessionCityMap.Count; i++)
        {
            string cityName = sessionCityMap[i];
            sessionOptions.Add(i + " - " + cityName);
        }
        sessionDropdown.ClearOptions();
        sessionDropdown.AddOptions(sessionOptions);

        // Add a listener to the session dropdown to handle selection changes
        sessionDropdown.onValueChanged.AddListener(OnSessionDropdownValueChanged);
    }


    private void OnSessionDropdownValueChanged(int sessionIndex)
    {
        int sessionNumber = sessionIndex + 1; // Session numbers start from 1
        DisplaySessionSequence(sessionNumber);

        // Enable all block buttons
        foreach (Button blockButton in blockButtons)
        {
            blockButton.gameObject.SetActive(true);
        }
        tutorialButton.gameObject.SetActive(true);
    }


    private void DisplaySessionSequence(int sessionNumber)
{
    // Retrieve the sequence for the selected session number
    int[] sequence = sessionSequenceMap[sessionNumber];

    // Update the labels of the UI Button elements
    for (int i = 0; i < blockButtons.Length; i++)
    {
        string buttonText = "Block " + sequence[i]; // Combine block number
        blockButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = buttonText;

        // Add a click listener to the button
        blockButtons[i].onClick.RemoveAllListeners(); // Remove any existing listeners

        int temp = i; // Create a temporary copy of 'i'
        blockButtons[i].onClick.AddListener(() => HandleButtonClick(sequence[temp]));
    }
}


  private void HandleButtonClick(int blockNumber)
{
    // Handle the button click action for the selected block
    Debug.Log("Button " + blockNumber + " clicked!");

    // Retrieve the scene names for the current block from the blockSceneMap
    List<string> sceneNames = blockSceneMap[blockNumber];

    if (sceneNames.Count > 0)
    {
        // Load the first scene in the block
        string firstSceneName = sceneNames[0];
        Photon.Pun.PhotonNetwork.LoadLevel(firstSceneName);
        sceneSwitcher.StartBlock(blockNumber, sceneNames);
    }
    else
    {
        Debug.Log("No scenes found for block " + blockNumber);
    }
}


}
