using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    // Variables públicas.
    // Detección.
    public float detectionRadius = 4f;
    public float loseRadius = 6f;
    public LayerMask playerLayer;

    // Movimientos.
    public float chaseSpeed = 4.0f;
    public float returnSpeed = 2.0f;

    // Daño.
    public int damageAmount = 1;

    // Variables privadas.
    private Vector2 originPosition;
    private Transform playerTarget;
    private bool isChasing = false;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        originPosition = transform.position;
    }

    void FixedUpdate()
    {
        DetectPlayer();
        if (isChasing) ChasePlayer(); else ReturnToOrigin();
    }

    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (hit != null)
        {
            playerTarget = hit.transform; isChasing = true;
        }
        else if (isChasing)
        {
            float d = Vector2.Distance(transform.position, playerTarget.position);
            if (d > loseRadius)
            {
                playerTarget = null;
                isChasing = false;
            }
        }
    }

    void ChasePlayer()
    {
        if (playerTarget == null) return;
        Vector2 dir = ((Vector2) playerTarget.position - (Vector2)transform.position).normalized;
        rb2d.linearVelocity = dir * chaseSpeed;
        if (dir.x > 0.01f) sr.flipX = false;
        if (dir.x < -0.01f) sr.flipX = true;

    }

    void ReturnToOrigin()
    {
        float dist = Vector2.Distance(transform.position, originPosition);
        if (dist < 0.01f)
        {
            rb2d.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 dir = (originPosition - (Vector2)transform.position).normalized;
        rb2d.linearVelocity = dir * returnSpeed;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var dmg = other.GetComponent<IDamageable>();
        if (dmg != null) dmg.TakeDamage(damageAmount);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRadius);
    }

}
