using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("����� �ҽ�")]
    [SerializeField] AudioSource bgmSource;     // BGM �����
    [SerializeField] AudioSource sfxSource;     // ȿ���� �����

    [Header("����� Ŭ��")]
    [SerializeField] AudioClip bgmClip;         // ����BGM Ŭ��

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
