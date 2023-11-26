using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevezone : MonoBehaviour
{
    public TriggerChecker triggerChecker;
    bool flag;
    UEventHandler eventHandler = new UEventHandler();
    void Start()
    {
        triggerChecker.OnTriggered.Subscribe(eventHandler, Entered);
    }

    void Entered(Transform t)
    {
        if (flag) return;

        flag = true;
        LevelManager.instance.NextLevel();

    }
}
