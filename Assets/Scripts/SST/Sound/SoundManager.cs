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
        Boss,
        QuestCompleted,
        QuestFailed,
        End
    }

    public static SoundManager Instance;

    [Header("오디오 소스")]
    [SerializeField] AudioSource bgmSource;     // BGM 재생용
    [SerializeField] AudioSource sfxSource;     // 효과음 재생용

    [Header("BGM 클립")]
    [SerializeField] AudioClip[] bgmClips = new AudioClip[(int)SoundType.End];

    [Header("버튼 클릭 SFX")]
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
        PlayBGM(bgmClips[(int)soundType], volume);
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

    // SFX도 타입으로 하나 만들어서 실행하게 바꿀 예정
    public void PlaySFX(SoundType soundType, float volume = 1.0f)
    {
        PlaySFX(bgmClips[(int)soundType], volume);
    }

    public void PlaySFX(AudioClip audioClip, float volume)
    {
        sfxSource.PlayOneShot(audioClip, volume);
    }

    // 버튼 클릭음 재생
    public void PlayBtnClickSFX()
    {
        sfxSource.volume = 0.7f;
        sfxSource.PlayOneShot(buttonSFX[0]);
    }

    // 버튼에 갖다대면 나는 소리
    public void PlayWheelSFX()
    {
        sfxSource.volume = 0.7f;
        sfxSource.PlayOneShot(buttonSFX[1]);
    }

    // 스타트 버튼 클릭음 재생
    public void PlayStartButtonSFX()
    {
        sfxSource.volume = 0.7f;
        sfxSource.PlayOneShot(buttonSFX[2]);
    }
}
