using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum BGMType
    {
        Login,
        Single,
        Meeting,
        Boss,
        QuestCompleted,
        QuestFailed,
        End
    }

    public enum SfxQuestType
    {
        Interaction,
        CreateQuest,
        LeaveQuest,
        OpenBox,
        End
    }

    public enum IngameSfxType
    {
        HunterAttack,
        HunterAttack1,
        HunterDodge,
        HunterHit,
        BossRoar,
        BossAttack,
        BossAttack1,
        End
    }

    public static SoundManager Instance;

    [Header("����� �ҽ�")]
    [SerializeField] AudioSource bgmSource;     // BGM �����
    [SerializeField] AudioSource sfxSource;     // ȿ���� �����

    [Header("BGM Ŭ��")]
    [SerializeField] AudioClip[] bgmClips = new AudioClip[(int)BGMType.End];

    [Header("��ư Ŭ�� SFX")]
    [SerializeField] AudioClip[] buttonSFXs = new AudioClip[3];

    [Header("����Ʈ SFX")]
    [SerializeField] AudioClip[] questSfxs = new AudioClip[(int)SfxQuestType.End];

    [Header("�ΰ��� SFX")]
    [SerializeField] AudioClip[] inGameSFXs = new AudioClip[(int)IngameSfxType.End];

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

    public void PlayBGM(BGMType bgmType, float volume = 1.0f)
    {
        StopBGM();
        PlayBGM(bgmClips[(int)bgmType], volume);
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
    public void PlaySFX(SfxQuestType qusetType, float volume = 1.0f)
    {
        PlaySFX(questSfxs[(int)qusetType], volume);
    }

    public void PlaySFX(IngameSfxType ingameSfxType, float volume = 1.0f)
    {
        PlaySFX(inGameSFXs[(int)ingameSfxType], volume);
    }

    public void PlaySFX(AudioClip audioClip, float volume)
    {
        sfxSource.PlayOneShot(audioClip, volume);
    }

    // ��ư Ŭ���� ���
    public void PlayBtnClickSFX()
    {
        sfxSource.volume = 0.7f;
        sfxSource.PlayOneShot(buttonSFXs[0]);
    }

    // ��ư�� ���ٴ�� ���� �Ҹ�
    public void PlayWheelSFX()
    {
        sfxSource.volume = 0.7f;
        sfxSource.PlayOneShot(buttonSFXs[1]);
    }

    // ��ŸƮ ��ư Ŭ���� ���
    public void PlayStartButtonSFX()
    {
        sfxSource.volume = 0.7f;
        sfxSource.PlayOneShot(buttonSFXs[2]);
    }
}
