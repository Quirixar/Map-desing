using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    public float destroyDelay = 0f;
    private HealthSystem hs;
    private bool isDead = false;


    void Awake()
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
        Destroy(gameObject, destroyDelay);
    }

}
