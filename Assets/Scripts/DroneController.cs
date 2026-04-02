using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float verticalSpeed = 5f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;

    private float moveX;
    private float moveZ;
    private float moveY;
    private float yaw;

    //public Transform droneBody;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void Update()
    {
        // Horizontal movement input
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical"); 

        // Vertical movement input
        moveY = 0f;
        if (Input.GetKey(KeyCode.Q)) moveY = 1f;   
        if (Input.GetKey(KeyCode.E)) moveY = -1f; 

        // Rotation input
        yaw = 0f;
        if (Input.GetKey(KeyCode.LeftArrow)) yaw = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) yaw = 1f;
    }

    void FixedUpdate()
    {
        MoveDrone();
        RotateDrone();
        //RotateBody();
    }

    void MoveDrone()
    {
        Vector3 move = transform.forward * moveZ + transform.right * moveX;
        Vector3 velocity = new Vector3(move.x * moveSpeed, moveY * verticalSpeed, move.z * moveSpeed);
        rb.linearVelocity = velocity;
    }

    void RotateDrone()
    {
        Quaternion turnOffset = Quaternion.Euler(0f, yaw * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnOffset);
    }

    //void RotateBody()
    //{
    //    if (droneBody == null) return;

    //    droneBody.Rotate(0f, yaw * rotationSpeed * Time.fixedDeltaTime, 0f);
    //}

}