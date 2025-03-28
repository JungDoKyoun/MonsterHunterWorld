using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("오디오 소스")]
    [SerializeField] AudioSource bgmSource;     // BGM 재생용
    [SerializeField] AudioSource sfxSource;     // 효과음 재생용

    [Header("오디오 클립")]
    [SerializeField] AudioClip bgmClip;         // 마을BGM 클립

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private void Start()
    {
        PlayBGM(bgmClip);
    }

    public void PlayBGM(AudioClip audioClip, float volume = 1.0f)
    {
        if(bgmSource.clip == audioClip && bgmSource.isPlaying)
        {
            return;
        }
        bgmSource.clip = audioClip;
        bgmSource.volume = volume;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip audioClip, float volume = 1.0f)
    {
        sfxSource.PlayOneShot(audioClip, volume);
    }
}
