// LevelExit.cs
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            LevelManager.Instance.GoToNextLevel();
        }
    }
}