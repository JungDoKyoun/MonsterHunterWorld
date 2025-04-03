using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _monsterPrefab;
    [SerializeField]
    private GageCtrl _gageController;

    private PlayerController _playerController = null;

    private List<PlayerController> _otherPlayers = new List<PlayerController>();

    private const string DeadCountTag = "DeadCount";
    private const string VictoryTag = "Victory";
    private static readonly float RespawnTime = 5.0f;
    private static readonly Vector3 PlayerStartPoint = new Vector3(-260, 41.5f, -43);
    private static readonly Vector3 MonsterStartPoint = new Vector3(-260, 41.5f, -32);


    private void Start()
    {
        Room room = PhotonNetwork.CurrentRoom;
        PlayerController.CreateAction += AddPlayer;
        PlayerController.MonsterAction += ReportMonseterState;
        if (room != null)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGMType.Boss);
            _gageController?.SetName(PhotonNetwork.NickName);
            if (PhotonNetwork.IsMasterClient == true && _monsterPrefab != null)
            {
                PhotonNetwork.InstantiateRoomObject(_monsterPrefab.name, MonsterStartPoint, Quaternion.identity, 0);
            }
            if (PlayerManager.LocalPlayerInstance == null)
            {
                StartCoroutine(SpawnPlayerWhenConnected());
                IEnumerator SpawnPlayerWhenConnected()
                {
                    //방에 들어왔긴 하지만, 방장이 씬을 바꾸어두었으므로 적용시킬때까지 약간의 딜레이 존재
                    yield return new WaitUntil(() => PhotonNetwork.InRoom); //고로 다시 인룸 상태까지 대기
                    if (_playerPrefab != null)
                    {
                        GameObject gameObject = PhotonNetwork.Instantiate(_playerPrefab.name, PlayerStartPoint, Quaternion.identity, 0);
                        FindObjectOfType<CinemachineFreeLook>().Set(gameObject.transform);
                        _playerController = gameObject.GetComponent<PlayerController>();
                        if (_playerController != null)
                        {
                            _playerController.attackable = true;
                            _playerController.Initialize(SetLife);
                        }
                    }
                }
            }
        }
    }

    private void AddPlayer(PlayerController playerController)
    {
        if(_otherPlayers.Contains(playerController) == false && playerController.photonView.Owner != PhotonNetwork.LocalPlayer)
        {
            int index = _otherPlayers.Count;
            _gageController?.SetName(playerController.photonView.Owner.NickName, index);
            playerController.Initialize((current, full) => _gageController?.SetLife(current, full, index));
            _otherPlayers.Add(playerController);
        }
    }

    private bool _victory = false;

    [SerializeField]
    private GameObject questWinPanel;

    private void ReportMonseterState(MonsterController monsterController)
    {
        if(monsterController.IsDie == true && _victory == false)
        {
            PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { VictoryTag, true } });
        }
    }

    private void Update()
    {
        if(_playerController != null)
        {
            _gageController.SetStamina(_playerController.currentStamina, _playerController.fullStamina);
        }
    }

    private void OnDestroy()
    {
        PlayerController.CreateAction -= AddPlayer;
        PlayerController.MonsterAction -= ReportMonseterState;
    }

    private void SetLife(int current, int full)
    {
        _gageController?.SetLife(current, full);
        if (current != full && current == 0)
        {
            ExitGames.Client.Photon.Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;
            if (hashtable.ContainsKey(DeadCountTag) == false)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { {DeadCountTag, false} });
                StartCoroutine(DoRevive());
                IEnumerator DoRevive()
                {
                    yield return new WaitForSeconds(RespawnTime);
                    if (_playerController != null)
                    {
                        _playerController.Revive(PlayerStartPoint);
                    }
                }
            }
            else
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { DeadCountTag, true } });
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for (int i = 0; i < _otherPlayers.Count; i++)
        {
            if (_otherPlayers[i] == null)
            {
                _gageController?.HidePlayer(i);
            }
        }
    }

    [SerializeField]
    private GameObject questFailPanel;

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable hashtable)
    {
        foreach(string key in hashtable.Keys)
        {
            switch (key)
            {
                case DeadCountTag:
                    if (hashtable[key] != null && bool.TryParse(hashtable[key].ToString(), out bool dead) && dead == true)
                    {
                        StartCoroutine(DoWaitGameOver());
                        IEnumerator DoWaitGameOver()
                        {
                            SoundManager.Instance.PlayBGM(SoundManager.BGMType.QuestFailed);
                            _playerController.enabled = false;
                            questFailPanel.Set(true);
                            yield return new WaitForSeconds(11);
                            PhotonNetwork.LeaveRoom();
                        }
                    }
                    break;
                case VictoryTag:
                    if (_victory == false)
                    {
                        _victory = true;
                        StartCoroutine(DoStartWin());
                        IEnumerator DoStartWin()
                        {
                            SoundManager.Instance.PlayBGM(SoundManager.BGMType.QuestCompleted);
                            _playerController.enabled = false;
                            questWinPanel.Set(true);
                            yield return new WaitForSeconds(10);
                            PhotonNetwork.LeaveRoom();
                        }
                    }
                    break;
            }
        }
    }

    public override void OnLeftRoom()
    {
        LoadingSceneManager.LoadSceneWithLoading("SingleRoom", new RoomOptions { MaxPlayers = 1 });
    }
}