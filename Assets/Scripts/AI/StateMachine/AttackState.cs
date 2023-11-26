using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AttackState : LogicMachineBehaviour<EnemnyLogicManager>
{
    public float attackDuration;
    public float attackDelay = 1.5f;
    public bool isMelee = true;
    public string attackTrigger;

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

        manager.animator.SetTrigger(attackTrigger);

        manager.mainCollider.enabled = false;
        manager.body.isKinematic = true;

        await Task.Delay(System.TimeSpan.FromSeconds(attackDelay));

        //Debug.Log("AttacK");
        CameraShaker.instance.Shake(0.25f, true);

        if (manager.triggerChecker.hasObject)
        {
            manager.playerMovController.Knockback(direction);
            HeartsUiManager.instance.TakeHit();
        }

        await Task.Delay(System.TimeSpan.FromSeconds(attackDuration));
        logicAnimator.SetBool("Attack", false);
        manager.mainCollider.enabled = true;
        manager.body.isKinematic = false;


    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }



}
