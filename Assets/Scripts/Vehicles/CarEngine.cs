using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

// Standard car engine: Do not modify this script


public class CarEngine : MonoBehaviour {

    public Transform path;
    private List<Transform> nodes;
    private int currentNode = 0;
    public int currentHMI = 1;
    public bool leadingCar = false;
    
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;
    
    [Header("Speeds")]
    public float maxSpeed = 30f;
    public float currentSpeed;
    public float currentSpeedMps;
    public float maxMotorTorque = 1000f;
    private float maxSteerAngle = 45f;
    private Vector3 centerOfMass;

    [Header("Brakes")]
    public float maxBrakeTorque = 20000f; //Increase this doesn't make the AV stop faster
    public float brakingDrag = 0.28f;
    public bool isBraking = false;
    public bool isStopped = false;  // Declare new variable
    public bool isPassed = false;
    public bool isHMIactivated;
    private float stopThreshold = 0.05f;  // Speed below which car is considered stopped
    public float stoppingDistance;  // Declare new variable
    public float stoppingTime;  // Declare new variable
    public float deceleration;  // Declare new variable
    public float brakeStartSpeed;  // Declare new variable

    private float brakeStartTime;  // Declare new variable
    private Vector3 brakeStartPos;  // Declare new variable

    [Header("Scenario")]
    public bool yieldingIntent =  true;
    public bool isAutonomous = true;

    [Header("Sensors")]
    public float sensorLength = 20f;
    public Vector3 frontSensorPos = new Vector3(0.0f, 0.2f, 0.5f);
    public float sensorConstraint = 12f;


    private bool isPedestrianDetected = false; // Declare at the top of the class
    private bool hasStartedMoving = false;


    //////////////////////////////////////////////
    //////////// VEHICLE INSTANTIATED ////////////
    //////////////////////////////////////////////

    public static CarEngine Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    //////////////////////////////////////////////
    //////////// SET UP VEHICLE //////////////////
    //////////////////////////////////////////////


    void Start(){

        //Stablise the car by lowering the center of mass
        GetComponent<Rigidbody>().centerOfMass = centerOfMass; 

        //Each car has a copy of the path      
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        for (int i = 0; i< pathTransforms.Length; i++){
            if(pathTransforms[i]!= path.transform){
                nodes.Add(pathTransforms[i]);
            }
        }
    }


    //////////////////////////////////////////////
    ////////////  RUN VEHICLE //////////////////
    //////////////////////////////////////////////



    void FixedUpdate(){
        ApplySteer();
        Drive();
        CheckWaypointDistance();
        Braking();
        CheckIfStopped();
        Sensors();
    }

    
    private void ApplySteer(){
            Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
            float newSteer = (relativeVector.x / relativeVector.magnitude)* maxSteerAngle;
            wheelFL.steerAngle = Mathf.Lerp(wheelFL.steerAngle,newSteer,0.5f);
            wheelFR.steerAngle = Mathf.Lerp(wheelFR.steerAngle,newSteer,0.5f); // Smoother turns with lerp
    }

    private void Drive(){
        currentSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f; // Current speed in km/h
        currentSpeedMps = GetComponent<Rigidbody>().velocity.magnitude; // Current speed m/s

        if (currentSpeed < maxSpeed && isBraking==false){
            wheelFL.motorTorque = maxMotorTorque;
            wheelFR.motorTorque = maxMotorTorque;
            hasStartedMoving = true;
        } else {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
        }        
    }


    private void CheckWaypointDistance(){
        if(Vector3.Distance(transform.position, nodes[currentNode].position) < 11f){ // Depend on distance between two waypoints
                if (currentNode == nodes.Count - 1){
                    currentNode = 0; // If the car is approach the last node in the path already, we steer it back to node O.
                } else {
                    currentNode++; // Else we steer it to the next node
                }
        }
    }
    
    
    private void Braking() {
        if(isBraking == true) {
            GetComponent<Rigidbody>().drag = brakingDrag;
            wheelRL.brakeTorque = maxBrakeTorque;
            wheelRR.brakeTorque = maxBrakeTorque;

            // Record the start time and position when braking starts
            if (!isStopped && brakeStartTime == 0) {
                brakeStartTime = Time.time;
                brakeStartPos = transform.position;
                brakeStartSpeed = currentSpeed;
                Debug.Log($"Speed at braking start: {brakeStartSpeed} km/h");
            }
        } else {
            GetComponent<Rigidbody>().drag = 0f;
            wheelRL.brakeTorque = 0f;
            wheelRR.brakeTorque = 0f;
        }
    }


    private void CheckIfStopped() {
        if (currentSpeedMps < stopThreshold && hasStartedMoving) {
            isStopped = true;

            // When the vehicle stops, calculate stopping time and distance
            if (brakeStartTime != 0) {
                stoppingTime = Time.time - brakeStartTime;
                stoppingDistance = Vector3.Distance(brakeStartPos, transform.position);

                // Calculate deceleration based on stopping time and distance
                deceleration = 2 * stoppingDistance / Mathf.Pow(stoppingTime, 2);

                // Log stopping distance, stopping time, and deceleration
                Debug.Log($"Stopping distance: {stoppingDistance} meters");
                Debug.Log($"Stopping time: {stoppingTime} seconds");
                Debug.Log($"Deceleration: {deceleration} m/s²");

                // Reset brake start time for next braking event
                brakeStartTime = 0;
            }
        } else {
            isStopped = false;
        }
    }


    private void Sensors() {
        RaycastHit hit;
        Vector3 sensorStartPos = transform.position + frontSensorPos;
        LayerMask mask = LayerMask.GetMask("Pedestrian");

        if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength, mask)) {  
            // Draw ray for 5 seconds
            Debug.DrawRay(sensorStartPos, transform.forward * sensorLength, Color.red, 5.0f);

            isBraking = true;

            // doesn't work yet, maybe need to try a different approach
            isPedestrianDetected = true; // Indicate a pedestrian is detected
            Debug.Log("Pedestrian detected.");  // Debug message
        }  
    }



    // Related to Sensors() function
    private void SpeedingUp(){
        isBraking = false;
    }



    private void SelfDestroy(){
        if(transform.position.y < -3){
            Destroy(this.gameObject);
        }
    }

    //////////////////////////////////////////////
    /////////////////// UPDATE ///////////////////
    //////////////////////////////////////////////

    
    void Update(){
        //Using keyboard inputs as crossing signal
        GetKeyboardSignal(); 
        ResumeDriving(); 
    }


    //////////////////////////////////////////////
    //////////// KEYBOARD INPUTS /////////////////
    //////////////////////////////////////////////


    private void GetKeyboardSignal(){
        if (Input.GetKeyDown(KeyCode.Space)){
            isBraking = true;
        } else if(Input.GetKeyDown(KeyCode.Return)){
            isBraking = false;
        }
    }
    
    private void ResumeDriving(){
        if(Input.GetKeyDown(KeyCode.Return)){
            isBraking = false;
        }
    }

    public bool GetBrakingStatus() {
    return isBraking;
    }

    public bool GetStoppedStatus() {
        return isStopped;
    }

    public bool GetPassedStatus() {
        return isPassed;
    }

    public bool GetActivationStatus() {
        return isHMIactivated;
    }

}