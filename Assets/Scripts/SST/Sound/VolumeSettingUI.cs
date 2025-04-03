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
        // JSON ���Ͽ��� ����� ������ �ҷ�����
        VolumeSettings settings = VolumeSettingManager.Instance.LoadVolumeSettings();

        // �����̴��� �ҷ��� ������ �������ش�
        masterSlider.value = settings.masterVolume;
        bgmSlider.value = settings.bgmVolume;
        sfxSlider.value = settings.sfxVolume;

        // AudioMixer���� �������ش�
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.Master, settings.masterVolume);
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.BGM, settings.bgmVolume);
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.SFX, settings.sfxVolume);
    }

    // �����̴� OnChangeValue�� �־��� �Լ���
    public void OnMasterVolumeChanged(float value)
    {
        // �ٲ���� �����̴� ���� ������ͼ��� �������ְ�
        AudioMixManager.Instance.SetAudioVolume(AudioMixManager.AudioMixType.Master, value);
        // �� �ٲ���� ���� ������ JSON ���Ͽ� �������ش�.
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

    // �����̴� ������ �ٲ���� ���� �������ִ� �Լ�
    private void SaveCurrentSettings()
    {
        // ������ ���ð��� ������ ���ο� ��ü�� �������ְ�
        VolumeSettings currentSettings = new VolumeSettings
        {
            // �ش� ��ü�� ���������� ���� �����̴��� ������ �������ش�.
            masterVolume = masterSlider.value,
            bgmVolume = bgmSlider.value,
            sfxVolume = sfxSlider.value,
        };
        // ������ ���ð��� ������ ��ü�� �������ֱ� ���� �Ŵ��� �Լ��� ȣ���Ѵ�.
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
