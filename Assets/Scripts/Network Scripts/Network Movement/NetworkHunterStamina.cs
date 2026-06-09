using UnityEngine;

public class NetworkHunterStamina : NetworkHunterMovement
{
    [SerializeField] private float staminaDuration = 10;
    private float staminaTimer;
    public bool CanSprint { get; set; }
    public bool IsSprinting { get; set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CanSprint = true;
        staminaTimer = staminaDuration;
    }

    private void Update()
    {
        if (IsSprinting && staminaTimer > 0)
        {
            UseStamina();
        }
        if(!IsSprinting && staminaTimer < staminaDuration)
        {
            RegenerateStamina();
        }
    }

    private void UseStamina()
    {
        staminaTimer -= Time.deltaTime;
        if (staminaTimer <= 0) 
        { 
            staminaTimer = 0;
            CanSprint = false;
        }
    }

    private void RegenerateStamina()
    {
        staminaTimer += (Time.deltaTime * 0.5f);
        if (staminaTimer > staminaDuration) { staminaTimer = staminaDuration; }
        if(staminaTimer > 0) { CanSprint = true; }
    }

}
