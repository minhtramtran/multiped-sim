using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMIactivation : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Collider is attached to the wheels, that's why we need to get the parent object
        GameObject vehicle = other.gameObject.transform.parent.gameObject;
        CarEngine carEngine = vehicle.GetComponent<CarEngine>();

        // Get the ScenarioManager script from the parent
        ScenarioManager scenarioManager = GetComponentInParent<ScenarioManager>();

        if (scenarioManager.vehicleBehavior == ScenarioManager.VehicleBehavior.stopForP1 || scenarioManager.vehicleBehavior == ScenarioManager.VehicleBehavior.stopForP2)
        {
            carEngine.isHMIactivated = true;
        }
        else if (scenarioManager.vehicleBehavior == ScenarioManager.VehicleBehavior.nonStop)
        {
            carEngine.isHMIactivated = false;
        }
    }
}
