using UnityEngine;




public abstract class LogicMachineBehaviour<T> : LogicMachineBehaviour where T : LogicMachineManager
{
    public T manager { get; private set; }
    public GameObject gameObject { get; private set; }
    public Transform transform { get; private set; }
    public Animator logicAnimator { get; private set; }

    protected bool active;

    private bool initialized = false;
    private bool entered = false;


    public override void Awake_Internal(Animator animator)
    {
        logicAnimator = animator;
        manager = logicAnimator.gameObject.GetComponent<T>();
        gameObject = manager.gameObject;
        transform = gameObject.transform;

        OnAwake();
        initialized = true;
    }


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        active=true;
        //Init_Internal(animator);
        Enter_Internal();

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(0) && initialized && entered)
            OnUpdate();

        base.OnStateUpdate(animator, stateInfo, layerIndex);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        active = false;
        OnExit();
        manager.ResetCurrentBehaviour();
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    private async void Enter_Internal()
    {
        await manager.WaitToExit();

        if (!active) return;

        manager.SetCurrentBehaviour(this);

        OnEnter();
        entered = true;

    }

    public abstract void OnAwake();
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}


public abstract class LogicMachineBehaviour : StateMachineBehaviour
{
    public abstract void Awake_Internal(Animator animator);

}

