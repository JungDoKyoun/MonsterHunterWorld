using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;
    public static UiManager Instance { get { Init(); return instance; } }

    float duration = 1f;

    private void Awake()
    {
        Init();
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("UIManager");
            if( go == null)
            {
                go = new GameObject { name = "UIManager" };
                go.AddComponent<UiManager>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<UiManager>();
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
