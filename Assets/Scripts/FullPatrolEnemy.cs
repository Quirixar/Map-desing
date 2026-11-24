using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class FullPatrolEnemy : MonoBehaviour
{
    private enum State
    {
        Patrol, Wait, Chase, Return
    }
    private State currentState = State.Patrol;

    [Header("Ruta")]
    public Transform [] wayPoints;
    private int currentIndex = 0;

    [Header("Movimientos")]
    public float moveSpeed = 2f;
    public float chaseSpeed = 2f;
    public float returnSpeed = 2f;
    public float arrivalRadius = 0.15f;
    public float stopChaseDistance = 0.6f;

    [Header("Espera")]
    public float waitTime = 4f;
    private float waitTimer = 0f;

    [Header("Detección")]
    public float detectionRadius = 4.0f;
    public float loseRadius = 5f;
    public LayerMask playerLayer;

    [Header("Daño")]
    public int damageAmount = 1;

    [Header("Vida")]
    public int playerHealth = 10;

    [Header("Detección de paredes")]
    public float detectionDistance = 0.5f;
    public LayerMask wallLayer;

    [Header("Componentes internos")]
    private Vector2 originPosition;
    private Transform playerTarget;
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
        CheckForPlayer();
        switch (currentState)
        {
            case State.Patrol: TickPatrol(); break;
            case State.Wait: TickWait(); break;
            case State.Chase: TickChase(); break;
            case State.Return: TickReturn(); break;
        }
    }

    private void CheckForPlayer()
    {
        if (currentState == State.Chase) return;
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (hit != null)
        {
            playerTarget = hit.transform;
            currentState = State.Chase;
        }
    }

    private void TickPatrol()
    {
        if (wayPoints == null || wayPoints.Length == 0) return;
        MoveTowards(wayPoints[currentIndex].position, moveSpeed);
        float dist = Vector2.Distance(transform.position, wayPoints[currentIndex].position);
        if (dist < arrivalRadius)
        {
            currentIndex = (currentIndex + 1) % wayPoints.Length;
            rb2d.linearVelocity = Vector2.zero;
            waitTimer = waitTime;
            currentState = State.Wait;
        }
    }

    private void TickWait()
    {
        rb2d.linearVelocity = Vector2.zero;
        waitTimer -= Time.fixedDeltaTime;
        if (waitTimer <= 0f) currentState = State.Patrol;
    }

    private void TickChase()
    {
        if (playerTarget == null)
        {
            currentState = State.Return; return;
        }
        float dist = Vector2.Distance(transform.position, playerTarget.position);
        if (dist > loseRadius)
        {
            playerTarget = null;
            currentState = State.Return; return;
        }
        if (dist <= stopChaseDistance)
        {
            rb2d.linearVelocity = Vector2.zero; return;
        }
        MoveTowards(playerTarget.position, chaseSpeed);

    }

    private void TickReturn()
    {
        float dist = Vector2.Distance(transform.position, originPosition);
        if (dist < 0.1f)
        {
            rb2d.linearVelocity = Vector2.zero;
            currentState = State.Patrol; return;
        }
        MoveTowards(originPosition, returnSpeed);
    }

    private void MoveTowards(Vector2 target, float speed)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        rb2d.linearVelocity = dir * speed;
        if (dir.x > 0.01f) sr.flipX = false;
        if (dir.x < -0.01f) sr.flipX = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var dmg = other.gameObject.GetComponent<HealthSystem>();
        if (dmg != null) dmg.TakeDamage(damageAmount);
    }

    bool IsWallNear()
    {
        Vector2 facingDirection = sr.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, detectionDistance, wallLayer);
        Debug.DrawRay(transform.position, facingDirection * detectionDistance, hit.collider != null ? Color.green:Color.red);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, loseRadius);
    }
}
