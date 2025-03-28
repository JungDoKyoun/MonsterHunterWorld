using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Photon.Pun;
using Photon.Realtime;

public class LoadingSceneManager : MonoBehaviourPunCallbacks
{
    public static string sceneToLoad;

    [Header("로딩 효과")]
    [SerializeField] CanvasGroup loadingCanvas;         // 페이드인,아웃 화면
    [SerializeField] Image loadingImage;                // 회전할 로딩 이미지
    [SerializeField] Text loadingText;                  // 깜빡이는 로딩 텍스트

    private float fadeDuration = 1f;         // 페이드 효과 지속시간
    private float blinkSpeed = 10f;          // 텍스트 깜빡임 속도

    Coroutine blinkCor;
    Coroutine rotateCor;

    RoomOptions single = new RoomOptions { MaxPlayers = 1 };

    private void Start()
    {
        // 시작과 동시에 이미지 회전, 텍스트 깜빡임 효과 코루틴 실행
        blinkCor = StartCoroutine(BlinkText());
        rotateCor = StartCoroutine(RotateImage());
        StartCoroutine(PlayLoadScene());
        SetCreateRoom(sceneToLoad, single);
    }

    public void SetCreateRoom(string toWhere, RoomOptions roomOptions)
    {
        PhotonNetwork.CreateRoom(toWhere, roomOptions);
    }

    IEnumerator PlayLoadScene()
    {
        float minLoadingTime = 3f;
        float startTime = 0f;

        // 시작과 동시에 이미지 회전, 텍스트 깜빡임 효과 코루틴 실행
        //Coroutine blinkCor = StartCoroutine(BlinkText());
        //Coroutine rotateCor = StartCoroutine(RotateImage());

        yield return StartCoroutine(FadeCanvasGroup(loadingCanvas, 0f, 1f, fadeDuration));

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncOperation.allowSceneActivation = false;
        
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }

        while(startTime < minLoadingTime)
        {
            startTime += Time.deltaTime;
            yield return null;
        }

        // 비동기씬 과정 완료되면 이미지 회전, 텍스트 깜빡임 코루틴 중지
        StopCoroutine(blinkCor);
        StopCoroutine(rotateCor);

        // 페이드 아웃 대기
        yield return StartCoroutine(FadeCanvasGroup(loadingCanvas, 1f, 0f, fadeDuration));

        yield return new WaitUntil( () => PhotonNetwork.InRoom == true);

        asyncOperation.allowSceneActivation = true;
    }

    // 페이드 인, 아웃 효과 코루틴
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        cg.alpha = start;

        if(elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;        
    }

    // 회전하는 이미지 효과 코루틴
    IEnumerator RotateImage()
    {
        while (true)
        {
            loadingImage.transform.Rotate(0, 0, -140f * Time.deltaTime);
            yield return null;
        }
    }

    // 텍스트 깜빡이는 효과 코루틴
    IEnumerator BlinkText()
    {
        Color basicColor = loadingText.color;
        while (true)
        {
            float alpha = Mathf.PingPong(Time.deltaTime * blinkSpeed, 1f);
            loadingText.color = new Color(basicColor.r, basicColor.g, basicColor.b, alpha);
            yield return null;
        }
    }

    public static void LoadSceneWithLoading(string targetScene)
    {
        sceneToLoad = targetScene;

        SceneManager.LoadScene("LoadingScene");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장하였습니다");
    }
}
