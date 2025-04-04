using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleLoginCanvasCtrl : MonoBehaviour
{
    [Header("��׶���� ĵ����")]
    [SerializeField] Canvas basicCanvas;

    [Header("ĵ���� �׷�")]
    [SerializeField] CanvasGroup titleCanvas;
    [SerializeField] CanvasGroup loginCanvas;

    [Header("�г� ����")]
    [SerializeField] Transform loginPanel;
    [SerializeField] Transform optionPanel;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        basicCanvas.gameObject.SetActive(true);
        titleCanvas.gameObject.SetActive(false);
        loginCanvas.gameObject.SetActive(false);

        UiManager.Instance.FadeInUI(titleCanvas);

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

        SoundManager.Instance.PlayBGM(SoundManager.BGMType.Login);
        //StartCoroutine(FadeInUI(titleCanvas));
    }

    private void Update()
    {
        if (Input.anyKeyDown && titleCanvas.gameObject.activeSelf)
        {
            UiManager.Instance.FadeOutUI(titleCanvas);
            UiManager.Instance.FadeInUI(loginCanvas);
        }
    }

    public void OptionButtonClick()
    {
        if (SceneManager.GetActiveScene().name == "TitleLoginScene")
        {
            loginPanel.gameObject.SetActive(false);
            optionPanel.gameObject.SetActive(true);
        }
    }

    public void LeaveOption()
    {
        if(SceneManager.GetActiveScene().name == "TitleLoginScene")
        {
            optionPanel.gameObject.SetActive(false);
            loginPanel.gameObject.SetActive(true);
        }
    }
}