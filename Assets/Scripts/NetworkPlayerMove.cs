using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkInputManager))]
public class NetworkPlayerMove : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;

    private void Start()
    {
        if (!IsOwner) return;

        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(NetworkInputManager.movementInputs.x, 0f, NetworkInputManager.movementInputs.y).normalized;
        movement = rb.transform.TransformDirection(movement);

        rb.AddForce(movement * moveSpeed, ForceMode.Force);
    }

   
}
