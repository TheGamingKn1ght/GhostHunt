using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkInputManager))]
public class NetworkPlayerMove : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody rb;

    #region Input Activations
    private void OnEnable()
    {
        NetworkInputManager.OnSprintInput += Sprint;
    }
    private void OnDisable()
    {
        NetworkInputManager.OnSprintInput -= Sprint;
    }
    #endregion

    private void Start()
    {
        if (!IsOwner) this.enabled=false;

        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 movement = new Vector3(NetworkInputManager.movementInputs.x, 0f, NetworkInputManager.movementInputs.y).normalized;
        movement = rb.transform.TransformDirection(movement);

        rb.AddForce(movement * moveSpeed, ForceMode.Force);
    }

    private void Sprint()
    {
        //Currently activates sprint speed, but never gets deactivated when uncommented
        //moveSpeed *= 2;
    }

   
}
