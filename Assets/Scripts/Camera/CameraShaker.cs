using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Threading.Tasks;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance;
    public float bigShakeIntensity = 5;
    public float smallShakeIntensity = 2;
    public CinemachineVirtualCamera vCam;
    float shakeValue;
    CinemachineBasicMultiChannelPerlin noiseSource;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        noiseSource = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public async void Shake(float duration, bool smallShake = false, bool fadeIntensity = false)
    {
        if (fadeIntensity)
            DOTween.To(() => shakeValue, x => noiseSource.m_AmplitudeGain = x, smallShake ? smallShakeIntensity : bigShakeIntensity, duration / 2).SetLoops(2, LoopType.Yoyo);
        else
        {
            noiseSource.m_AmplitudeGain = smallShake ? smallShakeIntensity : bigShakeIntensity;
            await Task.Delay((int)(duration * 1000));
            noiseSource.m_AmplitudeGain = 0;
        }
    }

}
