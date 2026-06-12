using UnityEngine;

public class NetworkHunterMovement : NetworkAbstractBaseMovement
{
    [SerializeField] private float sprintSpeed;
    NetworkHunterStamina staminaObj;

    protected override void Start()
    {
        base.Start();
        staminaObj = GetComponent<NetworkHunterStamina>();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void Update()
    {
        Sprint();
    }

    private void Sprint()
    {
        if (CanSprint())
        {
            moveSpeed = sprintSpeed;
            staminaObj.UseStamina();
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            staminaObj.RegenerateStamina();
        }
    }
    
    private bool CanSprint()
    {
        return staminaObj.HasStamina && NetworkInputManager.sprintInput;//boollogic; (future)
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
