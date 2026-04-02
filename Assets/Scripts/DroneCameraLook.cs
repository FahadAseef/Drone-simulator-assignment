using UnityEngine;

public class DroneCameraLook : MonoBehaviour
{
    [Header("References")]
    public Transform droneBody;       // The main Drone object
    public Transform cameraPivot;     // CameraPivot object

    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public float minTilt = -45f;
    public float maxTilt = 60f;

    private float pitch = 0f;
    private float yawOffset = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
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
}