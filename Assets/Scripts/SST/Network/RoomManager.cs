using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [Header("방 UI 관련")]
    [SerializeField] private Canvas roomInfoCanvas;          // 방 내부에서 보여지는 UI 캔버스
    [SerializeField] private RectTransform playListPanel;      // 플레이어 리스트가 표시될 패널
    [SerializeField] private GameObject playerListPrefab;      // 플레이어 리스트 아이템 프리팹
    [SerializeField] private GameObject hostPlayerPrefab;      // 호스트 리스트 아이템 프리펩

    [Header("방 버튼 : 스타트, 레디")]
    [SerializeField] private Button questStartButton;          // 방장이 게임 시작할 때 사용하는 버튼
    [SerializeField] private Button questReadyButton;          // 일반 플레이어가 준비 상태를 설정할 때 사용하는 버튼

    private void Start()
    {
        // 초기에 방 내부 UI는 비활성화
        roomInfoCanvas.gameObject.SetActive(false);
        questStartButton.gameObject.SetActive(false);
        questReadyButton.gameObject.SetActive(false);
    }

    #region Photon Callbacks for Room

    // 방에 입장했을 때 호출되는 콜백
    public override void OnJoinedRoom()
    {
        Debug.Log("방에 입장했습니다");
        roomInfoCanvas.gameObject.SetActive(true);

        // 방장이면 게임 시작 버튼, 일반 플레이어면 게임 레디 버튼 활성화
        if (PhotonNetwork.IsMasterClient)
        {
            questStartButton.gameObject.SetActive(true);
            questReadyButton.gameObject.SetActive(false);
        }
        else
        {
            questReadyButton.gameObject.SetActive(true);
            questStartButton.gameObject.SetActive(false);
        }

        UpdatePlayerListUI();
    }

    // 새로운 플레이어가 방에 입장하면 플레이어 리스트를 갱신
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " 님이 입장했습니다");
        UpdatePlayerListUI();
    }

    // 플레이어가 방을 나가면 플레이어 리스트를 갱신
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " 님이 나갔습니다");
        UpdatePlayerListUI();
    }

    // 플레이어의 Custom Properties(예: "isReady")가 변경되면 호출됨
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("isReady"))
        {
            UpdatePlayerListUI();
        }
    }

    // 방 나가기 요청 (로비로 돌아갈 경우)
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        roomInfoCanvas.gameObject.SetActive(false);
    }

    #endregion

    #region UI Update and Room Control

    // 플레이어 리스트 UI를 새로 고치는 함수
    private void UpdatePlayerListUI()
    {
        // 기존 리스트 항목 모두 제거
        foreach(Transform child in playListPanel)
        {
            Destroy(child.gameObject);
        }

        // 현재 방에 있는 모든 플레이어 정보 기반으로 리스트 생성
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // 방에 들어온 플레이어에 따라 플레이어리스트 프리팹을 플레이어 리스트 패널에 인스턴스화
            // 그 프리팹 텍스트를 해당 플레이어 닉네임으로 바꿔준다.

            // 각 플레이어가 마스터 클라이언트인지를 확인
            if (player.ActorNumber == PhotonNetwork.MasterClient.ActorNumber)
            {
                GameObject hostList = Instantiate(hostPlayerPrefab, playListPanel);
                hostList.GetComponentInChildren<Text>().text = player.NickName;
            }
            else
            {
                GameObject playerList = Instantiate(playerListPrefab, playListPanel);
                playerList.GetComponentInChildren<Text>().text = player.NickName;

                // 플레이어가 준비 상태라면 이미지 색상을 초록색으로 표시
                if (player.CustomProperties.ContainsKey("isReady") && (bool)player.CustomProperties["isReady"])
                {
                    Image readyImg = playerList.transform.Find("CheckReady").GetComponent<Image>();
                    readyImg.color = Color.green;
                    //playerList.GetComponentInChildren<Image>().color = Color.green;
                }
                else
                {
                    Image readyImg = playerList.transform.Find("CheckReady").GetComponent<Image>();
                    readyImg.color = Color.white;
                    //playerList.GetComponentInChildren<Image>().color = Color.white;
                }
            }
        }

    }

    // 일반 플레이어가 준비 버튼을 누르면 호출되는 함수
    public void OnReady()
    {
        // Photon Custom Properties를 통해 로컬 플레이어의 준비 상태를 true로 설정
        ExitGames.Client.Photon.Hashtable readyProps = new ExitGames.Client.Photon.Hashtable();
        readyProps["isReady"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(readyProps);
    }

    // 방장이 게임 시작 버튼을 누르면 호출되는 함수
    public void InGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 모든 플레이어와 함께 InGame 씬으로 전환
            PhotonNetwork.LoadLevel("InGame");
        }
    }

    #endregion
}

