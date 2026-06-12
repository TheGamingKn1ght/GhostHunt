using Unity.Netcode;
using UnityEngine;

public class NetworkHunterStamina : NetworkBehaviour
{
    [SerializeField] private float staminaDuration = 10;
    private float staminaTimer;

    private bool hasStamina;
    public bool HasStamina { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HasStamina = true;
        staminaTimer = staminaDuration;
    }

    public void UseStamina()
    {
        staminaTimer -= Time.deltaTime;
        if (staminaTimer <= 0) 
        { 
            staminaTimer = 0;
            HasStamina = false;
        }
    }

    public void RegenerateStamina()
    {
        staminaTimer = Mathf.Clamp(staminaTimer,0f,10f) + (Time.deltaTime * 0.5f);
        if(staminaTimer > (staminaDuration * 0.5f)) { HasStamina = true; }
    }

}
