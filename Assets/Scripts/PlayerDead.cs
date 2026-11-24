using UnityEngine;

public class PlayerDead : MonoBehaviour
{
    public MonoBehaviour[] ScriptsToDisable;
    private HealthSystem hs;
    private bool isDead = false;

    private void Awake()
    {
        hs = GetComponent<HealthSystem>();
    }

    private void OnEnable()
    {
        if (hs != null) hs.onDeath.AddListener(HandDeath);
    }

    private void OnDisable()
    {
        if (hs != null) hs.onDeath.RemoveListener(HandDeath);
    }

    private void HandDeath()
    {
        if (isDead) return;
        isDead = true;
        foreach (var script in ScriptsToDisable)
        {
            if (script != null)script.enabled = false;
        }
        if (UIManager.Instance != null)UIManager.Instance.ShowGameOver();
    }

    public void ResetStatic()
    {
        isDead = false;
    }
}