using UnityEngine;

public class EnemyRoute : MonoBehaviour
{

    //---Variables públicas---
    //Ruta
    public Transform[] waitpoints;

    //Movimiento
    public float moveSpeed = 1f;
    public float arrivalRadius = 0.15f;
    public float waitTime = 0f;

    //Daño
    public int damageAmount = 1;

    //-Variables privadas-
    private int currentIndex = 0;
    private float waitTimer = 0;
    private bool isWaiting = false;
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;


    void Start()
    {
       rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
    }

    void FixedUpdate()
    {
        if (waitpoints ==null || waitpoints.Length == 0) return;
        if (isWaiting)
        {
            rb2d.linearVelocity = Vector2.zero;
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0f) isWaiting = false; return;
        }
        Transform destination = waitpoints[currentIndex];
        Vector2 dir = ((Vector2)destination.position - (Vector2)transform.position).normalized;
        rb2d.linearVelocity = dir * moveSpeed;
        float dist = Vector2.Distance(transform.position, destination.position);
        if (dist < arrivalRadius)
        {
            currentIndex = (currentIndex + 1) % waitpoints.Length;
            if (waitTime > 0f)
            {
                isWaiting = true;
                waitTimer = waitTime;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var dmg = other.GetComponent<IDamageable>();
        if (dmg != null) dmg.TakeDamage(damageAmount);
    }
}
