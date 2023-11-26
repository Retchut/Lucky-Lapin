using DG.Tweening;
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
    public Ease rotationEase = Ease.InOutBack;

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


        if (isMelee)
        {

            manager.animator.SetTrigger(attackTrigger);

            manager.mainCollider.enabled = false;
            manager.body.isKinematic = true;

        }
        else
        {
            transform.DOLookAt(manager.playerMovController.transform.position, attackDelay, AxisConstraint.Y).SetEase(rotationEase);
        }

        await Task.Delay(System.TimeSpan.FromSeconds(attackDelay));

        if (isMelee)
        {

            //Debug.Log("AttacK");
            CameraShaker.instance.Shake(0.25f, true);

            if (manager.triggerChecker.hasObject)
            {
                manager.playerMovController.Knockback(direction);
                HeartsUiManager.instance.TakeHit();
            }

        }
        else
        {
            manager.chipThrower.Throw();
        }

        await Task.Delay(System.TimeSpan.FromSeconds(attackDuration));
        if (isMelee)
        {
            manager.mainCollider.enabled = true;
            manager.body.isKinematic = false;
        }
        logicAnimator.SetBool("Attack", false);

    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }



}
