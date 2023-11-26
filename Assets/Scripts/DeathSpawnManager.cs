using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DeathSpawnManager : MonoBehaviour
{
    public static DeathSpawnManager instance;
    public float chanceOfSpawn = 30;
    public GameObject[] items;
    public VisualEffect vfx;
    

    public float testDistance = 1.5f;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    [Button]
    public void SpawnInFront()
    {
        Spawn(PlayerMovControllerFloat.instance.transform.position + PlayerMovControllerFloat.instance.transform.forward * testDistance);
    }

    public void Spawn(Vector3 pos)
    {
        vfx.SetVector3("Pos", PlayerMovControllerFloat.instance.transform.position + PlayerMovControllerFloat.instance.transform.forward * testDistance);
        vfx.SendEvent("OnExplode");
        float randomValue = Random.Range(0f, 100f);

        // Check if the random value is within the specified chance percentage
        if (randomValue <= chanceOfSpawn)
        {
            GameObject obj = GameObject.Instantiate(items[Random.Range(0, items.Length)]);

            obj.transform.position = pos;
        }

    }

}
