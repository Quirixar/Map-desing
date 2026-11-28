using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
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
    private Animator animator;
    private HealthSystem health; /// a;adi esto

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<HealthSystem>(); // a;adi esto
        originPosition = transform.position;

        //  Cuando HealthSystem reciba da'o
        if (health != null)
        {
            health.onTakeDamage.AddListener(PlayHurtAnimation);
        }
    }

    void FixedUpdate()
    {
        DetectPlayer();
        if (isChasing) ChasePlayer(); else ReturnToOrigin();
        UpdateAnimations();
    }

    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (hit != null)
        {
            playerTarget = hit.transform;
            isChasing = true;
        }
        else if (isChasing && playerTarget != null)
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
        Vector2 dir = ((Vector2)playerTarget.position - (Vector2)transform.position).normalized;
        rb2d.linearVelocity = dir * chaseSpeed;
    }

    void ReturnToOrigin()
    {
        float dist = Vector2.Distance(transform.position, originPosition);
        if (dist < 0.05f)
        {
            rb2d.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 dir = (originPosition - (Vector2)transform.position).normalized;
        rb2d.linearVelocity = dir * returnSpeed;
    }

    void UpdateAnimations()
    {
        Vector2 vel = rb2d.linearVelocity;
        bool isMoving = vel.sqrMagnitude > 0.01f;

        animator.SetBool("IsMoving", isMoving);

        if (isMoving)
        {
            Vector2 dir = vel.normalized;
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
        }
    }

    //hurt en la direccion actual
    public void PlayHurtAnimation()
    {
        if (animator != null) animator.SetTrigger("Hurt");
    }

    private void OnDestroy()
    {
        // desconectar el evento al destruirse
        if (health != null)
        {
            health.onTakeDamage.RemoveListener(PlayHurtAnimation);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
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