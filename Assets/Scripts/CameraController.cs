using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player;
    [SerializeField]
    float mouseSensitivity = 1000f;
    [SerializeField]
    Light light;
    float xRotation = 0f;
    Color daySky = new Color();
    Color nightSky = new Color();
    Color dayLight = new Color();
    Color nightLight = new Color();

    void Start()
    {
        // hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        ColorUtility.TryParseHtmlString("#C9FEFF", out daySky);
        ColorUtility.TryParseHtmlString("#212135", out nightSky);
        Camera.main.backgroundColor = daySky;
        ColorUtility.TryParseHtmlString("#FDF497", out dayLight);
        ColorUtility.TryParseHtmlString("#000000", out nightLight);
        light.color = dayLight;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) && !GameManager.isNight) || GameManager.nightTrigger)
        {
            Camera.main.backgroundColor = nightSky;
            light.color = nightLight;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && !GameManager.isNight)
        {
            Camera.main.backgroundColor = daySky;
            light.color = dayLight;
        }

        float coefficient = mouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        xRotation -= mouseY * coefficient;
        xRotation = Mathf.Clamp(xRotation, -90f, 60f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        player.Rotate(Vector3.up * mouseX * coefficient);

    }
}
