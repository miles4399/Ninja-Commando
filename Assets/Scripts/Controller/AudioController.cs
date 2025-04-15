using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController> {
    private AudioSource _audioSource;

    protected override void OnAwake()
    {
        _audioSource = GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }
    public void PlaySoundOneShot(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
    public void StopMusic(AudioClip clip)
    {
        _audioSource.Stop();
    }
    public void PlayMusic(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}