using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    [Header("Äµ¹ö½º")]
    [SerializeField] CanvasGroup titleCanvas;
    [SerializeField] CanvasGroup loginCanvas;
    [SerializeField] CanvasGroup loadingCanvas;

    float duration = 1f;

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

}
