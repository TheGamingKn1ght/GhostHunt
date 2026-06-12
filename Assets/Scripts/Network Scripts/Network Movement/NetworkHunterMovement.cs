using UnityEngine;

public class NetworkHunterMovement : NetworkAbstractBaseMovement
{
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jogSpeed;
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
        if (CanSprint() && NetworkInputManager.sprintInput)
        {
            moveSpeed = sprintSpeed;
            staminaObj.UseStamina();
        }
        else if(!CanSprint() && NetworkInputManager.sprintInput)
        {
            moveSpeed = jogSpeed;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            staminaObj.RegenerateStamina();
        }
    }
    
    private bool CanSprint()
    {
        return staminaObj.HasStamina;// && NetworkInputManager.sprintInput;//boollogic; (future)
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
