using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public int damageAmount = 10;
    public string targetTag = "Player";


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag(targetTag))
        {
            HealthSystem health = collision.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
                Debug.Log("Recibiste daño." + damageAmount);
            }
        }

    }
}
