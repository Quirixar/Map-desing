using UnityEngine;
using System.Collections;
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Configuracion")]
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
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
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

    public void GoToNextLevel()
    {
        Checkpoint.hasCheckpoint = false;
        LoadLevel(currentConfig.nextLevel);
    }
}