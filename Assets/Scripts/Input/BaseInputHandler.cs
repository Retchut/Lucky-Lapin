using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UEventHandler;

[RequireComponent(typeof(PlayerInput))]

public class BaseInputHandler : MonoBehaviour
{
    public Camera playerCamera { get; private set; }
    public PlayerInput input { get; private set; }

    [SerializeField] private string[] devices;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();

        if (input.camera == null)
            playerCamera = Camera.main;
        else
            playerCamera = input.camera;
        devices = input.devices.Select(x => x.name).ToArray();
    }
    public InputDevice GetMainInput() => input.devices.Count <= 0 ? null : input.devices[0];
    public void ChangeInputDevice(int inputId)
    {
        var device = InputSystem.GetDeviceById(inputId);

        if (device == null) return;

        bool isKeyboard = device.name.Contains("Keyboard");

        input.SwitchCurrentControlScheme(isKeyboard ? "Keyboard&Mouse" : "Gamepad", device);
        devices = input.devices.Select(x => x.name).ToArray();
    }

    #region Button Base stuff
    //public delegate void ClickAction();

    public class Button<TValue>  //Suported data types--> float | Vector2 | Vector3 | Vector4
    {
        public TValue value { get; set; }
        //public event ClickAction Onpressed;
        //public event ClickAction Onreleased;
        public UEvent Onpressed = new UEvent();
        public UEvent Onreleased = new UEvent();

        public void Pressed() => Onpressed.TryInvoke();
        //public void Pressed() => Onpressed?.Invoke();
        public void Released() => Onreleased.TryInvoke();
        //public void Released() => Onreleased?.Invoke();
    }

    public class BufferedButton : MonoBehaviour  //Suported data types--> bool
    {
        public bool isPressed { get; set; }
        public float bufferTime { get; set; }

        Coroutine bufferRoutine;

        public void ClearButtonBuffer()
        {
            if (bufferRoutine != null) StopCoroutine(bufferRoutine);
            bufferRoutine = null;
        }
        public void SetButtonPress()
        {
            if (bufferRoutine != null) StopCoroutine(bufferRoutine);
            bufferRoutine = StartCoroutine(WaitToClearInput());
        }

        IEnumerator WaitToClearInput()
        {
            yield return new WaitForSeconds(bufferTime);
            isPressed = false;
        }
    }
    #endregion

    #region Info Setters
    protected void SetInputInfo(Button<float> button, InputValue inputValue)
    {
        var value = inputValue.Get<float>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value == 0)
            button.Released();
        else if (oldValue == 0)
            button.Pressed();
    }

    protected void SetInputInfo(Button<Vector2> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector2>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    protected void SetInputInfo(Button<Vector3> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector3>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    protected void SetInputInfo(Button<Vector4> button, InputValue inputValue)
    {
        var value = inputValue.Get<Vector4>();

        var oldValue = button.value; // This is done to prevent OnPressed incorrect value reads  (if value was set before the invoke)
        button.value = value;

        if (value.magnitude == 0)
            button.Released();
        else if (oldValue.magnitude == 0)
            button.Pressed();
    }

    #endregion

}
