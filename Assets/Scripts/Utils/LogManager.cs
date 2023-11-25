using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public enum LogType { Info, Warning, Error };
    public static void Log(string msg, LogType type = LogType.Info)
    {
#if UNITY_EDITOR
        switch (type)
        {
            case LogType.Info:
                Debug.Log(msg);
                break;
            case LogType.Warning:
                Debug.LogWarning(msg);
                break;
            default:
                Debug.LogError(msg);
                break;
        }
#endif
    }
}
