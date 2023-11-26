using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemnyLogicManager : LogicMachineManager
{

    public PlayerMovControllerFloat playerMovController;
    public TriggerChecker triggerChecker;
    public Collider mainCollider;
    public Rigidbody body;
    public VisualEffect vfx;
    public Animator animator;

    public MeshRenderer[] colorMeshes;
    public Color[] meshRandomColor;

    public MeshRenderer cardMesh;
    public Texture2D[] spriteRandomCard;

    public override void OnAwake()
    {
        if (colorMeshes != null && meshRandomColor != null && meshRandomColor.Length> 0)
        {
            Color randomColor = meshRandomColor[Random.Range(0, meshRandomColor.Length)];
            for (int i = 0; i < colorMeshes.Length; i++)
            {
                if (i == colorMeshes.Length - 1)
                {
                    colorMeshes[i].materials[2].color = randomColor;
                }
                else
                {
                    colorMeshes[i].materials[1].color = randomColor;

                }

            }
            foreach (var colorMesh in colorMeshes)
            {
            }
        }

        if (cardMesh != null)
        {
            cardMesh.material.mainTexture = spriteRandomCard[Random.Range(0, spriteRandomCard.Length)];
        }
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
