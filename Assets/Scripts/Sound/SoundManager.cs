using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundUtils;
using DG.Tweening;
using System.Threading.Tasks;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Range(0, 3)]
    public float musicVolume = 1f;

    [Range(0, 3)]
    public float sfxVolume = 1;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

    }

    public void PlaySFX(Sound sound)
    {
        sfxSource.PlaySound(sound);
    }

    public async void PlayMusic(Sound sound, bool loop = true, bool fadeIn = true, float fadeTime = .5f)
    {
        musicSource.loop = loop;

        if (musicSource.isPlaying && fadeIn)
        {
            musicSource.DOFade(0, fadeTime);
            await Task.Delay(Mathf.RoundToInt((fadeTime * 100)));
            musicSource.clip = sound.clip;
            musicSource.DOFade(musicVolume * sound.volume, fadeTime);
        }
        else if (fadeIn)
        {
            musicSource.volume = 0;
            musicSource.clip = sound.clip;
            musicSource.DOFade(musicVolume * sound.volume, fadeTime);
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.volume = musicVolume * sound.volume;
            musicSource.Play();
        }
    }



}
