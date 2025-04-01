using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        Login,
        Single,
        Meeting,
        End
    }

    public static SoundManager Instance;

    [Header("����� �ҽ�")]
    [SerializeField] AudioSource bgmSource;     // BGM �����
    [SerializeField] AudioSource sfxSource;     // ȿ���� �����

    [Header("����� Ŭ��")]
    [SerializeField] AudioClip[] audioClips = new AudioClip[(int)SoundType.End];

    [Header("��ư Ŭ�� SFX")]
    [SerializeField] AudioClip[] buttonSFX = new AudioClip[3];

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

        DontDestroyOnLoad(Instance);
    }

    public void PlayBGM(SoundType soundType, float volume = 1.0f)
    {
        PlayBGM(audioClips[(int)soundType], volume);
    }

    private void PlayBGM(AudioClip audioClip, float volume)
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

    // SFX�� Ÿ������ �ϳ� ���� �����ϰ� �ٲ� ����
    public void PlaySFX(SoundType soundType, float volume = 1.0f)
    {
        PlaySFX(audioClips[(int)soundType], volume);
    }

    public void PlaySFX(AudioClip audioClip, float volume)
    {
        sfxSource.PlayOneShot(audioClip, volume);
    }

    // ��ư Ŭ���� ���
    public void PlayBtnClickSFX()
    {
        sfxSource.PlayOneShot(buttonSFX[0]);
    }

    // ��ư�� ���ٴ�� ���� �Ҹ�? ���� ��� �־������ �𸣰���
    public void PlayWheelSFX()
    {
        sfxSource.PlayOneShot(buttonSFX[1]);
    }

    // ��ŸƮ ��ư Ŭ���� ���
    public void PlayStartButtonSFX()
    {
        sfxSource.PlayOneShot(buttonSFX[2]);
    }
}
