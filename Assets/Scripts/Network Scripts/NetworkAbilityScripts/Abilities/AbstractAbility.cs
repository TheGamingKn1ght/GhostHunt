using UnityEngine;
using Unity.Netcode;

public abstract class AbstractAbility : NetworkBehaviour
{
    protected Rigidbody rb;

    protected virtual void Start()
    {
        if (!IsOwner) this.enabled = false;
        rb = GetComponent<Rigidbody>();
    }
}

