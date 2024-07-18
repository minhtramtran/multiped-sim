using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurbController : MonoBehaviour
{
    public GameObject nearStones1;
    public GameObject farStones1;
    public GameObject nearStones2;
    public GameObject farStones2;

    private ChangeLEDColor nearStones1ChangeLEDColor;
    private ChangeLEDColor farStones1ChangeLEDColor;
    private ChangeLEDColor nearStones2ChangeLEDColor;
    private ChangeLEDColor farStones2ChangeLEDColor;

    private GetCarStatus carStatus;
    private ScenarioManager scenarioManager;

    void Start()
    {
        nearStones1ChangeLEDColor = nearStones1.GetComponent<ChangeLEDColor>();
        farStones1ChangeLEDColor = farStones1.GetComponent<ChangeLEDColor>();
        nearStones2ChangeLEDColor = nearStones2.GetComponent<ChangeLEDColor>();
        farStones2ChangeLEDColor = farStones2.GetComponent<ChangeLEDColor>();

        carStatus = GetComponent<GetCarStatus>();
        scenarioManager = GetComponent<ScenarioManager>();
    }

    void Update()
    {
        bool useGreen = false;
        
        if(carStatus.isBraking)
        {
            if(scenarioManager.vehicleBehavior == ScenarioManager.VehicleBehavior.stopForP1 || 
               (scenarioManager.vehicleBehavior == ScenarioManager.VehicleBehavior.stopForP2 && carStatus.isStopped))
            {
                useGreen = true;
            }
            else if(scenarioManager.vehicleBehavior == ScenarioManager.VehicleBehavior.stopForP2)
            {
                nearStones1ChangeLEDColor.useGreenMaterial = false;
                farStones1ChangeLEDColor.useGreenMaterial = false;
                nearStones2ChangeLEDColor.useGreenMaterial = true;
                farStones2ChangeLEDColor.useGreenMaterial = true;
                return;
            }
        } else if(carStatus.isPassed){
            useGreen = true;
        }
        
        nearStones1ChangeLEDColor.useGreenMaterial = useGreen;
        farStones1ChangeLEDColor.useGreenMaterial = useGreen;
        nearStones2ChangeLEDColor.useGreenMaterial = useGreen;
        farStones2ChangeLEDColor.useGreenMaterial = useGreen;
    }
}
