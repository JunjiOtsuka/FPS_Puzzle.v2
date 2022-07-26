using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public float xRotation = 0f;
    public float mouseX, mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        
        transform.localRotation = Quaternion.Euler(Mathf.Clamp(xRotation, -90, 90), 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
