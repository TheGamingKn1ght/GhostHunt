using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using System;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;

public class NetworkInputManager: NetworkBehaviour
{
    public static InputActions inputActions;

    public static Vector2 movementInputs;
    public static Vector2 lookInputs;
    public static bool sprintInput;
    public static bool crouchInput;

    public static event Action onSprintToggle;
    public static event Action onCrouchToggle;

    private void Awake()
    {
        inputActions = new InputActions();

        inputActions.Look.MouseX.performed += ctx => lookInputs.x = ctx.ReadValue<float>();
        inputActions.Look.MouseY.performed += ctx => lookInputs.y = ctx.ReadValue<float>();
    }

    private void Start()
    {
        if (!IsOwner) { this.enabled = false; }

        inputActions.Enable();
    }

    private void OnEnable()
    {
        inputActions.Movement.MovementKeys.performed += Move;
        inputActions.Movement.MovementKeys.canceled += Move;

        inputActions.Movement.Sprint.performed += OnSprintToggle;
        inputActions.Movement.Sprint.canceled += OnSprintToggle;

        inputActions.Movement.Crouch.performed += OnCrouchToggle;
        inputActions.Movement.Crouch.canceled += OnCrouchToggle;
    }

    private void OnDisable()
    {
        inputActions.Movement.MovementKeys.performed -= Move;
        inputActions.Movement.MovementKeys.canceled -= Move;

        inputActions.Movement.Sprint.performed -= OnSprintToggle;
        inputActions.Movement.Sprint.canceled -= OnSprintToggle;

        inputActions.Movement.Sprint.performed -= OnCrouchToggle;
        inputActions.Movement.Sprint.canceled -= OnCrouchToggle;
    }

    #region Input Callbacks

        #region Movement Callbacks

        private void Move(InputAction.CallbackContext ctx)
        {
            movementInputs = ctx.ReadValue<Vector2>();
        }

        private void OnSprintToggle(InputAction.CallbackContext ctx)
        {
            sprintInput = Convert.ToBoolean(ctx.ReadValue<float>());
            onSprintToggle?.Invoke();
        }

        private void OnCrouchToggle(InputAction.CallbackContext ctx)
        {
            crouchInput = Convert.ToBoolean(ctx.ReadValue<float>());
            onCrouchToggle?.Invoke();
        }

    #endregion

    #endregion
}
