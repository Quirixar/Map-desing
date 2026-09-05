using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Configuración")]
    public LevelConfig startingLevel;
    private LevelConfig currentConfig;
    private GameObject currentLevelInstance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Checkpoint.hasCheckpoint = false;
        LoadLevel(startingLevel);
    }

    public void LoadLevel(LevelConfig config)
    {
        if (config == null)
        {
            Debug.Log("No hay mas niveles. Fin del juego.");
            return;
        }
        if (currentLevelInstance != null)
            Destroy(currentLevelInstance);
        currentConfig = config;
        currentLevelInstance = Instantiate(config.tilemapPrefab);
        StartCoroutine(MovePlayerAfterLoad());
    }

    private IEnumerator MovePlayerAfterLoad()
    {
        yield return null;
        var player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("LevelManager: No se encontro objeto con tag 'Player'. " +
                             " y de paso verifica que el jugador existe en la escena y tiene el tag asignado.");
            yield break;
        }

        player.SetActive(true);

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        var movement = player.GetComponent<PlayerController>();
        if (movement != null) movement.enabled = true;
        var attack = player.GetComponent<PlayerAttack>();
        if (attack != null) attack.enabled = true;

        // Este es el HealthSystem y el de arriba movimiento.
        var health = player.GetComponent<HealthSystem>();
        if (health != null) health.ResetHealth();
        var dead = player.GetComponent<PlayerDead>();
        if (dead != null) dead.ResetStatic();

        if (Checkpoint.hasCheckpoint)
        {
            player.transform.position = Checkpoint.lastCheckpointPosition;
        }

        else
        {
            var spawn = GameObject.FindWithTag("PlayerSpawn");
            if (spawn != null)
            {
                player.transform.position = spawn.transform.position;
            }
            else
            {
                Debug.LogWarning("LevelManager: No se encontro 'PlayerSpawn' en el nivel '" +
                                 currentConfig.levelName + "'. " +
                                 "Verifica que el objeto PlayerSpawn existe dentro del Prefab " +
                                 "y tiene el tag PlayerSpawn asignado.");
            }
        }
        Debug.Log("Nivel cargado: " + currentConfig.levelName);
    }

    public void RetryCurrentLevel()
    {
        if (UIManager.Instance.GameOverPanel != null)
            UIManager.Instance.GameOverPanel.SetActive(false);

        Checkpoint.hasCheckpoint = false;
        LoadLevel(currentConfig);
    }

    public void GoToNextLevel()
    {
        if (UIManager.Instance.VictoryPanel != null)
            UIManager.Instance.VictoryPanel.SetActive(false);

        Checkpoint.hasCheckpoint = false;
        LoadLevel(currentConfig.nextLevel);
    }
}