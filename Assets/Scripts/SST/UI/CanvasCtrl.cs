using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    [Header("백그라운드용 캔버스")]
    [SerializeField] Canvas basicCanvas;

    [Header("캔버스 그룹")]
    [SerializeField] CanvasGroup titleCanvas;
    [SerializeField] CanvasGroup loginCanvas;
    [SerializeField] CanvasGroup loadingCanvas;

    [Header("로딩 이미지, 텍스트")]
    [SerializeField] Image loadingImage;
    [SerializeField] Text loadingText;

    private void Start()
    {
        basicCanvas.gameObject.SetActive(true);
        titleCanvas.gameObject.SetActive(false);
        loginCanvas.gameObject.SetActive(false);
        loadingCanvas.gameObject.SetActive(false);

        UiManager.Instance.FadeInUI(titleCanvas);
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
