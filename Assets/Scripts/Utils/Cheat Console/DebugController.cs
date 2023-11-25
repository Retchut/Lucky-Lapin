using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static LogManager;

public class DebugController : MonoBehaviour
{

    public static DebugController current;
    // Variables
    bool showConsole;
    bool ShowHelp;
    bool showLog;

    public KeyCode openConsoleKey = KeyCode.Backslash;
    public KeyCode closeConsoleKey = KeyCode.Escape;

    public bool darkTheme = true;

    public string UserInput = "";
    public float inputBoxSize = 40;
    public float helpBoxSize = 200;

    public List<DebugCommandBase> AllCommands;
    public List<DebugCommandBase> HelpCommands;
    //------Instantiate Commands------//
    public static DebugCommand Help;
    public static DebugCommand Log;
    public static DebugCommand Tweens;
    public static DebugCommand Quit;
    public static DebugCommand<int> GoToScene;


    private List<string> logEntries = new List<string>();
    private int maxLogEntries = 15; // Set your desired maximum number of log entries
    private string log;

    //You have to add <type> for commands that use parameters.

    Vector2 scroll;

    int tabCommandIndex;

    private async void Awake()
    {

        if (current != null)
        {
            Destroy(gameObject);
            return;

        }

        current = this;
        DontDestroyOnLoad(gameObject);


        //await Task.Delay(1400);

        //------Define Commands------//
        BindCommandAttributes();
        SceneManager.sceneLoaded += async (x, y) =>
        {
            await Task.Delay(500);
            BindCommandAttributes();
        };
    }

    private void InitBaseCommands()
    {

        /*FORMAT : First Argument is the name that would be used to call the command and second 
        one is the Description of the Command Third would be the Function that 
        would happen when the command is called*/

        Application.logMessageReceived += Application_logMessageReceived;
        Help = new DebugCommand("help", "Shows all available commands", () =>
        {
            ShowHelp = true;
            HelpCommands = AllCommands;
        });
        Log = new DebugCommand("log", "Shows/Hide logs", () =>
       {
           showLog = !showLog;

           if (showLog)
               // Update the log string by joining the log entries
               log = string.Join("\n", logEntries);
           else
               log = null;
       });

        Tweens = new DebugCommand("tweens", "Shows no of active tweens", () =>
        {
            Debug.Log($"Total active tweens are: { DG.Tweening.DOTween.TotalActiveTweens()}");
            var tweens = DG.Tweening.DOTween.PlayingTweens();
            for (int i = 0; (i < 8 && i < tweens.Count); i++)
            {
                Debug.Log($"Tween {i} target: {((UnityEngine.Object)tweens[i].target).name}", (UnityEngine.Object)tweens[i].target);

            }
        });

        Quit = new DebugCommand("quit", "Quit the application", () =>
       {
           Application.Quit();
       });

        GoToScene = new DebugCommand<int>("goto", "Go to scene number", (lvl) =>
        {
            SceneManager.LoadScene(lvl);
        });

        /*You need to add <type> for commands that take a argument also the () store 
        the Variable that user enter as parameter */

        //------Add all commands to the list------//
        AllCommands = new List<DebugCommandBase>
            {
                Help,Log,Quit,GoToScene, Tweens
            };

        HelpCommands = new List<DebugCommandBase>();
    }

    private void Application_logMessageReceived(string condition, string stackTrace, UnityEngine.LogType type)
    {
        string newLogEntry = $"({type.ToString().ToUpper()}) {condition}";

        // Add the new log entry to the list
        logEntries.Insert(0, newLogEntry);

        // Trim the list if it exceeds the maximum number of log entries
        if (logEntries.Count > maxLogEntries)
        {
            logEntries.RemoveAt(logEntries.Count - 1);
        }

    }

    //---Functions for commands(Note that functions outside the class can also be used.)---//


    bool CheckValidAssembly(string assemblyName)
    {
        return !assemblyName.StartsWith("Unity") && !assemblyName.StartsWith("System") && !assemblyName.StartsWith("Mono")
             && !assemblyName.StartsWith("com.unity") && !assemblyName.StartsWith("mscorlib");
    }
    void BindCommandAttributes()
    {
        InitBaseCommands();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => CheckValidAssembly(x.FullName));
        var assemblyList = assemblies.ToList();
        foreach (var assembliy in assemblies)
        {

            var types = assembliy.GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods();

                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(DebugCommandAttribute), true);

                    if (attributes.Length <= 0) continue;

                    var defAtribute = (DebugCommandAttribute)attributes[0];
                    var id = defAtribute.Id;

                    if (method.IsStatic)
                    {
                        var command = CreateDebugCommand(id, $"(Static) {defAtribute.Description}", null, method);
                        AllCommands.Add(command);
                    }
                    else
                    {

                        var instances = UnityEngine.Object.FindObjectsOfType(type);
                        //Debug.Log("Find objects entry");

                        for (int i = 0; i < instances.Length; i++)
                        {
                            var instance = instances[i];
                            var instanceId = i > 0 ? id + i : id;
                            var parameteres = method.GetParameters();
                            DebugCommandBase command;

                            if (parameteres.Length > 1)
                            {
                                Debug.LogError("Debug command does not support more than one parameter");
                                return;
                            }
                            else if (parameteres.Length == 1)
                            {
                                command = CreateDebugCommand(id, instanceId, $"({instances[i].name}) {defAtribute.Description}", instance, method, parameteres);
                            }
                            else
                            {
                                command = CreateDebugCommand(instanceId, $"({instances[i].name}) {defAtribute.Description}", instance, method);
                            }

                            //if (!AllCommands.Any(x => x.baseId == id && x.instanceId == instances[i].GetInstanceID()))
                            AllCommands.Add(command);
                        }
                    }



