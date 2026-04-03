using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Missile Settings")]
    public float speed = 30f;
    public float rotateSpeed = 5f;
    public float hitDistance = 2f;

    [Header("Homing")]
    public float closeRange = 10f;          // When missile gets close
    public float closeTurnMultiplier = 2.5f;
    public float closeSpeedMultiplier = 0.7f;

    [HideInInspector] public Transform target;

    [Header("Explosion")]
    public GameObject explosionPrefab;

    private bool hasHit = false;

    void Update()
    {
        if (hasHit) return;

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        HomeToTarget();
    }

    void HomeToTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        // Increase turning when close
        float currentRotateSpeed = rotateSpeed;
        float currentSpeed = speed;

        if (distance < closeRange)
        {
            currentRotateSpeed *= closeTurnMultiplier;
            currentSpeed *= closeSpeedMultiplier;
        }

        // Smoothly rotate toward target
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentRotateSpeed * Time.deltaTime);

        // Move forward only
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // Proximity hit
        if (distance <= hitDistance)
        {
            HitTarget(target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Target"))
        {
            HitTarget(other.transform);
        }
    }

    void HitTarget(Transform hitTarget)
    {
        if (hasHit) return;
        hasHit = true;

        Debug.Log("MISSILE HIT: " + hitTarget.name);

        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, hitTarget.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Destroy(hitTarget.gameObject);
        Destroy(gameObject);
    }
}