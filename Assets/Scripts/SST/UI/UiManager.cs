using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;


    float duration = 1f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    public void FadeInUI(CanvasGroup canvas)
    {
        StartCoroutine(FadeInUICor(canvas));
    }

    IEnumerator FadeInUICor(CanvasGroup canvas)
    {
        float elapsedTime = 0f;
        canvas.alpha = 0f;
        canvas.gameObject.SetActive(true);
        canvas.interactable = false;

        while (elapsedTime < duration)
        {
            canvas.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 1f;
        canvas.interactable = true;
    }

    public void FadeOutUI(CanvasGroup canvas)
    {
        StartCoroutine(FadeOutUICor(canvas));
    }

    IEnumerator FadeOutUICor(CanvasGroup canvas)
    {
        canvas.interactable = false;
        float elapsedTime = 0f;
        canvas.alpha = 1f;

        while (elapsedTime < duration)
        {
            canvas.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 0f;
        canvas.gameObject.SetActive(false);
    }
}
