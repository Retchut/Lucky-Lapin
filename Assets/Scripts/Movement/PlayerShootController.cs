using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public PlayerInputHandlerPlatformer inputHandler;
    public PlayerMovControllerFloat movController;
    public Transform gunModel;

    public BulletData bulletDataPlaceholder;
    public Bullet bulletPlaceholder;

    public List<Bullet> bulletPool;

    public float startingDistance;
    public Vector3 offset;

    public float fireRate = 0.5f;

    public float lastShootTime = -1;

    public UEventHandler eventHandler;
    void Start()
    {
        //inputHandler.input_interact.Onpressed.Subscribe(eventHandler, Shoot);
    }


    private void Update()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (inputHandler.input_interact.value <= 0) return;

        float currentShootTime = Time.time;

        if (lastShootTime != -1 && currentShootTime - lastShootTime <= fireRate) return;


        var bullet = GameObject.Instantiate(bulletPlaceholder);
        bullet.data = bulletDataPlaceholder;

        //var direction = movController.horizontalVel.normalized;
        var direction = gunModel.forward;

        bullet.transform.position = transform.position + direction * startingDistance + offset;
        bullet.transform.forward = direction;

        lastShootTime = currentShootTime;
    }

}
