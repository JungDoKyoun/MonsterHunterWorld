using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using Firebase.Database;

public class LoadingSceneManager : MonoBehaviourPunCallbacks
{
    public static string sceneToLoad;
    static RoomOptions selectRoomOption;
    private int singleRoomIndex = 1;
    private int meetingRoomIndex = 1;

    [Header("�ε� ȿ��")]
    [SerializeField] CanvasGroup loadingCanvas;         // ���̵���,�ƿ� ȭ��
    [SerializeField] Image loadingImage;                // ȸ���� �ε� �̹���
    [SerializeField] Text loadingText;                  // �����̴� �ε� �ؽ�Ʈ

    private float fadeDuration = 1f;         // ���̵� ȿ�� ���ӽð�
    private float blinkSpeed = 10f;          // �ؽ�Ʈ ������ �ӵ�

    Coroutine blinkCor;
    Coroutine rotateCor;


    private void Start()
    {
        loadingCanvas.gameObject.SetActive(false);
        SoundManager.Instance.StopBGM();
        // ���۰� ���ÿ� �̹��� ȸ��, �ؽ�Ʈ ������ ȿ�� �ڷ�ƾ ����
        blinkCor = StartCoroutine(BlinkText());
        rotateCor = StartCoroutine(RotateImage());

        if(selectRoomOption.MaxPlayers == 1)
        {
            StartCoroutine(PlayLoadScene());
            CreateSingleRoom(sceneToLoad, selectRoomOption);
        }
        else
        {
            CreateMeetingRoom(sceneToLoad, selectRoomOption);
        }
    }

    public void CreateSingleRoom(string toWhere, RoomOptions roomOptions)
    {
        PhotonNetwork.CreateRoom(toWhere, roomOptions);
    }

    public void CreateMeetingRoom(string toWhere, RoomOptions roomOptions)
    {
        PhotonNetwork.JoinOrCreateRoom(toWhere, roomOptions, TypedLobby.Default);
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

        yield return new WaitUntil( () => PhotonNetwork.CurrentRoom != null);

        // �񵿱�� ���� �Ϸ�Ǹ� �̹��� ȸ��, �ؽ�Ʈ ������ �ڷ�ƾ ����
        StopCoroutine(blinkCor);
        StopCoroutine(rotateCor);

        // ���̵� �ƿ� ���
        yield return StartCoroutine(FadeCanvasGroup(loadingCanvas, 1f, 0f, fadeDuration));


        asyncOperation.allowSceneActivation = true;
    }

    // ���̵� ��, �ƿ� ȿ�� �ڷ�ƾ
    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        cg.alpha = start;
        cg.gameObject.SetActive(true);
        
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

    public static void LoadSceneWithLoading(string targetScene, RoomOptions roomOptions)
    {
        sceneToLoad = targetScene;
        selectRoomOption = roomOptions;

        SceneManager.LoadScene("LoadingScene");
    }

    public override void OnJoinedRoom()
    {
#if UNITY_EDITOR
        Debug.Log("�濡 �����Ͽ����ϴ�");
        Debug.Log(sceneToLoad + selectRoomOption.MaxPlayers);
#endif
        if (selectRoomOption.MaxPlayers != 1)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(sceneToLoad);
        }
    }

    // ���࿡ ���� 16�� ������ ���� ���� ��� ����ó���� ���� ���� �ݹ� �Լ�
    // ���� �濡 ���°� �����ϸ� ���� ���� ����� index�� �������Ѽ�
    // MeetingHouse1 �濡 ������ index++ -> MeetingHouse2 �� ���� ������ �����
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("��ȸ�� �� ���忡 �����߽��ϴ�. ���ο� ��ȸ�� ���� �����մϴ�");
        meetingRoomIndex++;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 16 };
        PhotonNetwork.JoinOrCreateRoom("MeetingHouse" + meetingRoomIndex, roomOptions, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("�̱۷� ������ �����߽��ϴ�. ���ο� �̱۷��� �����մϴ�");
        singleRoomIndex++;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 1 };
        PhotonNetwork.CreateRoom("SingleRoom" + singleRoomIndex, roomOptions, TypedLobby.Default);
    }
}
