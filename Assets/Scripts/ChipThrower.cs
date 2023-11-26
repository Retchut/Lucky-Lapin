using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipThrower : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject chipPlaceholder;
    public float throwSpeed;
    public float throwRotation;


    public void Throw()
    {
        GameObject chip=  GameObject.Instantiate(chipPlaceholder);

        chip.transform.position= transform.position;

        //chip.transform.rotation= 
        //Chip chipComp= chip.GetComponent<Chip>();

        //chipComp.

    }
}
