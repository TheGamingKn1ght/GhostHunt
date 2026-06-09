using UnityEngine;

public class NetworkHunterMovement : NetworkAbstractBaseMovement
{
    [SerializeField] private float sprintSpeed;
    private bool canSprint;
    private bool isSprinting = false;

    private void Sprint()
    {
        if (canSprint)
        {
            moveSpeed = NetworkInputManager.sprintInput ? sprintSpeed : baseMoveSpeed;
            isSprinting = true;
        }
        else { isSprinting = false; }

    }

    #region Event Subscriptions
    private void OnEnable()
    {
        NetworkInputManager.onSprintToggle += Sprint;
    }

    private void OnDisable()
    {
        NetworkInputManager.onSprintToggle -= Sprint;
    }
    #endregion
}
