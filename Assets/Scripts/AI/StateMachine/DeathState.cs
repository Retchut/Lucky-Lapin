using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DeathState : LogicMachineBehaviour<EnemnyLogicManager>
{
    public float knockForceHorizontal;
    public float knockForceV;

    public float delayToDisable = 1.5f;
    public override void OnAwake()
    {
    }

    public override async void OnEnter()
    {
        manager.mainCollider.enabled = false;
        manager.body.isKinematic = false;

        manager.body.AddForce(-transform.forward * knockForceHorizontal, ForceMode.Impulse);
        manager.body.AddForce(Vector3.up * knockForceV, ForceMode.Impulse);


        await Task.Delay(System.TimeSpan.FromSeconds(delayToDisable));

        manager.deathVfx.SendEvent("OnExplode");

        DeathSpawnManager.instance.Spawn(transform.position);

        manager.modelContainer.gameObject.SetActive(false);

        await Task.Delay(System.TimeSpan.FromSeconds(delayToDisable));
        manager.gameObject.SetActive(false);
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

}
