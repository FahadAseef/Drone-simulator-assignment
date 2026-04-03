using UnityEngine;

public class DroneCameraLook : MonoBehaviour
{
    [Header("References")]
    public Transform droneBody;              // Main Drone
    public Transform cameraPivot;            // CameraPivot
    public TargetDetector targetDetector;    // Drag TargetDetector here

    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public float minTilt = -45f;
    public float maxTilt = 60f;

    [Header("Lock Settings")]
    public Vector3 targetOffset = new Vector3(0f, 1.5f, 0f); // Aim at target center

    private float pitch = 0f;
    private float yawOffset = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (targetDetector != null && targetDetector.lockedTarget != null)
        {
            HandleLockedLook();
        }
        else
        {
            HandleLook();
        }
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Left / Right camera pan
        yawOffset += mouseX;

        // Up / Down camera tilt
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minTilt, maxTilt);

        // Apply rotation to CameraPivot only
        cameraPivot.localRotation = Quaternion.Euler(pitch, yawOffset, 0f);
    }

    void HandleLockedLook()
    {
        Transform target = targetDetector.lockedTarget;
        if (target == null) return;

        // Aim at target center
        Vector3 targetPos = target.position + targetOffset;

        // Direction from camera to target
        Vector3 direction = targetPos - cameraPivot.position;

        // Exact look rotation
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Remove unwanted roll
        Vector3 euler = lookRotation.eulerAngles;
        euler.z = 0f;

        // Apply exact rotation
        cameraPivot.rotation = Quaternion.Euler(euler);

        // Sync values so unlocking feels smooth
        pitch = euler.x;
        if (pitch > 180f) pitch -= 360f;

        yawOffset = euler.y;
    }
}