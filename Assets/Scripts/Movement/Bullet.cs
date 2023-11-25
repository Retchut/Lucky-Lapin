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
        transform.position += transform.forward * data.speed;
    }
}
