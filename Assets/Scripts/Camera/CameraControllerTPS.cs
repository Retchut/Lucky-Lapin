using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerTPS : MonoBehaviour
{
    public PlayerInputHandlerPlatformer inputHandler;

    public Transform targetRotator;

    [Range(0.1f, 5f)] public float sensitivityX = 1;
    [Range(0.1f, 5f)] public float sensitivityY = 1;

    private float sensMultiplier = 15f;

    private void Start()
    {
        CursorManager.instance.HideCursor();
    }

    void Update()
    {
        Look();
    }

    private void Look()
    {
        var newRotation = Quaternion.Euler(targetRotator.eulerAngles.x + (inputHandler.input_look.value.y * sensMultiplier * sensitivityX * -Time.smoothDeltaTime),
                                       targetRotator.eulerAngles.y + (inputHandler.input_look.value.x * sensMultiplier * sensitivityY * Time.smoothDeltaTime),
                                       targetRotator.eulerAngles.z);

        // Debug.Log("Rotation is X:" + newRotation.eulerAngles.x +
        //                                             " Y:" + newRotation.eulerAngles.y +
        //                                               " Z:" + newRotation.eulerAngles.z);

        targetRotator.rotation = newRotation;
    }
}
