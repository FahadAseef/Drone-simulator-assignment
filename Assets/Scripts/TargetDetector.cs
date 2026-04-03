using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectDistance = 100f;
    public LayerMask targetLayer;

    [Header("Targets")]
    public Transform currentTarget;
    public Transform lockedTarget;

    [Header("Missile Settings")]
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;

    [Header("Rotation Change")]
    public Transform objectToRotate;
    public float normalXRotation = 10f;
    public float lockedXRotation = 3f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        SetObjectXRotation(normalXRotation);
    }

    void Update()
    {
        DetectTarget();

        HandleKeyboardInput();
    }

    void DetectTarget()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectDistance, targetLayer))
        {
            if (hit.collider.CompareTag("Target"))
            {
                currentTarget = hit.collider.transform;
                return;
            }
        }

        currentTarget = null;
    }

    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            LockTarget();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseTarget();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchMissile();
        }
    }


    // UI BUTTON FUNCTIONS

    public void LockTarget()
    {
        if (currentTarget != null)
        {
            lockedTarget = currentTarget;
            Debug.Log("LOCKED TARGET: " + lockedTarget.name);

            // Change X rotation to locked angle
            SetObjectXRotation(lockedXRotation);
        }
        else
        {
            Debug.Log("No target in crosshair to lock!");
        }
    }

    public void ReleaseTarget()
    {
        if (lockedTarget != null)
        {
            Debug.Log("UNLOCKED TARGET: " + lockedTarget.name);
            lockedTarget = null;

            // Change X rotation back to normal
            SetObjectXRotation(normalXRotation);
        }
        else
        {
            Debug.Log("No locked target to release!");
        }
    }

    public void LaunchMissile()
    {
        if (lockedTarget != null)
        {
            FireMissile();
        }
        else
        {
            Debug.Log("No locked target to fire at!");
        }
    }

    void FireMissile()
    {
        GameObject missileObj = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);

        Missile missileScript = missileObj.GetComponent<Missile>();
        missileScript.target = lockedTarget;

        Debug.Log("MISSILE FIRED at: " + lockedTarget.name);
    }

    void SetObjectXRotation(float xValue)
    {
        if (objectToRotate == null) return;

        Vector3 currentRot = objectToRotate.localEulerAngles;
        objectToRotate.localEulerAngles = new Vector3(xValue, currentRot.y, currentRot.z);
    }

    void OnDrawGizmos()
    {
        if (cam == null) return;

        // Detection ray
        Gizmos.color = Color.red;
        Gizmos.DrawRay(cam.transform.position, cam.transform.forward * detectDistance);

        // Locked target line
        if (lockedTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(cam.transform.position, lockedTarget.position);
        }
    }
}