                    //foreach (var attribute in attributes)
                    //{
                    //    bool flag = true;
                    //}
                }

                //TODO: update this to support instance methods (Using unity GetObjectsOfType to see every reference to the objects

            }
        }
    }

    DebugCommandBase CreateDebugCommand(string id, string description, object instance, System.Reflection.MethodBase method)
    {
        return new DebugCommand(id, description, () =>
         {
             method.Invoke(instance, null);
         });

    }

    DebugCommandBase CreateDebugCommand(string baseId, string id, string description, UnityEngine.Object instance, System.Reflection.MethodBase method, System.Reflection.ParameterInfo[] parameters)
    {
        DebugCommandBase command;
        switch (true)
        {
            case bool _ when parameters[0].ParameterType == typeof(int):
                command = new DebugCommand<int>(id, description + " [int]", (value) =>
                 {
                     method.Invoke(instance, new object[] { value });
                 });
                break;
            case bool _ when parameters[0].ParameterType == typeof(double):
                command = new DebugCommand<double>(id, description + " [double]", (value) =>
                {
                    method.Invoke(instance, new object[] { value });
                });
                break;
            case bool _ when parameters[0].ParameterType == typeof(float):
                command = new DebugCommand<float>(id, description + " [float]", (value) =>
                {
                    method.Invoke(instance, new object[] { value });
                });
                break;
            case bool _ when parameters[0].ParameterType == typeof(bool):
                command = new DebugCommand<bool>(id, description + " [bool]", (value) =>
                {
                    method.Invoke(instance, new object[] { value });
                });
                break;
            default:
                command = new DebugCommand<string>(id, description + " [string]", (value) =>
                {
                    method.Invoke(instance, new object[] { value });
                });
                break;

        }

        //command.instanceId = instance.GetInstanceID();
        return command;
    }


    //Update Method
    void Update()
    {
        if (Input.GetKeyDown(openConsoleKey))
        {
            showConsole = !showConsole;
            UserInput = "";
            ShowHelp = false;

            showLog = false;
        }
        if (Input.GetKeyDown(KeyCode.Return) && showConsole)
        {
            tabCommandIndex = 0;

            HandleInput();

            if (!UserInput.ToUpper().Contains(Help.commandId.ToUpper()))
                showConsole = false;

            UserInput = "";
            //Time.timeScale = 1;
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        if (Input.inputString.Length == 1 && Input.inputString != "\b" && Input.inputString != "\t" && !Input.GetKeyDown(openConsoleKey) && !Input.GetKeyDown(KeyCode.Return))
        {

            UserInput += Input.inputString;


            if (!string.IsNullOrEmpty(UserInput))
                HelpCommands = AllCommands.Where(x => x.commandId.StartsWith(UserInput.Replace(" ", ""))).ToList();

            ShowHelp = true;
        }

        if (Input.GetKeyDown(KeyCode.Tab) && HelpCommands.Count > 0)
        {
            if (tabCommandIndex >= HelpCommands.Count)
                tabCommandIndex = 0;

            UserInput = HelpCommands[tabCommandIndex].commandId;
            tabCommandIndex++;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            tabCommandIndex = 0;

            if (UserInput.Length > 0)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                    UserInput = "";
                else
                    UserInput = UserInput.Remove(UserInput.Length - 1);
            }


            if (UserInput.Length == 0)
            {
                ShowHelp = false;
            }

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
        RenderConsole();

        RenderLog();
    }

    private void RenderLog()
    {
        if (!showLog) return;

        GUI.color = darkTheme ? Color.black : Color.white;
        GUIStyle TextSytle = new GUIStyle(GUI.skin.textField);
        TextSytle.fontSize = 15;
        float margin = 5;
        Rect logRect = new Rect(margin, margin, Screen.width - margin, Screen.height - margin);

        GUI.Label(logRect, log, TextSytle);
    }

    private void RenderConsole()
    {
        //If show console is false then we simple return
        if (!showConsole) return;



        GUI.backgroundColor = darkTheme ? Color.black : Color.white;
        GUI.color = darkTheme ? Color.black : Color.white;

        GUIStyle TextSytle = new GUIStyle(GUI.skin.textField);
        TextSytle.fontSize = 15;

        GUIStyle HelpTextStyle = new GUIStyle(GUI.skin.label);

        HelpTextStyle.fontSize = 15;

        float marginTop = 10;
        float commandMargin = 20;


        if (ShowHelp)
        {
            var startY = marginTop + inputBoxSize;
            GUI.Box(new Rect(0, startY, Screen.width, helpBoxSize), "");
            Rect Viewport = new Rect(0, startY, Screen.width - 50, inputBoxSize * AllCommands.Count);
            scroll = GUI.BeginScrollView(new Rect(0, startY, Screen.width, helpBoxSize), scroll, Viewport);
            startY += marginTop;

            for (int i = 0; i < HelpCommands.Count; i++)
            {
                DebugCommandBase command = HelpCommands[i] as DebugCommandBase;
                string label = $" {command.commandId} : {command.commandDescription} ";
                Rect labelRect = new Rect(5, startY + commandMargin * i, Viewport.width, 40);
                GUI.Label(labelRect, label, HelpTextStyle);
            }
            GUI.EndScrollView();
        }

        GUI.Box(new Rect(0, 0, Screen.width, inputBoxSize), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("CheatBox");
        //UserInput = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 65f), UserInput, TextSytle);
        GUI.Label(new Rect(10f, marginTop + 5f, Screen.width - 20f, 65f), UserInput, TextSytle);
    }
}
