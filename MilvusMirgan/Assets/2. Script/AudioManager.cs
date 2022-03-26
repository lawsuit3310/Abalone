using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource AudioSource;
    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioSource.clip = clip;
        AudioSource.Play();
    }
}
