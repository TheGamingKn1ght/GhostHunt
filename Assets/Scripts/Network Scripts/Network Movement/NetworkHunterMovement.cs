using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using System.Collections;
using System.Runtime.CompilerServices;
using System;

public class NetworkHunterMovement : NetworkAbstractBaseMovement
{
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float jogSpeed;
    NetworkHunterStamina staminaObj;

    [Header("Vaulting Values")]
    [Tooltip("Height Of Vault Detection Cast. 0 is the bottom of the player capsule collider.")]
    [SerializeField, Min(0)] private float vaultCheckHeight;

    [Tooltip("Maximum Vault Detection Range. Minimum 0")]
    [SerializeField,Min(0)] private float vaultCheckCastDistance;

    [Tooltip("Maximum thickness of an obstacle that a player is permited to vault over. Minimum 0. ")]
    [SerializeField, Min(0)] private float maxVaultObstacleThickness;

    [Tooltip("Maximum angle that a player can look onto the face of an obstacle and be allowed to execute a vault. Minimum 0")]
    [SerializeField, Min(0)] private float maxVaultLookAngle;

    [Tooltip("Time in seconds it takes to perform a vault. Minimum 0")]
    [SerializeField,Range(0,10)] private float vaultTime;

    [Tooltip("Collider tag to look for when checking for vaultable obstacles colliders")]
    [SerializeField] private string vaultTag;

    //Temporary Debug Values
        private bool vaultCheck;
        private RaycastHit vaultHit;
        private Vector3 checkPosition;
        bool playerHasSpaceToVault;
    //
    
    private bool readyToVault;
    private bool vaulting;

    public static event Action onBeginVault;
    public static event Action onEndVault;


    private float colliderHeight;
    private float colliderCrouchHeight; 
    private Vector3 playerHeight;
    private Vector3 crouchHeight;

    protected override void Start()
    {
        base.Start();
        staminaObj = GetComponent<NetworkHunterStamina>();

        //Crouch Variables
        playerHeight = transform.localScale;
        crouchHeight = new Vector3(playerHeight.x, playerHeight.y * 0.5f, playerHeight.z);
        colliderHeight = this.playerCollider.height;
        colliderCrouchHeight = colliderHeight * 0.5f;
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void Update()
    {
        VaultCheck();
        Sprint();
        Crouch();

        
    }

    private void Crouch()
    {
        if (NetworkInputManager.crouchInput)
        {
            rb.transform.localScale = crouchHeight;
            playerCollider.height = colliderCrouchHeight;
            moveSpeed = crouchSpeed;
        }
        else
        {
            rb.transform.localScale = playerHeight;
            playerCollider.height = colliderHeight;
            moveSpeed = baseMoveSpeed;
        }

    }

    private void Sprint()
    {
        if (CanSprint() && NetworkInputManager.sprintInput)
        {
            moveSpeed = sprintSpeed;
            staminaObj.UseStamina();
        }
        else if(!CanSprint() && NetworkInputManager.sprintInput)
        {
            moveSpeed = jogSpeed;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            staminaObj.RegenerateStamina();
        }
    }

    private void Vault()
    {
        if(readyToVault && !vaulting)
        {
            vaulting = true;
            canMove = false;
            rb.linearVelocity = new Vector3(0, 0, 0);
            rb.useGravity = false;
            onBeginVault.Invoke();

            StartCoroutine(VaultCoroutine());
        }
        
    }

    #region Check Functions
        private bool CanSprint()
        {
            return staminaObj.HasStamina;// && NetworkInputManager.sprintInput;//boollogic; (future)
        }

        /// <summary>
        /// Checks If The Player Currently Meets All Prerequisits To Vault.
        /// This Includes Being In Range Of An Obstacle With A "Vaultable Obstacle" Tag, Facing The Obstacle At The Correct Angle And Having An Unimpeeded
        /// Exit Area For The Vault.
        /// </summary>
        private void VaultCheck()
        {
            if (IsGrounded())
            {
                vaultCheck = Physics.Raycast(new Vector3(transform.position.x, (transform.position.y - colliderHeight/2) + vaultCheckHeight, transform.position.z), 
                    transform.forward, out vaultHit, vaultCheckCastDistance, groundMask);

                if (vaultCheck && vaultHit.collider.CompareTag(vaultTag.ToString()))
                {
                    Collider obstacleCollider = vaultHit.collider;

                    float lookAngle = Mathf.Abs(Vector3.Angle(-vaultHit.normal, transform.forward));
                    
                    Vector3 obstacleNormal = vaultHit.normal;
                    Vector3 obstacleThroughDir = -obstacleNormal;

                    Vector3 reverseCastStart = vaultHit.point + (obstacleThroughDir * maxVaultObstacleThickness);

                    if (lookAngle < maxVaultLookAngle && Physics.Raycast(reverseCastStart, obstacleNormal, out RaycastHit exitHit, 5f))
                    {
                        Debug.Log(lookAngle);
                        if (obstacleCollider == exitHit.collider)
                        {
                            float obstacleThickness = Vector3.Distance(vaultHit.point, exitHit.point);

                            float totalDistance = obstacleThickness + playerCollider.radius + 0.05f;
                            checkPosition = vaultHit.point + (totalDistance * obstacleThroughDir);

                            playerHasSpaceToVault = !(Physics.CheckSphere(checkPosition, playerCollider.radius));

                            if (playerHasSpaceToVault)
                            {
                                readyToVault = true;
                                return;
                            }
                        }
                    }
                   
                }
            }

            readyToVault = false;
        }
    #endregion

    private IEnumerator VaultCoroutine()
    {
        float waitTime = vaultTime / 3;
        float elapsedTime = 0;
        Vector3 vaultUpHeightPosition = new Vector3(transform.position.x,transform.position.y + playerCollider.height/2, transform.position.z);
        Vector3 finalPosition = checkPosition;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(transform.position, vaultUpHeightPosition, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;

        while(elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(finalPosition.x,transform.position.y, finalPosition.z),1);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        elapsedTime = 0;

        while(elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(transform.position,finalPosition, 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        
        rb.useGravity = true;
        vaulting = false;
        onEndVault.Invoke();
        canMove = true;

        yield return null;
    }

    private void OnDrawGizmos()
    {
        Vector3 vaultCheckDirection = transform.forward * vaultCheckCastDistance;

        Gizmos.color = vaultCheck ? Color.green : Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x, (transform.position.y - colliderHeight/2) + vaultCheckHeight, transform.position.z), vaultCheckDirection);

        
        Gizmos.color = playerHasSpaceToVault && readyToVault ? Color.green : Color.red;
        Gizmos.DrawSphere(checkPosition, playerCollider.radius);

    }


    #region Event Subscriptions
    private void OnEnable()
    {
        NetworkInputManager.onSprintToggle += Sprint;
        NetworkInputManager.onCrouchToggle += Crouch;
        NetworkInputManager.onVaultInput += Vault;
    }

    private void OnDisable()
    {
        NetworkInputManager.onSprintToggle -= Sprint;
        NetworkInputManager.onCrouchToggle -= Crouch;
        NetworkInputManager.onVaultInput -= Vault;
    }
    #endregion
}
