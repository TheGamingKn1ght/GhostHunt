using Unity.Netcode;
using UnityEngine;
using System.Collections;

public class NetworkAbstractAbilities : NetworkBehaviour
{
    //Replace int with type "Ability" once you create each individual ability
    NetworkList<int> Abilities = new NetworkList<int>(); 

    public virtual void UseAbility()
    {
        Debug.Log("using base ability functionality");
    }
}
