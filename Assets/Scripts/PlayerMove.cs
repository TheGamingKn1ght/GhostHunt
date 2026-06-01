using UnityEngine;
using Unity.Netcode;

public class PlayerMove : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return; //If player does not belond to current client, voids movement code
        
        //Movement code
    }
}
