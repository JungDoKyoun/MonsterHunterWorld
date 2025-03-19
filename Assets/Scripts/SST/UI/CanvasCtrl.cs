using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    [Header("��׶���� ĵ����")]
    [SerializeField] Canvas basicCanvas;

    [Header("ĵ���� �׷�")]
    [SerializeField] CanvasGroup titleCanvas;
    [SerializeField] CanvasGroup loginCanvas;
    [SerializeField] CanvasGroup loadingCanvas;

    [Header("�ε� �̹���, �ؽ�Ʈ")]
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
