using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || triggered) return;
        {
            triggered = true;
            var rb2d = other.GetComponent<Rigidbody2D>();
            if (rb2d != null ) rb2d.linearVelocity = Vector2.zero;
            var movement = other.GetComponent<PlayerController>();
            if (movement != null) movement.enabled = false;
            var attack = other.GetComponent<PlayerAttack>();
            if (attack != null ) attack.enabled = false;
            if (UIManager.Instance != null) UIManager.Instance.ShowVictory();
        }
    }
}