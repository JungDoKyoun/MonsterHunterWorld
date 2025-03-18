using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    [Header("캔버스")]
    [SerializeField] CanvasGroup titleCanvas;
    [SerializeField] CanvasGroup loginCanvas;
    [SerializeField] CanvasGroup loadingCanvas;

    [Header("로딩 이미지, 텍스트")]
    [SerializeField] Image loadingImage;
    [SerializeField] Text loadingText;

    float duration = 1f;
    float textInterval = 0.5f;

    private void Start()
    {
        StartCoroutine(FadeInUI(titleCanvas));
        loginCanvas.gameObject.SetActive(false);
        loadingCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(FadeOutUI(titleCanvas));
            StartCoroutine(FadeInUI(loginCanvas));
        }
    }

    IEnumerator FadeInUI(CanvasGroup canvas)
    {
        float elapsedTime = 0f;
        canvas.alpha = 0f;

        while ( elapsedTime < duration)
        {
            canvas.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 1f;
        canvas.gameObject.SetActive(true);
    }

    IEnumerator FadeOutUI(CanvasGroup canvas)
    {
        canvas.interactable = false;
        float elapsedTime = 0f;
        canvas.alpha = 1f;

        while( elapsedTime < duration)
        {
            canvas.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 0f;
        canvas.gameObject.SetActive(false);
    }

    IEnumerator AnimationLoading()
    {
        int dotCount = 0;

        loadingImage.transform.Rotate(new Vector3(0,0,30f * Time.deltaTime));

        while (true)
        {
            dotCount = (dotCount + 1) % 4;
            loadingText.text = "로딩중" + new string('.', dotCount);
            yield return new WaitForSeconds(textInterval);
        }
    }

}
