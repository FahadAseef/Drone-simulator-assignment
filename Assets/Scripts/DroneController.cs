using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float verticalSpeed = 5f;
    public float rotationSpeed = 100f;

    [Header("Joystick")]
    public Joystick moveJoystick;    
    public Joystick rotateJoystick;

    private Rigidbody rb;

    private float moveX;
    private float moveZ;
    private float moveY;
    private float yaw;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        // KEYBOARD INPUT
        float keyboardX = Input.GetAxis("Horizontal"); // A / D
        float keyboardZ = Input.GetAxis("Vertical");   // W / S

        float keyboardYaw = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) keyboardYaw = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) keyboardYaw = 1f;

        float keyboardY = 0f;
        if (Input.GetKey(KeyCode.Q)) keyboardY = 1f;   // Up
        if (Input.GetKey(KeyCode.E)) keyboardY = -1f;  // Down


        // JOYSTICK INPUT
        float joystickX = 0f;
        float joystickZ = 0f;

        float joystickYaw = 0f;
        float joystickVertical = 0f;

        if (moveJoystick != null)
        {
            joystickX = moveJoystick.Horizontal;
            joystickZ = moveJoystick.Vertical;
        }

        if (rotateJoystick != null)
        {
            joystickYaw = rotateJoystick.Horizontal;   // Left / Right
            joystickVertical = rotateJoystick.Vertical; // Up / Down
        }

        // COMBINE INPUTS
        moveX = Mathf.Clamp(keyboardX + joystickX, -1f, 1f);
        moveZ = Mathf.Clamp(keyboardZ + joystickZ, -1f, 1f);

        yaw = Mathf.Clamp(keyboardYaw + joystickYaw, -1f, 1f);

        // Q/E + joystick Up/Down
        moveY = Mathf.Clamp(keyboardY + joystickVertical, -1f, 1f);
    }

    void FixedUpdate()
    {
        MoveDrone();
        RotateDrone();
    }

    void MoveDrone()
    {
        Vector3 move = (transform.forward * moveZ + transform.right * moveX).normalized;
        Vector3 velocity = new Vector3(move.x * moveSpeed, moveY * verticalSpeed, move.z * moveSpeed);
        rb.linearVelocity = velocity;
    }

    void RotateDrone()
    {
        Quaternion turnOffset = Quaternion.Euler(0f, yaw * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnOffset);
    }
}