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

    // 퀘스트 상호작용 SFX
    public enum SfxQuestType
    {
        Interaction,
        CreateQuest,
        LeaveQuest,
        OpenBox,
        End
    }

    // 헌터 전용 SFX
    public enum HunterSfxType
    {
        HunterAttack,
        HunterAttack1,
        HunterDodge,
        HunterHit,
        End
    }

    // 보스 전용 SFX
    public enum BossSfxType
    {
        BossRoar,
        BossAttack,
        BossAttack1,
        BossFire,
        End
    }

    public static SoundManager Instance;

    [Header("오디오 소스")]
    [SerializeField] AudioSource bgmSource;     // BGM 재생용
    [SerializeField] AudioSource sfxSource;     // 효과음 재생용 , 플레이어 효과음
    [SerializeField] AudioSource sfxSource2;    // 보스 효과음 재생

    [Header("BGM 클립")]
    [SerializeField] AudioClip[] bgmClips = new AudioClip[(int)BGMType.End];

    [Header("버튼 클릭 SFX")]
    [SerializeField] AudioClip[] buttonSFXs = new AudioClip[3];

    [Header("퀘스트 SFX")]
    [SerializeField] AudioClip[] questSfxs = new AudioClip[(int)SfxQuestType.End];

    [Header("플레이어 SFX")]
    [SerializeField] AudioClip[] hunterSfxs = new AudioClip[(int)HunterSfxType.End];

    [Header("보스 SFX")]
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

    // SFX는 Enum 타입별로 오버로드해놓음
    // 퀘스트 상호작용 SFX 재생
    public void PlaySFX(SfxQuestType qusetType)
    {
        PlaySFX(questSfxs[(int)qusetType]);
    }

    // 헌터(플레이어) 전용 SFX 재생
    public void PlaySFX(HunterSfxType hunterSfxType)
    {
        PlaySFX(hunterSfxs[(int)hunterSfxType]);
    }

    // 위의 함수를 받아서 재생
    private void PlaySFX(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    // 보스 전용 SFX 플레이 함수
    // 다른 오디오 소스에서 재생해서 플레이어 SFX와 동시에 재생하기 위함
    public void PlaySFX(BossSfxType bossSfxType)
    {
        PlayBossSFX(bossSFXs[(int)bossSfxType]);
    }

    // 위의 함수를 받아서 재생
    private void PlayBossSFX(AudioClip audioClip)
    {
        sfxSource2.PlayOneShot(audioClip);
    }

    // 버튼 클릭음 재생
    public void PlayBtnClickSFX()
    {
        sfxSource.PlayOneShot(buttonSFXs[0]);
    }

    // 버튼에 갖다대면 나는 소리
    public void PlayWheelSFX()
    {
        sfxSource.PlayOneShot(buttonSFXs[1]);
    }

    // 스타트 버튼 클릭음 재생
    public void PlayStartButtonSFX()
    {
        sfxSource.PlayOneShot(buttonSFXs[2]);
    }
}
