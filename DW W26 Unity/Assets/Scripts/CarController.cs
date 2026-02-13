using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    //Rigidbody Vari
    private Rigidbody rb;
    

    //Scores
    public GameObject ScoreD1;
    public GameObject ScoreD2;

    //Timer
    public Timer timer;

    //Steering. One vari to hold math, one to set vals
    private float currentSteeringAngle; //This holds math
    public float maxSteeringAngle;

    //Braking. Same thing.
    private float currentBrakeForce;
    public float brakeForce;
    private bool isBraking;

    //Motor stuff
    public float motorForce;

    //Input varis
    private float h;
    private float v;

    //Spped vals
    public float currentSpeed;
    public float topSpeed;

    //vari refering to enum
    public driveTrain driveTrainRef= driveTrain.RWD;

    //Separate WheelColliders and Transforms. WheelColliders have built-in funcionality for position and rotation, so we will use that for the meshes.
    //DON'T ASSIGN A WHEELCOLLIDER TO A MESH.
    public WheelCollider frontLeftCollider, frontRightCollider, rearLeftCollider, rearRightCollider;
    public Transform frontLeftMesh, frontRightMesh, rearLeftMesh, rearRightMesh;

    //Vari for GM
    private GameManager gm;

    private bool goalReached;

    //Player Input
    private PlayerInput PlayerInputX;
    private InputAction InputActionTurn;
    private InputAction InputActionGas;
    private InputAction InputActionBrake;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameManager.Instance; //Can simply use this since it's static, so there is only one.
    }
    public void AssignPlayerInputDevice(PlayerInput playerInput)
    {
        // Record our player input (ie controller).
        PlayerInputX = playerInput;
        // Find the references to the "Move" and "Jump" actions inside the player input's action map
        // Here I specify "Player/" but it in not required if assigning the action map in PlayerInput inspector.
        
        InputActionTurn = playerInput.actions.FindAction($"Player/Move");
        InputActionGas = playerInput.actions.FindAction($"Player/Shoot");
        InputActionBrake = playerInput.actions.FindAction($"Player/Crouch");
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.Log($"{name}'s {nameof(PlayerController)}.{nameof(Rigidbody)} is null.");
            return;
        }
        
        //Call four funcions instead of writing everything in here
        PlayerInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        
        
    }
    
    private void PlayerInput()
    {
        Vector2 turning = InputActionTurn.ReadValue<Vector2>();

        h = turning.x;

        if(InputActionGas.IsPressed())
        {
            v = 1;
            Debug.Log("Gas");
        }

        else
        {
            v = 0;
        }

            isBraking = InputActionBrake.IsPressed();
        if(goalReached && InputActionBrake.IsPressed())
        {
            SceneManager.LoadScene("RallyMap");
        }

    }
    private void HandleMotor()
    {
        //If car is RWD, torque comes from rear wheels. If FWD, from front.
        switch(driveTrainRef)
        {
            case driveTrain.RWD:
                rearLeftCollider.motorTorque = v * motorForce;
                rearRightCollider.motorTorque = v * motorForce;
                break;

            case driveTrain.FWD:
                frontLeftCollider.motorTorque = v * motorForce;
                frontRightCollider.motorTorque = v * motorForce;
                break;
        }

        //Get our car's current speed from Rigidbody
        currentSpeed = rb.linearVelocity.magnitude * 3.6f;

        //Prevent going past max speed
        if(currentSpeed >= topSpeed)
        {
            currentSpeed = topSpeed;
        }
        //ensure the car stops
                //Math.Abs is absolute value.
                //ignores + or - sign

        if (currentSpeed <= Mathf.Abs(0.01f))
        {
            currentSpeed = 0;
        }

        //The AASHTO stopping distance formula. This is real.
        float stoppingDistance = (0.278f * Time.deltaTime * rb.linearVelocity.magnitude) + (Mathf.Pow(rb.linearVelocity.magnitude, 2) / (254 * (0.7f + 1)));

        //Multiply by the mass of the vehicle to determine brakeForce
        float brakeForce = rb.mass * (rb.linearVelocity.magnitude / stoppingDistance);

        //Ternary if statement (short hand if)
        //if isBraking is true, currentBrakeForce is the math above, if false it's 0
        currentBrakeForce = isBraking ? brakeForce : 0;

        //FINALLY send that to ApplyBrakes()
        ApplyBrakes(currentBrakeForce);
    }

    void ApplyBrakes(float brakez)
    {
        //Apply formula to all wheels
        frontLeftCollider.brakeTorque = brakez;
        frontRightCollider.brakeTorque = brakez;
        rearLeftCollider.brakeTorque = brakez;
        rearRightCollider.brakeTorque = brakez;
    }

    private void HandleSteering()
    {
        //Steering Angle is the maxSteeringAngle * h
        currentSteeringAngle = maxSteeringAngle * h;

        frontLeftCollider.steerAngle = currentSteeringAngle;
        frontRightCollider.steerAngle = currentSteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateOneWheel(frontLeftCollider, frontLeftMesh);
        UpdateOneWheel(frontRightCollider, frontRightMesh);
        UpdateOneWheel(rearLeftCollider, rearLeftMesh);
        UpdateOneWheel(rearRightCollider, rearRightMesh);
    }

    void UpdateOneWheel (WheelCollider w, Transform t)
    {
        Vector3 pos;
        Quaternion rot;

        //WheelColliders have built-in funcion "GetWorldPose() which returns pos and rot. We use that to move the meshes.
        w.GetWorldPose(out pos, out rot);

        //Make the wheel meshes' pos and rot the same as the colliders
        t.position = pos;
        t.rotation = rot;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check tp see what the player collided with.
        //if they hit the goal flag, call Lap() on the GM
        //If they hit the checkpoint, set checkpointPassed to true.
        if (other.gameObject.layer == 8)
        {
            //Lap() is a public func in th GM
            timer.GameEnd();
            ScoreD1.SetActive(true);
            ScoreD2.SetActive(true);
            goalReached = true;
        }

        if (other.gameObject.name == "Checkpoint")
        {
            gm.checkpointPassed = true;
        }

        if (other.gameObject.layer == 7 || other.gameObject.layer == 6)
        {
            SceneManager.LoadScene("RallyMap");
        }
        
    }


}

//enum is a list of things. Unity only reads it as an index of things. It cannot parse what a thing is.
// Enums are a type of class object, so they can be defined outside of the main class.
public enum driveTrain
{
    RWD, FWD
}
