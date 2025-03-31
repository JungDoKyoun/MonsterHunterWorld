using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleLoginCanvasCtrl : MonoBehaviour
{
    [Header("��׶���� ĵ����")]
    [SerializeField] Canvas basicCanvas;

    [Header("ĵ���� �׷�")]
    [SerializeField] CanvasGroup titleCanvas;
    [SerializeField] CanvasGroup loginCanvas;

    [Header("��� ����")]
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
