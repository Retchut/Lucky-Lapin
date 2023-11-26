using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemnyLogicManager : LogicMachineManager
{

    public PlayerMovControllerFloat playerMovController;
    public TriggerChecker triggerChecker;
    public Collider mainCollider;
    public Rigidbody body;

    public override void OnAwake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BulletHit()
    {
        logicAnimator.SetTrigger("Death");

    }

}
