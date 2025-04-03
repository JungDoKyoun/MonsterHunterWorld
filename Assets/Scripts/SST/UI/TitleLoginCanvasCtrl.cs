using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleLoginCanvasCtrl : MonoBehaviour
{
    [Header("백그라운드용 캔버스")]
    [SerializeField] Canvas basicCanvas;

    [Header("캔버스 그룹")]
    [SerializeField] CanvasGroup titleCanvas;
    [SerializeField] CanvasGroup loginCanvas;

    [Header("패널 관리")]
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