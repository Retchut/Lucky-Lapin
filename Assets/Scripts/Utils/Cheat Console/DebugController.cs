using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LogManager;
namespace RadiantGames
{
    public class DebugController : MonoBehaviour
    {
        // Variables
        bool ShowConsole;
        bool ShowHelp;

        public string UserInput = "";

        public List<object> AllCommands;
        //------Instantiate Commands------//
        public static DebugCommand Help;

        //You have to add <type> for commands that use parameters.

        Vector2 scroll;

        private void Awake()
        {
            //------Define Commands------//

            /*FORMAT : First Argument is the name that would be used to call the command and second 
            one is the Description of the Command Third would be the Function that 
            would happen when the command is called*/

            Help = new DebugCommand("h", "Shows all available commands", () =>
            {
                ShowHelp = !ShowHelp;
            });


        
            /*You need to add <type> for commands that take a argument also the () store 
            the Variable that user enter as parameter */

            //------Add all commands to the list------//
            AllCommands = new List<object>
            {
                Help,
            };
        }
        //---Functions for commands(Note that functions outside the class can also be used.)---//


        //Update Method
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightAlt))
            {
                ShowConsole = true;
                //Cursor.lockState = CursorLockMode.None;
                UserInput = "";
                //Cursor.visible = true;
            }
            if (Input.GetKeyDown(KeyCode.Return) && ShowConsole)
            {
                //If user presses enter and show console is true
                //print("Handling Input");
                HandleInput();

                if (!UserInput.ToUpper().Contains(Help.commandId.ToUpper()))
                    ShowConsole = false;

                UserInput = "";
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
            }

            if (Input.inputString.Length == 1 && Input.inputString != "\b")
            {
                UserInput += Input.inputString;
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (UserInput.Length > 0)
                    UserInput = UserInput.Remove(UserInput.Length - 1);
            }
        }
        //So what this method basically does is convert debug command base o debug command
        //See Debug Command script for more info.
        private void HandleInput()
        {
            string[] properties = UserInput.Split(' ');
            for (int i = 0; i < AllCommands.Count; i++)
            {
                DebugCommandBase commandBase = AllCommands[i] as DebugCommandBase;
                if (UserInput.ToUpper().Contains(commandBase.commandId.ToUpper()))
                {
                    if (AllCommands[i] as DebugCommand != null)
                    {
                        (AllCommands[i] as DebugCommand).Invoke();
                    }
                    else if (AllCommands[i] as DebugCommand<string> != null)
                    {
                        (AllCommands[i] as DebugCommand<string>).Invoke(properties[1]);
                    }
                    else if (AllCommands[i] as DebugCommand<int> != null)
                    {
                        (AllCommands[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                    }
                    else if (AllCommands[i] as DebugCommand<float> != null)
                    {
                        (AllCommands[i] as DebugCommand<float>).Invoke(float.Parse(properties[1]));
                    }
                }
            }
        }

        private void OnGUI()
        {


            //If show console is false then we simple return
            if (!ShowConsole) return;


            GUIStyle TextSytle = new GUIStyle(GUI.skin.textField);
            TextSytle.fontSize = 15;

            GUIStyle HelpTextStyle = new GUIStyle(GUI.skin.label);
            HelpTextStyle.fontSize = 15;

            float y = 10;

            if (ShowHelp)
            {
                float BoxSize = 200;
                GUI.Box(new Rect(0, y, Screen.width, BoxSize), "");

                Rect Viewport = new Rect(0, 0, Screen.width - 50, 40 * AllCommands.Count);
                scroll = GUI.BeginScrollView(new Rect(0, y, Screen.width, BoxSize), scroll, Viewport);
                for (int i = 0; i < AllCommands.Count; i++)
                {
                    DebugCommandBase command = AllCommands[i] as DebugCommandBase;
                    string label = $" {command.commandId} : {command.commandDescription} ";
                    Rect labelRect = new Rect(5, 40 * i, Viewport.width, 40);
                    GUI.Label(labelRect, label, HelpTextStyle);
                }
                GUI.EndScrollView();
                y += BoxSize + 20;
            }

            GUI.Box(new Rect(0, y, Screen.width, 40), "");
            GUI.backgroundColor = new Color(0, 0, 0, 0);
            GUI.SetNextControlName("CheatBox");
            //UserInput = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 65f), UserInput, TextSytle);
            GUI.Label(new Rect(10f, y + 5f, Screen.width - 20f, 65f), UserInput, TextSytle);


        }
    }
}
