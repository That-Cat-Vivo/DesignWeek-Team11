using UnityEngine;
using UnityEngine.InputSystem;

public class TurretControl : MonoBehaviour
{
    
    [field: SerializeField] public Rigidbody rb { get; private set; }
    [field: SerializeField] public Camera cam { get; private set; }
    [field: SerializeField] public float LookSpeed { get; private set; }

    //Look varis
    public float lookSensitivity;
    //For smooth movement
    public float smoothing = 1.5f;

    private Vector3 lookValue;

    //What Raycast will detect
    LayerMask targetSurface;


    

    //Player Input
    private PlayerInput PlayerInput;
    private InputAction InputActionLook;
    private InputAction InputActionFire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Grab the turret's Rigidbody for position info.
        Rigidbody rb = GetComponent<Rigidbody>();
        Quaternion turretTurn = rb.transform.rotation;

        //Choose what layer is our targetSurface
        targetSurface = LayerMask.GetMask("Obstacle");
    }
    public void AssignPlayerInputDevice(PlayerInput playerInput)
    {
        // Record our player input (ie controller).
        PlayerInput = playerInput;
        // Find the references to the "Move" and "Jump" actions inside the player input's action map
        // Here I specify "Player/" but it in not required if assigning the action map in PlayerInput inspector.
        
        InputActionLook = playerInput.actions.FindAction($"Player/Look");
        InputActionFire = playerInput.actions.FindAction($"Player/Attack");
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if (rb == null)
        {
            Debug.Log($"{name}'s {nameof(PlayerController)}.{nameof(Rigidbody)} is null.");
            return;
        }
        //calculations for turret rotation

        //lookValue acts as a storage point for the player's input.
        lookValue = InputActionLook.ReadValue<Vector2>();

        //Floats separate the x and y values.
        float y = lookValue.y;
        float x = lookValue.x;
        
        //The turret body and player camera are separately controlled. 
        //The turret turns on the x axis while the camera turns on the y axis.
        //This prevents the axis of rotation from effecting each other.
        transform.Rotate(new Vector3(0, x * 5, 0));
        cam.transform.Rotate(new Vector3(-y * 5, 0, 0));

        //Do raycast
        RaycastHit hit;

        //If ray hits a collider on the obstacle layer Firing is possible
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, targetSurface))
        {
            Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            Debug.Log("Hitting");

            //If the attack button is pressed, destroy the detected object.
            if (InputActionFire.IsPressed())
            {
                Destroy(hit.collider.gameObject);
            }

        }

        //If ray does not hit an obstacle, do nothing.
        else
        {
            Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("NotHitting");
            
        }

        
        

    }
}
