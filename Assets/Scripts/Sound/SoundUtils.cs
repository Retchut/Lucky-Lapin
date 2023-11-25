using System;
using UnityEngine;
//using Random = System.Random;

public static class SoundUtils
{

    [Serializable]
    public struct Sound
    {
        [Range(0, 3)] public float volume;
        public AudioClip clip;
    }

    public static void PlaySound(this AudioSource source, Sound sound)
    {
        source.PlayOneShot(sound.clip, sound.volume);
    }

    public static void PlaySoundVol(this AudioSource source, Sound sound, float volMultiplier)
    {
        source.PlayOneShot(sound.clip, sound.volume* volMultiplier);
    }

    public static void PlayRandomSound(this AudioSource source, Sound[] sounds)
    {
        PlaySound(source, sounds[UnityEngine.Random.Range(0,sounds.Length)]);
    }
}
