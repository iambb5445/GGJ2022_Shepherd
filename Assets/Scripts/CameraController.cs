using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    float mouseSensitivity = 1000f;
    float xRotation = 0f;

    void Start()
    {
        // hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float coefficient = mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        xRotation -= mouseY * coefficient;
        xRotation = Mathf.Clamp(xRotation, -90f, 60f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX * coefficient);

    }
}
