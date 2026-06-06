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

    public static event Action onSprintToggle;
    


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
    }

    private void OnDisable()
    {
        inputActions.Movement.MovementKeys.performed -= Move;
        inputActions.Movement.MovementKeys.canceled -= Move;

        inputActions.Movement.Sprint.performed -= OnSprintToggle;
        inputActions.Movement.Sprint.canceled -= OnSprintToggle;
    }

    #region Input Callbacks

        #region Movement Callbacks

        private void Move(InputAction.CallbackContext ctx)
        {
            movementInputs = ctx.ReadValue<Vector2>();
        }

        private void OnSprintToggle(InputAction.CallbackContext ctx)
        {
            sprintInput = Mathf.Approximately(Math.Min(ctx.ReadValue<float>(),1),1);
            onSprintToggle?.Invoke();
        }

    #endregion

    #endregion
}
