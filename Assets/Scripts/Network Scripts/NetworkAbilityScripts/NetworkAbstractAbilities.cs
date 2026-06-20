using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class NetworkAbstractAbilities : NetworkBehaviour
{
    [SerializeField] protected AbstractAbility UseableAbility;

    protected virtual void UseAbility()
    {
        Debug.Log("using base ability functionality");
    }
}
