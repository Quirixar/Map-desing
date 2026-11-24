using UnityEngine;

public class LinealEnemy : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform waypointA;
    public Transform waypointB;

    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float arrivalRadius = 0.15f;

    [Header("Daño")]
    public int damageAmount = 1;

    private bool goingToB = true;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (waypointA == null || waypointB == null) return;

        Transform destination = goingToB ? waypointB : waypointA;

        Vector2 dir = ((Vector2)destination.position - (Vector2)transform.position).normalized;
        rb2d.linearVelocity = dir * moveSpeed;

        float dist = Vector2.Distance(transform.position, destination.position);
        if (dist < arrivalRadius) goingToB = !goingToB;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var dmg = other.GetComponent<IDamageable>();
        if (dmg != null) dmg.TakeDamage(damageAmount);
    }
}
