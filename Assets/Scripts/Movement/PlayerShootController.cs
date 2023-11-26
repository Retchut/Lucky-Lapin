using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public static PlayerShootController instance;

    public PlayerVFXManagerPlatformer vfxManager;

    public PlayerInputHandlerPlatformer inputHandler;
    public PlayerMovControllerFloat movController;
    public Transform gunModel;

    public Light shotLight;

    public BulletData bulletDataPlaceholder;
    public Bullet bulletPlaceholder;

    public List<Bullet> bulletPool;

    public float startingDistance;
    public Vector3 offset;

    public float fireRate = 0.5f;

    public float lastShootTime = -1;
    public float castRadius = 0.1f;
    public float castDistance = 25;
    public LayerMask shootLayers = ~1;

    public float lightSeconds = 0.1f;
    public UEventHandler eventHandler;


    private void Awake()
    {
        instance = this;
        shotLight.enabled = false;
    }
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

        if (!RolletUIManager.instance.TestShoot()) return;

        float currentShootTime = Time.time;

        if (lastShootTime != -1 && currentShootTime - lastShootTime <= fireRate) return;


        var bullet = GameObject.Instantiate(bulletPlaceholder);
        bullet.data = bulletDataPlaceholder;

        //var direction = movController.horizontalVel.normalized;
        var direction = movController.horizontalDirection == Vector3.zero ? gunModel.forward : movController.horizontalDirection;


        //RaycastHit[] hits = new RaycastHit[3];
        RaycastHit hit1;
        RaycastHit hit2;
        RaycastHit hit3;

        float rotDif = 9;
        var dir2 = Quaternion.EulerAngles(new Vector3(0, rotDif)) * direction;
        dir2 = -dir2;

        var dir3 = Quaternion.EulerAngles(new Vector3(0, -rotDif)) * direction;
        dir3 = -dir3;

        Vector3 finalDirection = direction;


        bool hashit1 = Physics.SphereCast(transform.position, castRadius, direction, out hit1, castDistance, shootLayers, QueryTriggerInteraction.Collide);
        bool hashit2 = Physics.SphereCast(transform.position, castRadius, dir2, out hit2, castDistance, shootLayers, QueryTriggerInteraction.Collide);
        bool hashit3 = Physics.SphereCast(transform.position, castRadius, dir3, out hit3, castDistance, shootLayers, QueryTriggerInteraction.Collide);

        RaycastHit? closestHit = null;

        if (hashit1)
            closestHit = hit1;

        if (hashit2)
        {
            if (!closestHit.HasValue || closestHit.Value.distance > hit2.distance)
                closestHit = hit2;
        }

        if (hashit3)
        {
            if (!closestHit.HasValue || closestHit.Value.distance > hit3.distance)
                closestHit = hit3;
        }

        if (closestHit.HasValue)
        {
            finalDirection = closestHit.Value.collider.transform.position - transform.position;
        }

        finalDirection.y = 0;

        Debug.DrawRay(transform.position, direction, Color.red, 5);
        Debug.DrawRay(transform.position, dir2, Color.cyan, 5);
        Debug.DrawRay(transform.position, dir3, Color.green, 5);

        bullet.transform.position = transform.position + direction * startingDistance + offset;
        bullet.transform.forward = finalDirection;

        lastShootTime = currentShootTime;
        ShowEffect();
    }

    public async void ShowEffect()
    {
        RolletUIManager.instance.Next();
        shotLight.enabled = true;
        await Task.Delay(System.TimeSpan.FromSeconds(lightSeconds));
        shotLight.enabled = false;
    }

    public void BlowUp()
    {
        CameraShaker.instance.Shake(.4f);

        movController.Knockback(-gunModel.forward);

        vfxManager.Explode();
        HeartsUiManager.instance.PutToSoro();
    }

}
