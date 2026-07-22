using UnityEngine;

public class WaiterEnemy : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform waypointA;
    public Transform waypointB;

    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float arrivalRadius = 0.15f;

    [Header("Pausa")]
    public float waitTime = 1.5f;

    [Header("Daño")]
    public int damageAmount = 1;

    private bool goingToB = true;
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
        if (waypointA == null || waypointB == null) return;
        if (isWaiting)
        {
            rb2d.linearVelocity = Vector2.zero;
            waitTimer -= Time.fixedDeltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                goingToB = !goingToB;
            } return;
        }

        Transform destination = goingToB ? waypointB:waypointA;
        Vector2 dir = ((Vector2) destination.position - (Vector2)transform.position).normalized;
        rb2d.linearVelocity = dir * moveSpeed;
        //rb2d.MovePosition(rb2d.position + dir * moveSpeed * Time.fixedDeltaTime);
        float dist = Vector2.Distance(transform.position, destination.position);
        if (dist < arrivalRadius)
        {
            isWaiting = true;
            waitTimer = waitTime;
        }



    }
}
