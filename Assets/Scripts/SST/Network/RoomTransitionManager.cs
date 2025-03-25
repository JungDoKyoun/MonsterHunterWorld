using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomType
{
    SingleRoom,         // 싱글용 룸
    SingleQuestRoom,    // 싱글 퀘스트 룸       
    MeetingHouse,       // 집회소 룸
    MultiQuestRoom      // 집회소 멀티퀘스트 룸
}

public class RoomTransitionManager : MonoBehaviourPunCallbacks
{
    private static RoomTransitionManager instance;
    public static RoomTransitionManager Instance { get { Init(); return instance; } }

    // 플레이어가 다음에 입장할 룸의 타입 저장
    public RoomType nextRoom;
    // 퀘스트 룸인 경우, 생성할 방의 이름 저장
    public string questRoomName;

    private void Awake()
    {
        Init();
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("RoomTransitionManager");
            if( go == null)
            {
                go = new GameObject { name = "RoomTransitionManager" };
                go.AddComponent<RoomTransitionManager>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<RoomTransitionManager>();
        }
    }

    // 외부에서 호출. 다음에 입장할 룸을 지정, 현재 방 떠남
    public void GoToRoom(RoomType roomType, string roomName = "")
    {
        // 원하는 룸 타입 설정
        nextRoom = roomType;
        // 퀘스트 룸 이름을 설정 (해당하는 경우)
        questRoomName = roomName;
        // 현재 방을 떠나야만, 다음방 입장 가능하므로 방 떠남
        PhotonNetwork.LeaveRoom();
    }

    // 방 떠났을 때, 호출되는 함수
    public override void OnLeftRoom()
    {
        // 방 떠남 체크용. 다음 룸 타입도 확인하기 위해서 출력
        Debug.Log("방을 나갔습니다. 원하는 룸 : " +  nextRoom);

        StartCoroutine(WaitForJoinRoom());
    }

    IEnumerator WaitForJoinRoom()
    {
        while (!PhotonNetwork.InLobby)
        {
            yield return null;
        }
        Debug.Log("로비에 입장했습니다. 이제 새로운 방을 생성/입장 합니다");

        // 원하는 상태에 따라서 새 방을 생성하거나 입장함
        switch (nextRoom)
        {
            // 싱글룸 타입
            case RoomType.SingleRoom:
                JoinOrCreateRoom("SingleRoom", 1);
                break;
            // 싱글 퀘스트 타입
            case RoomType.SingleQuestRoom:
                JoinOrCreateRoom(questRoomName, 1);
                break;
            // 집회소 타입
            case RoomType.MeetingHouse:
                JoinOrCreateRoom("MeetingHouse", 16);
                break;
            // 멀티 퀘스트(집회소) 타입
            case RoomType.MultiQuestRoom:
                JoinOrCreateRoom(questRoomName, 4);
                break;
        }
    }

    // 주어진 방 이름과, 최대 플레이어 수로 JoinOrCreateRoom을 호출해
    // 방에 입장하거나, 새로 생성
    private void JoinOrCreateRoom(string roomName, int maxPlayers)
    {
        // 새로운 방 옵션 설정 : 최대 플레이어 수만 지정
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = maxPlayers };
        // 지정된 방의 이름이 있으면 입장하고, 없으면 생성
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // 새 방에 입장하면 호출되는 콜백
    public override void OnJoinedRoom()
    {
        // 확인용. 현재 방 이름도 출력
        Debug.Log("새로운 방에 입장했습니다 : " + PhotonNetwork.CurrentRoom.Name);

        // 원하는 룸 타입에 따라 씬을 전환
        switch (nextRoom)
        {
            // 룸 타입이 싱글룸
            case RoomType.SingleRoom:
                // 룸 타입이 싱글룸인데 현재 씬의 이름이 SingleRoom이 아니라면
                if (SceneManager.GetActiveScene().name != "SingleRoom")
                {
                    // SingleRoom 씬으로 전환
                    SceneManager.LoadScene("SingleRoom");
                }
                break;

            // 싱글퀘스트룸 상태일때는 씬 전환은 안함
            case RoomType.SingleQuestRoom:
                break;

            // 집회소 상태면 집회소로 씬 전환
            case RoomType.MeetingHouse:
                if (SceneManager.GetActiveScene().name != "MeetingHouse")
                {
                    SceneManager.LoadScene("MeetingHouse");
                }
                break;

            // 멀티퀘스트 상태에선 씬 전환은 안함
            case RoomType.MultiQuestRoom:
                break;
        }
    }
}
