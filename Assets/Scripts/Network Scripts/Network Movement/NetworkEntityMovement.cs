using UnityEngine;

public class NetworkEntityMovement : NetworkAbstractBaseMovement
{
    [SerializeField] private float overrideAccelerationModifier;
    protected override void Start()
    {
        base.Start();
        accelerationModifier = overrideAccelerationModifier;
    }
}
