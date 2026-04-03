using UnityEngine;
using UnityEngine.EventSystems;

public class DroneCameraLook : MonoBehaviour
{
    [Header("References")]
    public Transform droneBody;            
    public Transform cameraPivot;        
    public TargetDetector targetDetector;

    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public float minTilt = -45f;
    public float maxTilt = 60f;

    [Header("Lock Settings")]
    public Vector3 targetOffset = new Vector3(0f, 1.5f, 0f); // Aim at target center

    private float lookUpDown = 0f;
    private float rotateLeftRight = 0f;

    //void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //}

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
#if UNITY_EDITOR || UNITY_STANDALONE
        // PC Controls
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotateLeftRight += mouseX;
        lookUpDown -= mouseY;
#else
    // MOBILE TOUCH CONTROLS
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);

        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        return;

        // Only move camera when finger is moving
        if (touch.phase == TouchPhase.Moved)
        {
            Vector2 delta = touch.deltaPosition;

            float touchX = delta.x * mouseSensitivity * 0.02f * Time.deltaTime;
            float touchY = delta.y * mouseSensitivity * 0.02f * Time.deltaTime;

            rotateLeftRight += touchX;
            lookUpDown -= touchY;
        }
    }
#endif

        lookUpDown = Mathf.Clamp(lookUpDown, minTilt, maxTilt);

        cameraPivot.localRotation = Quaternion.Euler(lookUpDown, rotateLeftRight, 0f);
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
        lookUpDown = euler.x;
        if (lookUpDown > 180f) lookUpDown  -= 360f;

        rotateLeftRight = euler.y;
    }
}