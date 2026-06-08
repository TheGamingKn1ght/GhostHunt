using UnityEngine;

public class NetworkHunterMovement : NetworkAbstractBaseMovement
{
    [SerializeField] private float sprintSpeed;

    private void Sprint()
    {
        moveSpeed = NetworkInputManager.sprintInput ? sprintSpeed : baseMoveSpeed;
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
