using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ReTryButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(reTryLevel);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(reTryLevel);
    }

    private void reTryLevel()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.RetryCurrentLevel();
        }
        else Debug.LogWarning("Funciono :) #2");
    }
}