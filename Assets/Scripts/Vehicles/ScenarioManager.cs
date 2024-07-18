using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public enum VehicleBehavior { stopForP1, stopForP2, nonStop };

    public GameObject brakingP1;
    public GameObject brakingP2;
    public GameObject activationP1;
    public GameObject activationP2;
    public GameObject nonBraking;

    public GameObject smartCurbs;
    public GameObject ARcrossing;

    [Header("Vehicle Behaviour")]
    public VehicleBehavior vehicleBehavior;

    [Header("Design Concepts")]
    public bool noHMI = true;
    public bool vehicleHMI = false;
    public bool streetHMI = false;
    public bool pedestrianHMI = false;

    void Start()
    {
        DeactivateAll();

        switch (vehicleBehavior)
        {
            case VehicleBehavior.stopForP1:
                brakingP1.SetActive(true);
                activationP1.SetActive(true);
                break;
            case VehicleBehavior.stopForP2:
                brakingP2.SetActive(true);
                activationP2.SetActive(true);
                break;
            case VehicleBehavior.nonStop:
                nonBraking.SetActive(true);
                break;
            default:
                break;
        }

        if (streetHMI)
        {
            smartCurbs.SetActive(true);
        }

        if(pedestrianHMI){
            ARcrossing.SetActive(true);
        }
    }

    private void DeactivateAll()
    {
        CheckNullObjects(brakingP1, brakingP2, activationP1, activationP2, nonBraking, smartCurbs, ARcrossing);

        brakingP1.SetActive(false);
        brakingP2.SetActive(false);
        activationP1.SetActive(false);
        activationP2.SetActive(false);
        nonBraking.SetActive(false);
        smartCurbs.SetActive(false);
        ARcrossing.SetActive(false);
    }

    private void CheckNullObjects(params GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            if (obj == null)
            {
                Debug.LogError("One or more Game Objects are not assigned in the inspector.");
                return;
            }
        }
    }
}
