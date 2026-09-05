using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class NextLevelButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(goToNextLevel);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(goToNextLevel);
    }

    private void goToNextLevel()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.GoToNextLevel();
        }
        else Debug.LogWarning("Funciono :)");
    }
}
