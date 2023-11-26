using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipThrower : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemnyLogicManager logicMang;

    public GameObject chipPlaceholder;
    public float distanceStart = 1.1f;


    private void Awake()
    {
    }

    [Button]

    public void Throw()
    {
        GameObject chip = GameObject.Instantiate(chipPlaceholder);

        chip.transform.position = transform.position + (transform.forward * distanceStart);

        //chip.transform.rotation= 
        Chip chipComp = chip.GetComponent<Chip>();
        chipComp.color = logicMang.randomColor;
        chipComp.direction = transform.forward;
        //chipComp.

    }
}
