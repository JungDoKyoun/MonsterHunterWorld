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

    [Header("�ε� ȿ��")]
    [SerializeField] CanvasGroup loadingCanvas;         // ���̵���,�ƿ� ȭ��
    [SerializeField] Image loadingImage;                // ȸ���� �ε� �̹���
    [SerializeField] Text loadingText;                  // �����̴� �ε� �ؽ�Ʈ

    private float fadeDuration = 1f;         // ���̵� ȿ�� ���ӽð�
    private float blinkSpeed = 10f;          // �ؽ�Ʈ ������ �ӵ�

    Coroutine blinkCor;
    Coroutine rotateCor;

    RoomOptions single = new RoomOptions { MaxPlayers = 1 };

    private void Start()
    {
        // ���۰� ���ÿ� �̹��� ȸ��, �ؽ�Ʈ ������ ȿ�� �ڷ�ƾ ����
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

        // ���۰� ���ÿ� �̹��� ȸ��, �ؽ�Ʈ ������ ȿ�� �ڷ�ƾ ����
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

        // �񵿱�� ���� �Ϸ�Ǹ� �̹��� ȸ��, �ؽ�Ʈ ������ �ڷ�ƾ ����
        StopCoroutine(blinkCor);
        StopCoroutine(rotateCor);

        // ���̵� �ƿ� ���
        yield return StartCoroutine(FadeCanvasGroup(loadingCanvas, 1f, 0f, fadeDuration));

        yield return new WaitUntil( () => PhotonNetwork.InRoom == true);

        asyncOperation.allowSceneActivation = true;
    }

    // ���̵� ��, �ƿ� ȿ�� �ڷ�ƾ
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

    // ȸ���ϴ� �̹��� ȿ�� �ڷ�ƾ
    IEnumerator RotateImage()
    {
        while (true)
        {
            loadingImage.transform.Rotate(0, 0, -140f * Time.deltaTime);
            yield return null;
        }
    }

    // �ؽ�Ʈ �����̴� ȿ�� �ڷ�ƾ
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
        Debug.Log("�濡 �����Ͽ����ϴ�");
    }
}
