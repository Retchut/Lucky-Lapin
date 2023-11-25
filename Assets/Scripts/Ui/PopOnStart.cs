using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

public class PopOnStart : MonoBehaviour
{
    public float popDuration = 0.5f;
    public Ease popEase = Ease.OutBack;
    public int msDelay = 1000;

    async void Start()
    {
        transform.localScale = Vector3.zero;
        await Task.Delay(msDelay);

        transform.DOScale(Vector3.one, popDuration).SetEase(popEase);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
