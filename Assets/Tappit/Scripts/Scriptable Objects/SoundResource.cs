using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundResource : ScriptableObject {

    public AudioClip[] Clips;

    public float Volume = 1.0f;

    public float VolumeVariant = 0f;

    public float Pitch = 1.0f;

    public float PitchVariant = 0f;

    public bool Loop = false;

    public float Delay = 0f;

    public AudioSource Play()
    {
        AudioClip audioClip = Clips[Random.Range(0, Clips.Length)];

        AudioSource audioSource = SoundManager.PlaySFX(audioClip, Loop, Delay, Volume + Random.Range(0f, VolumeVariant), Pitch + Random.Range(0f, PitchVariant));

        return audioSource;
    }
}
