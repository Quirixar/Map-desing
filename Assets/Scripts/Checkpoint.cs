// Checkpoint.cs
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Vector3 lastCheckpointPosition;
    public static bool hasCheckpoint = false;

    [Header("Visual")]
    public SpriteRenderer indicatorSprite;
    public Color activatedColor = Color.blue;

    private bool activated = false;

    void OnEnable()
    {
        activated = false;
        if (indicatorSprite != null)
            indicatorSprite.color = Color.white;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;
            lastCheckpointPosition = transform.position;
            hasCheckpoint = true;

            if (indicatorSprite != null)
                indicatorSprite.color = activatedColor;

            Debug.Log("Checkpoint guardado en: " + transform.position);
        }
    }
}