using UnityEngine;

public class NetworkEntityMovement : NetworkAbstractBaseMovement
{
    [Header("Advanced Movement Variables")]
    [SerializeField] private float overrideAccelerationModifier;
    protected override void Start()
    {
        base.Start();
        accelerationModifier = overrideAccelerationModifier;
    }
}
