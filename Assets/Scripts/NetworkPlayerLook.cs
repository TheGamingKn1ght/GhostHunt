using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(NetworkInputManager))]
public class NetworkPlayerLook : NetworkBehaviour
{
    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;

    [SerializeField] private Camera viewCam;

    private float xClamp = 85f;
    float xRotation;

    private void Start()
    {
        if (!IsOwner)
        {
            viewCam.gameObject.SetActive(false);
            viewCam.enabled = false;
            this.enabled = false;
        }
    }

    private void Update()
    {
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
}
