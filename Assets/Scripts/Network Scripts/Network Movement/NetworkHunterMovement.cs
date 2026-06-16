using UnityEngine;

public class NetworkHunterMovement : NetworkAbstractBaseMovement
{
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jogSpeed;
    NetworkHunterStamina staminaObj;

    private float colliderHeight;
    private float colliderCrouchHeight; 
    private Vector3 playerHeight;
    private Vector3 crouchHeight;

    protected override void Start()
    {
        base.Start();
        staminaObj = GetComponent<NetworkHunterStamina>();

        //Crouch Variables
        playerHeight = transform.localScale;
        crouchHeight = new Vector3(playerHeight.x, playerHeight.y * 0.5f, playerHeight.z);
        colliderHeight = this.collider.height;
        colliderCrouchHeight = colliderHeight * 0.5f;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void Update()
    {
        Sprint();
        Crouch();
    }

    private void Crouch()
    {
        if (NetworkInputManager.crouchInput)
        {
            rb.transform.localScale = crouchHeight;
            collider.height = colliderCrouchHeight;
            moveSpeed = crouchSpeed;
        }
        else
        {
            rb.transform.localScale = playerHeight;
            collider.height = colliderHeight;
            moveSpeed = baseMoveSpeed;
        }

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
        NetworkInputManager.onCrouchToggle += Crouch;
    }

    private void OnDisable()
    {
        NetworkInputManager.onSprintToggle -= Sprint;
        NetworkInputManager.onCrouchToggle -= Crouch;
    }
    #endregion
}
