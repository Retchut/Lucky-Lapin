using SimpleJSON;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

using NativeWebSocket;
using System.Collections.Generic;
using System.Numerics;

public class PlayerInputHandlerPlatformer : BaseInputHandler
{

    private const string SHOOT_BTN = "play";
    private const string UP_BTN = "bet_2";
    private const string DOWN_BTN = "play_18";
    private const string LEFT_BTN = "play_8";
    private const string RIGHT_BTN = "play_38";
    private const string UP_BTN_ALT = "bet_6";
    private const string DOWN_BTN_ALT = "bet_10";
    private const string LEFT_BTN_ALT = "play_68";
    private const string RIGHT_BTN_ALT = "play_88";

    // axis - isActive
    private Dictionary<string, string> machineButtonsState = new Dictionary<string, string>(){
    {"up", "inactive"},
    {"down", "inactive"},
    {"left", "inactive"},
    {"right", "inactive"},
    {"shoot", "inactive"}
};

    // public BufferedButton input_bufferedJump = new BufferedButton { bufferTime = 2 };
    public Button<Vector2> input_move = new Button<Vector2>();
    public Button<Vector2> input_look = new Button<Vector2>();
    public Button<float> input_jump = new Button<float>();
    public Button<float> input_sprint = new Button<float>();
    public Button<float> input_pause = new Button<float>();
    public Button<float> input_interact = new Button<float>();

    private WebSocket socket;

    protected override async void Awake()
    {
        base.Awake();

        if (!Application.isEditor)
        {
            url = "localhost";
            port = "3333";
        }

        // connect to machine socket
        try
        {
            socket = new WebSocket("ws://" + url + ":" + port);

            socket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };

            socket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };

            socket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };

            socket.OnMessage += (bytes) =>
            {
                // getting the message as a string
                string byteString = System.Text.Encoding.UTF8.GetString(bytes);
                var packet = JSON.Parse(byteString);
                int ascii_code = packet["payload"]["data"]["ascii_code"];
                string name = packet["payload"]["data"]["name"]; // btn name
                int input_address = packet["payload"]["data"]["input_address"];
                string input_state = packet["payload"]["data"]["input_state"]; // button press - active | inactive
                string output_state = packet["payload"]["data"]["output_state"]; // button light - active | inactive
                int output_address = packet["payload"]["data"]["output_address"];
                string type = packet["payload"]["data"]["type"];
                OnMachineButton(name, input_state);
            };

            // waiting for messages
            await socket.Connect();
        }
        catch (UriFormatException e)
        {
            Debug.LogWarning("No socket url and port were defined - socket wasn't set up");
        }
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (socket != null)
        {
            socket.DispatchMessageQueue();
        }
#endif
    }

    private async void OnDestroy()
    {
        if (socket != null)
        {
            await socket.Close();
        }
    }


    private void OnMove(InputValue inputValue) => SetInputInfo(input_move, inputValue);
    private void OnLook(InputValue inputValue) => SetInputInfo(input_look, inputValue);
    private void OnJump(InputValue inputValue) => SetInputInfo(input_jump, inputValue);
    private void OnSprint(InputValue inputValue) => SetInputInfo(input_sprint, inputValue);
    private void OnPause(InputValue inputValue) => SetInputInfo(input_pause, inputValue);
    private void OnInteract(InputValue inputValue) => SetInputInfo(input_interact, inputValue);
    private void OnMachineButton(string buttonName, string buttonState)
    {
        if (buttonState != "active" && buttonState != "inactive")
        {
            Debug.LogWarning("Invalid button state: " + buttonName + " - " + buttonState);
            return;
        }

        // shoot button
        if (buttonName == SHOOT_BTN && canToggleButton("shoot", buttonState))
        {
            setMachineButtonState("shoot", buttonState);
            SetMachineInputFloat(input_interact, (buttonState == "active") ? 1.0f : 0.0f);
            return;
        }

        // movement controls and failsafe
        Vector2 newInput = new Vector2(0, 0);
        switch (buttonName)
        {
            case UP_BTN_ALT:
            case UP_BTN:
                // up
                if (canToggleButton("up", buttonState))
                {
                    setMachineButtonState("up", buttonState);
                    newInput.y += (buttonState == "active") ? 1.0f : -1.0f;
                }
                break;
            case DOWN_BTN_ALT:
            case DOWN_BTN:
                //down
                if (canToggleButton("down", buttonState))
                {
                    setMachineButtonState("down", buttonState);
                    newInput.y += (buttonState == "active") ? -1.0f : 1.0f;
                }
                break;
            case LEFT_BTN_ALT:
            case LEFT_BTN:
                // left
                if (canToggleButton("left", buttonState))
                {
                    setMachineButtonState("left", buttonState);
                    newInput.x += (buttonState == "active") ? -1.0f : 1.0f;
                }
                break;
            case RIGHT_BTN_ALT:
            case RIGHT_BTN:
                // right
                if (canToggleButton("right", buttonState))
                {
                    setMachineButtonState("right", buttonState);
                    newInput.x += (buttonState == "active") ? 1.0f : -1.0f;
                }
                break;
            default:
                Debug.LogWarning("Unhandled button: " + buttonName + ": " + buttonState);
                return;
        }
        if (newInput != UnityEngine.Vector2.zero)
            IncrementMachineInputVector2(input_move, newInput);
    }

    // only called when buttonState is either "active" or "inactive" - no need to verify for any other case
    private bool canToggleButton(string buttonAction, string buttonState) => machineButtonsState[buttonAction] != buttonState;

    private void setMachineButtonState(string buttonAction, string newState) => machineButtonsState[buttonAction] = newState;
}
