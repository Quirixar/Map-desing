using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Weapon sword;

    [Header("Cámara")]
    public Camera gameCamera;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
        if (gameCamera == null) gameCamera = Camera.main;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Attack.performed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (sword == null || gameCamera == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = gameCamera.ScreenToWorldPoint(mouseScreenPos);
        Vector2 direction = (Vector2)mouseWorldPos - (Vector2)transform.position;

        sword.AimAndSwing(direction);
    }
}