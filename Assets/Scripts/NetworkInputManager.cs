using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class NetworkInputManager: NetworkBehaviour
{
    public static InputActions inputActions;

    public static Vector2 movementInputs;
    public static Vector2 aimInputs;

    private void Awake()
    {
        inputActions = new InputActions();
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

        inputActions.Look.Aim.performed += Look;
        inputActions.Look.Aim.canceled += Look;
    }

    private void OnDisable()
    {
        inputActions.Movement.MovementKeys.performed -= Move;
        inputActions.Movement.MovementKeys.canceled -= Move;

        inputActions.Look.Aim.performed -= Look;
        inputActions.Look.Aim.canceled -= Look;
    }

    #region Input Callbacks

        #region Movement Callbacks

        private void Move(InputAction.CallbackContext ctx)
        {
            movementInputs = ctx.ReadValue<Vector2>();
        }

    #endregion

        #region MouseMovement Callbacks

        private void Look(InputAction.CallbackContext ctx)
        {
            aimInputs = ctx.ReadValue<Vector2>();
        }

        #endregion

    #endregion
}
