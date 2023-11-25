using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerFPS : MonoBehaviour
{

    [Range(0.1f, 5f)]  public float sensitivityX=1;
    [Range(0.1f, 5f)] public float sensitivityY=1;
    public PlayerInputHandlerPlatformer inputManager;

    private float sensMultiplier = 15f;

    private float desiredX;
    private float xRotation;

    private void Start()
    {
        CursorManager.instance.HideCursor();
    }

    private void Update()
    {
        Look();
    }
    private void Look()
    {
        float mouseX = inputManager.input_look.value.x * sensitivityX * Time.smoothDeltaTime * sensMultiplier;
        float mouseY = inputManager.input_look.value.y * sensitivityY * Time.smoothDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        //orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }
}
