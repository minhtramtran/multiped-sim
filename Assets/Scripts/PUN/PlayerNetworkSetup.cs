using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// this script ensures that every player only controls their own local rig
public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public GameObject LocalXRRigGameobject;
    public GameObject Avatar;

    public GameObject HiddenHead; // do not want it to be invisible to other
    public GameObject HiddenHair;
    public GameObject HiddenEyeLeft;
    public GameObject HiddenEyeRight;
    public GameObject HiddenTeeth;
    public GameObject HiddenGlasses;

    private Vector3 initialPos = Vector3.zero; // Initial position is (0,0,0)


    void Start()
    {
        if (photonView.IsMine)
        {
            // the player is local
            LocalXRRigGameobject.transform.position = initialPos; // Ensure LocalXRRigGameobject is at initial position
            LocalXRRigGameobject.SetActive(true);
            Avatar.GetComponent<AvatarAnimationController>().enabled = true;
            SetLayerRecursively(HiddenHead, 6);
            SetLayerRecursively(HiddenHair, 6);
            SetLayerRecursively(HiddenEyeLeft, 6);
            SetLayerRecursively(HiddenEyeRight, 6);
            SetLayerRecursively(HiddenTeeth, 6);
            SetLayerRecursively(HiddenGlasses,6);
        }
        else
        {
            // the player is remote
            LocalXRRigGameobject.SetActive(false);
            Avatar.GetComponent<AvatarAnimationController>().enabled = false;
            SetLayerRecursively(HiddenHead, 0);
            SetLayerRecursively(HiddenHair, 0);
            SetLayerRecursively(HiddenEyeLeft, 0);
            SetLayerRecursively(HiddenEyeRight, 0);
            SetLayerRecursively(HiddenTeeth, 0);
            SetLayerRecursively(HiddenGlasses, 0);
        }
    }

    
    void Update()
    {
        
    }

    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }
}
