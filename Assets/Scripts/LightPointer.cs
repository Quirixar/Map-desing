using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class LightPointer : MonoBehaviour

{
    [Header("Referencia")]
    public Light2D spotlight;
    public Transform playertransform;
    private float lastdirectionX = 1f;
    //private float lastdirectionY = 1f;

    private InputSystem_Actions inputActions;

    private Vector2 lastDirection = Vector2.right;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }
    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    void Update()
    {
        if (spotlight == null || playertransform == null) return;
        Vector2 move = inputActions.Player.Move.ReadValue<Vector2>();
        if (move.sqrMagnitude > 0.01f) lastDirection = move.normalized;
        float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg;
        spotlight.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

    }
}