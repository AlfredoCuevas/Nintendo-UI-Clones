using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource[] _audioSources;

    private bool _isFadingIn = false;
    private bool _isFadingOut = false;

    private void Awake()
    {
        KeepOnlyOneAudioManager();

        _audioSources = GetComponentsInChildren<AudioSource>();
    }

    public bool IsPlaying(string soundGameObjectName)
    {
        AudioSource sound = _audioSources.FirstOrDefault(t => t.gameObject.name == soundGameObjectName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: (" + soundGameObjectName + ") not found while calling IsPlaying()");
            return false;
        }

        return sound.isPlaying;
    }

    public void Play(string soundGameObjectName)
    {
        AudioSource sound = _audioSources.FirstOrDefault(t => t.gameObject.name == soundGameObjectName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: (" + soundGameObjectName + ") not found");
            return;
        }

        sound.Play();
    }

    public void PlayOneShot(string soundGameObjectName)
    {
        AudioSource sound = _audioSources.FirstOrDefault(t => t.gameObject.name == soundGameObjectName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: (" + soundGameObjectName + ") not found");
            return;
        }

        sound.PlayOneShot(sound.clip);
    }

    public void Stop(string soundGameObjectName)
    {
        AudioSource sound = _audioSources.FirstOrDefault(t => t.gameObject.name == soundGameObjectName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundGameObjectName + " not found");
            return;
        }

        sound.Stop();
    }

    // volumeLevel is only affected by values from 0-1. Anything else is clamped
    public void SetVolume(string soundGameObjectName, float volumeLevel)
    {
        AudioSource sound = _audioSources.FirstOrDefault(t => t.gameObject.name == soundGameObjectName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundGameObjectName + " not found");
            return;
        }

        sound.volume = volumeLevel;
    }

    public void IncrementVolume(string soundGameObjectName, float volumeIncrement)
    {
        AudioSource sound = _audioSources.FirstOrDefault(t => t.gameObject.name == soundGameObjectName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundGameObjectName + " not found");
            return;
        }

        sound.volume += volumeIncrement;
    }

    // If already Fading Out this function will wait for the FadeOut to finish
    public void FadeInSound(string soundGameObjectName, float fadeTime)
    {
        AudioSource sound = _audioSources.FirstOrDefault(t => t.gameObject.name == soundGameObjectName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundGameObjectName + " not found");
            return;
        }

        if (!_isFadingIn)
        {
            StartCoroutine(FadeInOverTime(sound, fadeTime));
        }
    }

    // If already Fading In this funciton will wait for the FadeIn to finish
    public void FadeOutSound(string soundGameObjectName, float fadeTime)
    {
        AudioSource sound = _audioSources.FirstOrDefault(t => t.gameObject.name == soundGameObjectName);

        if (sound == null)
        {
            Debug.LogWarning("Sound: " + soundGameObjectName + " not found");
            return;
        }

        if (!_isFadingOut)
        {
            StartCoroutine(FadeOutOverTime(sound, fadeTime));
        }
    }

    private IEnumerator FadeInOverTime(AudioSource audioSource, float fadeTime)
    {
        while (_isFadingOut)
        {
            _isFadingIn = true;
            yield return null;
        }
        _isFadingIn = true;

        float startVolume = audioSource.volume;
        audioSource.volume = 0f;
        audioSource.Play();

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.volume = startVolume;
        _isFadingIn = false;
    }

    private IEnumerator FadeOutOverTime(AudioSource audioSource, float fadeTime)
    {
        while (_isFadingIn)
        {
            _isFadingOut = true;
            yield return null;
        }
        _isFadingOut = true;

        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
        _isFadingOut = false;
    }

    private void KeepOnlyOneAudioManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
