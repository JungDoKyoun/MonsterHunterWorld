using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // BGM
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

    // ����Ʈ ��ȣ�ۿ� SFX
    public enum SfxQuestType
    {
        Interaction,
        CreateQuest,
        LeaveQuest,
        OpenBox,
        End
    }

    // ���� ���� SFX
    public enum HunterSfxType
    {
        HunterAttack,
        HunterAttack1,
        HunterDodge,
        HunterHit,
        End
    }

    // ���� ���� SFX
    public enum BossSfxType
    {
        BossRoar,
        BossAttack,
        BossAttack1,
        BossFire,
        End
    }

    public static SoundManager Instance;

    [Header("����� �ҽ�")]
    [SerializeField] AudioSource bgmSource;     // BGM �����
    [SerializeField] AudioSource sfxSource;     // ȿ���� ����� , �÷��̾� ȿ����
    [SerializeField] AudioSource sfxSource2;    // ���� ȿ���� ���

    [Header("BGM Ŭ��")]
    [SerializeField] AudioClip[] bgmClips = new AudioClip[(int)BGMType.End];

    [Header("��ư Ŭ�� SFX")]
    [SerializeField] AudioClip[] buttonSFXs = new AudioClip[3];

    [Header("����Ʈ SFX")]
    [SerializeField] AudioClip[] questSfxs = new AudioClip[(int)SfxQuestType.End];

    [Header("�÷��̾� SFX")]
    [SerializeField] AudioClip[] hunterSfxs = new AudioClip[(int)HunterSfxType.End];

    [Header("���� SFX")]
    [SerializeField] AudioClip[] bossSFXs = new AudioClip[(int)BossSfxType.End];

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

    public void PlayBGM(BGMType bgmType)
    {
        StopBGM();
        PlayBGM(bgmClips[(int)bgmType]);
    }

    private void PlayBGM(AudioClip audioClip)
    {
        if(bgmSource.clip == audioClip && bgmSource.isPlaying)
        {
            return;
        }
        bgmSource.clip = audioClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // SFX�� Enum Ÿ�Ժ��� �����ε��س���
    // ����Ʈ ��ȣ�ۿ� SFX ���
    public void PlaySFX(SfxQuestType qusetType)
    {
        PlaySFX(questSfxs[(int)qusetType]);
    }

    // ����(�÷��̾�) ���� SFX ���
    public void PlaySFX(HunterSfxType hunterSfxType)
    {
        PlaySFX(hunterSfxs[(int)hunterSfxType]);
    }

    // ���� �Լ��� �޾Ƽ� ���
    private void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    // ���� ���� SFX �÷��� �Լ�
    // �ٸ� ����� �ҽ����� ����ؼ� �÷��̾� SFX�� ���ÿ� ����ϱ� ����
    public void PlaySFX(BossSfxType bossSfxType)
    {
        PlayBossSFX(bossSFXs[(int)bossSfxType]);
    }

    // ���� �Լ��� �޾Ƽ� ���
    private void PlayBossSFX(AudioClip audioClip)
    {
        sfxSource2.PlayOneShot(audioClip);
    }

    // ��ư Ŭ���� ���
    public void PlayBtnClickSFX()
    {
        sfxSource.PlayOneShot(buttonSFXs[0]);
    }

    // ��ư�� ���ٴ�� ���� �Ҹ�
    public void PlayWheelSFX()
    {
        sfxSource.PlayOneShot(buttonSFXs[1]);
    }

    // ��ŸƮ ��ư Ŭ���� ���
    public void PlayStartButtonSFX()
    {
        sfxSource.PlayOneShot(buttonSFXs[2]);
    }
}
