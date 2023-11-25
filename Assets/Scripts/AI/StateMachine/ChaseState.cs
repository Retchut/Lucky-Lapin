using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : LogicMachineBehaviour<EnemnyLogicManager>
{
    public float stopDistance=1;
    public float chaseMaxDistance = 20;
    public float chaseSpeed;

    public float turnSpeed;
    public float maxY = 3;
    public float currentY;

    public override void OnAwake()
    {
    }

    public override void OnEnter()
    {


    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        var playerPos = manager.playerMovController.transform.position;

        var currentPos = manager.transform.position;

        currentY = currentPos.y;
        if (currentY > maxY) return;

        var distance = Vector3.Distance(playerPos, currentPos);
        if (distance <= stopDistance)
        {
            logicAnimator.SetBool("Attack",true);
            return;

        }


        var direction = playerPos - currentPos;
        direction.y = 0;
        direction.Normalize();

        transform.forward = Vector3.Slerp(transform.transform.forward, direction, Time.deltaTime * turnSpeed);

        transform.position += transform.forward * chaseSpeed * Time.deltaTime;
    }

}
