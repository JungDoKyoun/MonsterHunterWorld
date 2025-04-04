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

    [Header("로딩 효과")]
    [SerializeField] CanvasGroup loadingCanvas;         // 페이드인,아웃 화면
    [SerializeField] Image loadingImage;                // 회전할 로딩 이미지
    [SerializeField] Text loadingText;                  // 깜빡이는 로딩 텍스트

    private float fadeDuration = 1f;         // 페이드 효과 지속시간
    private float blinkSpeed = 10f;          // 텍스트 깜빡임 속도

    Coroutine blinkCor;
    Coroutine rotateCor;


    private void Start()
    {
        loadingCanvas.gameObject.SetActive(false);
        SoundManager.Instance.StopBGM();
        // 시작과 동시에 이미지 회전, 텍스트 깜빡임 효과 코루틴 실행
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

        yield return new WaitUntil( () => PhotonNetwork.CurrentRoom != null);

        // 비동기씬 과정 완료되면 이미지 회전, 텍스트 깜빡임 코루틴 중지
        StopCoroutine(blinkCor);
        StopCoroutine(rotateCor);

        // 페이드 아웃 대기
        yield return StartCoroutine(FadeCanvasGroup(loadingCanvas, 1f, 0f, fadeDuration));


        asyncOperation.allowSceneActivation = true;
    }

    // 페이드 인, 아웃 효과 코루틴
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

    public static void LoadSceneWithLoading(string targetScene, RoomOptions roomOptions)
    {
        sceneToLoad = targetScene;
        selectRoomOption = roomOptions;

        SceneManager.LoadScene("LoadingScene");
    }

    public override void OnJoinedRoom()
    {
#if UNITY_EDITOR
        Debug.Log("방에 입장하였습니다");
        Debug.Log(sceneToLoad + selectRoomOption.MaxPlayers);
#endif
        if (selectRoomOption.MaxPlayers != 1)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LoadLevel(sceneToLoad);
        }
    }

    // 만약에 위의 16명 설정한 방이 꽉찰 경우 예외처리를 위해 만든 콜백 함수
    // 위의 방에 들어가는게 실패하면 새로 방을 만든다 index를 증가시켜서
    // MeetingHouse1 방에 못들어가면 index++ -> MeetingHouse2 방 참가 없으면 만들기
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("집회소 룸 입장에 실패했습니다. 새로운 집회소 룸을 생성합니다");
        meetingRoomIndex++;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 16 };
        PhotonNetwork.JoinOrCreateRoom("MeetingHouse" + meetingRoomIndex, roomOptions, TypedLobby.Default);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("싱글룸 생성에 실패했습니다. 새로운 싱글룸을 생성합니다");
        singleRoomIndex++;
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 1 };
        PhotonNetwork.CreateRoom("SingleRoom" + singleRoomIndex, roomOptions, TypedLobby.Default);
    }
}
