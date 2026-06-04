using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkInputManager: NetworkBehaviour
{
    public static InputActions inputActions;

    public static Vector2 movementInputs;
    public static Vector2 lookInputs;

    public static event System.Action OnSprintInput;

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

        inputActions.Movement.Sprint.performed += ctx => OnSprintInput?.Invoke();
    }

    private void OnDisable()
    {
        inputActions.Movement.MovementKeys.performed -= Move;
        inputActions.Movement.MovementKeys.canceled -= Move;
    }

    #region Input Callbacks

        #region Movement Callbacks

        private void Move(InputAction.CallbackContext ctx)
        {
            movementInputs = ctx.ReadValue<Vector2>();
        }

    #endregion

    #endregion
}
