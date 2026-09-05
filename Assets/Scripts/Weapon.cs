using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Daño")]
    public int damage = 4;

    [Header("Duración del golpe activo")]
    public float activeTime = 0.15f;

    [Header("Posicionamiento respecto al jugador")]
    public float distanceFromPlayer = 0.5f;

    private Collider2D hitCollider;
    private HashSet<HealthSystem> alreadyHit = new HashSet<HealthSystem>();

    private void Awake()
    {
        hitCollider = GetComponent<Collider2D>();
        hitCollider.enabled = false;
    }

    public void AimAndSwing(Vector2 direction)
    {
        direction.Normalize();
        transform.localPosition = direction * distanceFromPlayer;


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);

        Swing();
    }

    public void Swing()
    {
        StopAllCoroutines();
        StartCoroutine(SwingRoutine());
    }

    private IEnumerator SwingRoutine()
    {
        alreadyHit.Clear();
        hitCollider.enabled = true;
        yield return new WaitForSeconds(activeTime);
        hitCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        HealthSystem enemyHealth = other.GetComponent<HealthSystem>();
        if (enemyHealth == null || alreadyHit.Contains(enemyHealth)) return;

        alreadyHit.Add(enemyHealth);
        enemyHealth.TakeDamage(damage);
        Debug.Log("Golpeaste al enemigo");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);
    }
}