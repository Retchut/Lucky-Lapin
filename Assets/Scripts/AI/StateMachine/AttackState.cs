using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AttackState : LogicMachineBehaviour<EnemnyLogicManager>
{
    public float attackDuration;


    public override void OnAwake()
    {

    }

    public override async void OnEnter()
    {
        var playerPos = manager.playerMovController.transform.position;

        var currentPos = manager.transform.position;
        var direction = playerPos - currentPos;
        direction.y = 0;
        direction.Normalize();

        if (!manager.triggerChecker.hasObject) return;


        manager.playerMovController.Knockback(direction);

        await Task.Delay(System.TimeSpan.FromSeconds(attackDuration));

        logicAnimator.SetBool("Attack", false);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }



}
