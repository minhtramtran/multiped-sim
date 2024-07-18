using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMIcontroller : MonoBehaviour
{
    // cruisingMode as the default mode
    public int whichState = 1; 
    //public int crossingState;

    // reference to CarEngine script
    private CarEngine carEngine;


    // LED lighting
    public GameObject bumper;
    public GameObject bumper_left;
    public GameObject bumper_right;
    public GameObject crossing;
    public GameObject laserLight;
    
    void Start()
    {
        carEngine = GetComponent<CarEngine>();
    }

  void Update()
{
    // Update whichState based on braking and stopped status
    if (carEngine.GetStoppedStatus()) {
        whichState = 3;
    }
    else if (carEngine.GetActivationStatus()) { // instead of activate when the AV breaks, activate when the stopping distance is 20m
        whichState = 2;
    }
    else {
        whichState = 1; // Default state is cruisingMode
    }

    toggleHMI();
}


   public void toggleHMI(){ 
        switch(whichState) {
            case 1:
                cruisingMode();
                break;
            case 2:
                yieldingMode();
                break;
             case 3:
                fullstopMode();
                break;
            default:
                break;
        }
   }


    private void cruisingMode()
    {
        bumper.SetActive(true);
        bumper_left.SetActive(false);
        bumper_right.SetActive(false);
        crossing.SetActive(false);
        laserLight.SetActive(false);
    }

    private void yieldingMode()
    {
        bumper.SetActive(false);
        bumper_left.SetActive(true);
        bumper_right.SetActive(true);
        crossing.SetActive(false);
        laserLight.SetActive(false);
    }

    private void fullstopMode()
    {
        bumper.SetActive(false);
        bumper_left.SetActive(true);
        bumper_right.SetActive(true);
        crossing.SetActive(true);
        laserLight.SetActive(true);
    }

}
