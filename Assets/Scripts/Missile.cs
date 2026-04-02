using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Missile Settings")]
    public float speed = 20f;
    public float rotateSpeed = 5f;
    public float hitDistance = 1f; // how close before hit

    [HideInInspector] public Transform target;

    [Header("Explosion")]
    public GameObject explosionPrefab;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        // Rotate toward target
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);

        // Move directly toward target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // If close enough, destroy target
        if (Vector3.Distance(transform.position, target.position) <= hitDistance)
        {
            HitTarget(target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            HitTarget(other.transform);
        }
    }

    void HitTarget(Transform hitTarget)
    {
        Debug.Log("MISSILE HIT: " + hitTarget.name);

        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, hitTarget.position, Quaternion.identity);
            Destroy(fx, 1f);
        }

        Destroy(hitTarget.gameObject);
        Destroy(gameObject);
    }
}