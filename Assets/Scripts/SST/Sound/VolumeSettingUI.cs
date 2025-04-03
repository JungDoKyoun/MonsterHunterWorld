using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeSettingUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // JSON 파일에서 저장된 볼륨값 불러오기
        VolumeSettings settings = VolumeSettingManager.Instance.LoadVolumeSettings();

        // 슬라이더에 불러온 값들을 적용해준다
        masterSlider.value = settings.masterVolume;
        bgmSlider.value = settings.bgmVolume;
        sfxSlider.value = settings.sfxVolume;

        // AudioMixer에도 적용해준다
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.Master, settings.masterVolume);
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.BGM, settings.bgmVolume);
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.SFX, settings.sfxVolume);
    }

    // 슬라이더 OnChangeValue에 넣어줄 함수들
    public void OnMasterVolumeChanged(float value)
    {
        // 바뀌어진 슬라이더 값을 오디오믹서에 적용해주고
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.Master, value);
        // 그 바뀌어진 값을 현재의 JSON 파일에 저장해준다.
        SaveCurrentSettings();
    }

    public void OnBGMVolumeChanged(float value)
    {
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.BGM, value);
        SaveCurrentSettings();
    }

    public void OnSFXVolumeChanged(float value)
    {
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.SFX, value);
        SaveCurrentSettings();
    }

    // 슬라이더 조절로 바뀌어진 값을 저장해주는 함수
    private void SaveCurrentSettings()
    {
        // 현재의 세팅값을 저장할 새로운 객체를 생성해주고
        VolumeSettings currentSettings = new VolumeSettings
        {
            // 해당 객체의 볼륨값들을 현재 슬라이더의 값으로 저장해준다.
            masterVolume = masterSlider.value,
            bgmVolume = bgmSlider.value,
            sfxVolume = sfxSlider.value,
        };
        // 현재의 세팅값을 저장한 객체를 저장해주기 위해 매니저 함수를 호출한다.
        VolumeSettingManager.Instance.SaveVolumeSettings(currentSettings);
    }

    public void OnButtonClick()
    {
        SoundManager.Instance.PlayBtnClickSFX();
    }

    public void LeaveOption()
    {
        if ((SceneManager.GetActiveScene().name == "TitleLoginScene") == false)
        {
            UIManager.Instance.CloseTopUI();
        }
    }

    public void PlayWheelSFX()
    {
        SoundManager.Instance.PlayWheelSFX();
    }
}
