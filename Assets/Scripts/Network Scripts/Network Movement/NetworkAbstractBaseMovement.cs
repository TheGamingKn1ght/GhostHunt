using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

[RequireComponent(typeof(NetworkInputManager))]
public abstract class NetworkAbstractBaseMovement : NetworkBehaviour
{
    [SerializeField] protected float baseMoveSpeed;

    protected float moveSpeed;
    protected Rigidbody rb;
    protected CapsuleCollider playerCollider;

    [Header("Ground CheckSphere Parameters")]
    [SerializeField] protected float sphereCheckOffset;
    [SerializeField] protected float sphereCheckRadius;
    [SerializeField] protected LayerMask groundMask;

    protected virtual void Start()
    {
        if (!IsOwner) this.enabled = false;

        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        moveSpeed = baseMoveSpeed;
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        Vector3 movement = new Vector3(NetworkInputManager.movementInputs.x, 0f, NetworkInputManager.movementInputs.y).normalized;
        movement = rb.transform.TransformDirection(movement);

        rb.AddForce(movement * moveSpeed, ForceMode.Force);
    }

    #region Check Functions

    protected bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position + Vector3.down *sphereCheckOffset, sphereCheckRadius, groundMask, QueryTriggerInteraction.UseGlobal);
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (IsGrounded())
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawSphere(transform.position + Vector3.down * sphereCheckOffset, sphereCheckRadius);
    }
}
