using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkInputManager))]
public abstract class NetworkAbstractBaseMovement : NetworkBehaviour
{
    [SerializeField] protected float baseMoveSpeed;

    protected float moveSpeed;
    protected Rigidbody rb;

    private void Start()
    {
        if (!IsOwner) this.enabled = false;

        rb = GetComponent<Rigidbody>();
        moveSpeed = baseMoveSpeed;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public virtual void Move()
    {
        Vector3 movement = new Vector3(NetworkInputManager.movementInputs.x, 0f, NetworkInputManager.movementInputs.y).normalized;
        movement = rb.transform.TransformDirection(movement);

        rb.AddForce(movement * moveSpeed, ForceMode.Force);
    }
}
