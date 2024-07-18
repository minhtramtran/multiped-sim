using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCarStatus : MonoBehaviour
{
    private CarEngine carEngine;

    // Declare these fields at the class level
    public bool isBraking;
    public bool isStopped;
    public bool isPassed;
    public bool isHMIactivated;

    void Update()
    {
        if (carEngine == null)
        {
            carEngine = CarEngine.Instance;

            if (carEngine == null)
            {
                return; // CarEngine has not been instantiated yet, so return early.
            }
        }

        // Now we're sure that carEngine is not null and we can safely get the values.
        isBraking = carEngine.GetBrakingStatus();
        isStopped = carEngine.GetStoppedStatus();
        isPassed = carEngine.GetPassedStatus();
        isHMIactivated = carEngine.GetActivationStatus();

        // Use isBraking and isStopped...
    }
}
