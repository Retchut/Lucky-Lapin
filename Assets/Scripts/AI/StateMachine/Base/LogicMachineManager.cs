using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public abstract class LogicMachineManager : MonoBehaviour
{
    private Animator logicAnimator;

    private const int MAX_WAIT_TICKS = 50;

    private LogicMachineBehaviour[] behaviours;
    public LogicMachineBehaviour currentBehaviour { get; private set; }


    private void Awake()
    {
        logicAnimator = GetComponent<Animator>();

        behaviours = logicAnimator.GetBehaviours<LogicMachineBehaviour>();

        foreach (var behaviour in behaviours)
            behaviour.Awake_Internal(logicAnimator);

       OnAwake();
    }

    public abstract void OnAwake();


    //public async void SetLogicTrigger(string name)
    //{
    //    logicAnimator.SetTrigger(name);

    //    await Task.Delay()
    //}






    public void SetCurrentBehaviour(LogicMachineBehaviour behaviour)
    {
        currentBehaviour = behaviour;
    }

    public void ResetCurrentBehaviour()
    {
        currentBehaviour = null;
    }

    public async Task WaitToExit()
    {
        for (int i = 0; i < MAX_WAIT_TICKS; i++)
        {
            if (currentBehaviour == null) break;
            await Task.Delay(15);
        }

        return;

    }

}

