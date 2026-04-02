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

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        DetectTarget();
        HandleLockInput();
        HandleFireInput();
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

    void HandleLockInput()
    {
        // Press F to lock target
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentTarget != null)
            {
                lockedTarget = currentTarget;
                Debug.Log("LOCKED TARGET: " + lockedTarget.name);
            }
        }

        // Press R to unlock target
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (lockedTarget != null)
            {
                Debug.Log("UNLOCKED TARGET: " + lockedTarget.name);
                lockedTarget = null;
            }
        }
    }

    void HandleFireInput()
    {
        // Press Space to fire missile
        if (Input.GetKeyDown(KeyCode.Space))
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
    }

    void FireMissile()
    {
        GameObject missileObj = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);

        Missile missileScript = missileObj.GetComponent<Missile>();
        missileScript.target = lockedTarget;

        Debug.Log("MISSILE FIRED at: " + lockedTarget.name);
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