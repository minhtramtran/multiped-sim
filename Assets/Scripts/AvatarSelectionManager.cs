using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AvatarSelectionManager : MonoBehaviour
{
    [SerializeField]

    public GameObject[] selectableAvatarModels;
    public GameObject[] loadableAvatarModels;

    public int avatarSelectionNumber = 0;


    public static AvatarSelectionManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {

            avatarSelectionNumber = 0;
            ActivateAvatarModelAt(avatarSelectionNumber);
            LoadAvatarModelAt(avatarSelectionNumber);
    }


    public void NextAvatar()
    {
        avatarSelectionNumber += 1;
        if (avatarSelectionNumber >= selectableAvatarModels.Length)
        {
            avatarSelectionNumber = 0;
        }
        ActivateAvatarModelAt(avatarSelectionNumber);

    }

    public void PreviousAvatar()
    {
        avatarSelectionNumber -= 1;

        if (avatarSelectionNumber < 0)
        {
            avatarSelectionNumber = selectableAvatarModels.Length - 1;
        }
        ActivateAvatarModelAt(avatarSelectionNumber);

    }

    /// Activates the selected Avatar model inside the Avatar Selection Platform
    private void ActivateAvatarModelAt(int avatarIndex)
    {
        foreach (GameObject selectableAvatarModel in selectableAvatarModels)
        {
            selectableAvatarModel.SetActive(false);
        }

        selectableAvatarModels[avatarIndex].SetActive(true);
        Debug.Log(avatarSelectionNumber);

        LoadAvatarModelAt(avatarSelectionNumber);
    }


    /// Loads the Avatar Model and integrates into the VR Player Controller gameobject
    private void LoadAvatarModelAt(int avatarIndex)
    {
        foreach (GameObject loadableAvatarModel in loadableAvatarModels)
        {
            loadableAvatarModel.SetActive(false);
        }

        loadableAvatarModels[avatarIndex].SetActive(true);
        SaveAvatarSelection(avatarIndex);

        //ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.AVATAR_SELECTION_NUMBER, avatarSelectionNumber} };
        //PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }


    // TRAM: When the player selects an avatar, save the selection both locally and in a Photon custom property
    private void SaveAvatarSelection(int avatarIndex)
    {
        // Save locally using PlayerPrefs
        PlayerPrefs.SetInt(MultiplayerVRConstants.SELECTED_AVATAR_INDEX, avatarIndex);

        // Save to Photon custom properties to share with other players in the session
        PhotonNetwork.LocalPlayer.CustomProperties[MultiplayerVRConstants.SELECTED_AVATAR_INDEX] = avatarIndex;
    }

}
