using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletData data;
    void Start()
    {

    }
    void Update()
    {
        transform.position += transform.forward * data.speed * Time.deltaTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyHitbox")
        {
            var enemyLogic = other.attachedRigidbody.GetComponent<EnemnyLogicManager>();
            enemyLogic.BulletHit();
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if(collision.collider.gameObject.layer!= LayerMask.NameToLayer("Enemy"))
        //gameObject.SetActive(false);

    }
}
