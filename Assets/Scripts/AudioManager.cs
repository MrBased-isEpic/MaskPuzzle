using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    #region Singleton
    
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion
    
    private AudioSource musicSource;
    private AudioSource sfxSource;

    public void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        musicSource = sources[0];
        sfxSource = sources[1];
    }
    
    public void PlayMusic(AudioClip clip, float volume)
    {
        if (musicSource.isPlaying)
        {
            StartCoroutine(FadeOutCurrentAndPlay(clip, volume));
            return;
        }
        
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutMusic());
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    private IEnumerator FadeOutCurrentAndPlay(AudioClip clip, float volume)
    {
        yield return StartCoroutine(FadeOutMusic());

        musicSource.volume = volume;
        musicSource.clip = clip;
        musicSource.Play();
    }

    private IEnumerator FadeOutMusic()
    {
        yield return StartCoroutine(Animations.LerpVolume(musicSource, 0, 1));
    }
}
