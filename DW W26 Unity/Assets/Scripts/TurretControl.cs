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
    

    //Player Input
    private PlayerInput PlayerInput;
    private InputAction InputActionLook;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Grab the turret's Rigidbody for position info.
        Rigidbody rb = GetComponent<Rigidbody>();
        Quaternion turretTurn = rb.transform.rotation;
    }
    public void AssignPlayerInputDevice(PlayerInput playerInput)
    {
        // Record our player input (ie controller).
        PlayerInput = playerInput;
        // Find the references to the "Move" and "Jump" actions inside the player input's action map
        // Here I specify "Player/" but it in not required if assigning the action map in PlayerInput inspector.
        
        InputActionLook = playerInput.actions.FindAction($"Player/Look");
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb == null)
        {
            Debug.Log($"{name}'s {nameof(PlayerController)}.{nameof(Rigidbody)} is null.");
            return;
        }
        lookValue = InputActionLook.ReadValue<Vector2>();
        float y = lookValue.y;
        float x = lookValue.x;
        
        transform.Rotate(new Vector3(0, x * 5, 0));
        cam.transform.Rotate(new Vector3(-y * 5, 0, 0));
    }
}
