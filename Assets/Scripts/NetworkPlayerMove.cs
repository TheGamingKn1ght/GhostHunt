using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkInputManager))]
public class NetworkPlayerMove : NetworkBehaviour
{
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    private Rigidbody rb;


    private void Start()
    {
        if (!IsOwner) this.enabled=false;

        moveSpeed = walkSpeed;
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
        moveSpeed = NetworkInputManager.sprintInput ? sprintSpeed : walkSpeed;
        Debug.Log("Sprinting : " + NetworkInputManager.sprintInput);
    }

    #region Input Activations
    private void OnEnable()
    {
        NetworkInputManager.onSprintToggle += Sprint;
    }
    private void OnDisable()
    {
        NetworkInputManager.onSprintToggle -= Sprint;
    }
    #endregion


}
