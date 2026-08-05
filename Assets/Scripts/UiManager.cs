using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player Health Bar")]
    public Image PlayerHealthFill;
    public int PlayerMaxHealth = 100;

    [Header("Boss Health Bar")]
    public Image BossHealthFill;
    public int BossMaxHealth = 800;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject); return;
        }


    }

    public void UpdatePlayerHealth(int currentHealth)
    {
        if (PlayerHealthFill != null)
        {
            PlayerHealthFill.fillAmount = (float) currentHealth / PlayerMaxHealth;
        }
    }

    public void UpdateBossHealth(int currentHealth)
    {
        if (BossHealthFill != null)
        {
            BossHealthFill.fillAmount = (float)currentHealth / BossMaxHealth;
        }
    }

    /*public void ShowGameOver()
    {
        if (GameOverPanel != null) GameOverPanel.SetActive(true);
    }

    public void ShowVictory()
    {
        if (VictoryPanel != null) VictoryPanel.SetActive(true);
    }*/
}
