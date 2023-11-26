using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public Vector3 direction;
    public float rotationAmount = 0.1f;
    public float speed = 0.1f;
    public Color color;
    public MeshRenderer meshRenderer;
    public TriggerChecker checker;

    UEventHandler eventHandler = new UEventHandler();
    bool flag;
    private void Awake()
    {


    }
    void Start()
    {
        checker.OnTriggered.Subscribe(eventHandler, ChipCollided);
        meshRenderer.materials[1].color = color;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        meshRenderer.transform.Rotate(Vector3.forward, rotationAmount * Time.deltaTime);
    }

    void ChipCollided(Transform t)
    {
        if (flag) return;
        flag = true;

        if (t.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            HeartsUiManager.instance.TakeHit();
        }
        gameObject.SetActive(false);
    }
}
