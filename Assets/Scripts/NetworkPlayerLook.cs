using UnityEngine;
using Unity.Netcode;
using System;

[RequireComponent(typeof(NetworkInputManager))]
public class NetworkPlayerLook : NetworkBehaviour
{
    [Tooltip("Horizontal Sensitivity Multiplier")]
    [SerializeField] private float xSensitivity;

    [Tooltip("Vertical Sensitivity Multiplier")]
    [SerializeField] private float ySensitivity;

    [Tooltip("Camera to be controlled by this script. Recommended to select the player's main camera")]
    [SerializeField] private Camera viewCam;

    private float xClamp = 85f;
    private bool canLook;
    float xRotation;


    private void Start()
    {
        if (!IsOwner)
        {
            viewCam.gameObject.SetActive(false);
            viewCam.enabled = false;
            this.enabled = false;
        }

        EnableLook();
    }

    private void Update()
    {
        Debug.Log(canLook);
        if (!canLook) return;
        Look();
    }

    private void Look()
    {
        Cursor.lockState = CursorLockMode.Locked;

        float mouseX = NetworkInputManager.lookInputs.x * xSensitivity * Time.deltaTime;
        float mouseY = NetworkInputManager.lookInputs.y * ySensitivity * Time.deltaTime;

        this.transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp, xClamp);

        viewCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void EnableLook() { canLook = true; }
    private void DisableLook() { canLook = false; }

    #region Event Subscriptions
        private void OnEnable()
        {
            NetworkHunterMovement.onEndVault += EnableLook;
            NetworkHunterMovement.onBeginVault += DisableLook;
        }

        private void OnDisable()
        {
            NetworkHunterMovement.onEndVault -= EnableLook;
            NetworkHunterMovement.onBeginVault -= DisableLook;
        }

    #endregion

}
