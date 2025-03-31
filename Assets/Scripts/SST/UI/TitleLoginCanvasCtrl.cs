using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleLoginCanvasCtrl : MonoBehaviour
{
    [Header("백그라운드용 캔버스")]
    [SerializeField] Canvas basicCanvas;

    [Header("캔버스 그룹")]
    [SerializeField] CanvasGroup titleCanvas;
    [SerializeField] CanvasGroup loginCanvas;

    [Header("배경 음악")]
    [SerializeField] AudioClip titleBgm;


    private void Start()
    {
        basicCanvas.gameObject.SetActive(true);
        titleCanvas.gameObject.SetActive(false);
        loginCanvas.gameObject.SetActive(false);

        UiManager.Instance.FadeInUI(titleCanvas);
        SoundManager.Instance.PlayBGM(titleBgm, 0.2f);
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
}
