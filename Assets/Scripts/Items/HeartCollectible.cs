using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

public class HeartCollectible : MonoBehaviour
{
    public bool collected;

    public Transform model;

    public TriggerChecker triggerChecker;
    public float contSpinAmnt = 0.1f;

    public float finalSpinAmnt = 560f;
    public float finalSpinDuration = 5;
    public AnimationCurve finalSpinEase;

    public SoundUtils.Sound[] heartsSounds;

    public AudioSource aSource;

    UEventHandler eventHandler = new UEventHandler();
    private void Awake()
    {
        triggerChecker.OnTriggered.Subscribe(eventHandler, PlayerEntered);
    }



    void PlayerEntered(Transform triggered)
    {
        if (collected) return;

        Collect();
    }
    [Button]
    public void Collect()
    {
        collected = true;
        HeartsUiManager.instance.GainHeart();

        aSource.PlayRandomSound(heartsSounds);

        model.DORotate(Vector3.up * finalSpinAmnt, finalSpinDuration, RotateMode.WorldAxisAdd).SetEase(finalSpinEase);
        model.DOScale(0, finalSpinDuration).SetEase(finalSpinEase);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Spin();
    }

    void Spin()
    {
        if (collected) return;

        model.Rotate(Vector3.up, contSpinAmnt * Time.deltaTime);
    }
}
