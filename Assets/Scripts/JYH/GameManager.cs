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

    private static readonly float RespawnTime = 5.0f;
    private static readonly Vector3 PlayerStartPoint = new Vector3(-260, 41.5f, -43);
    private static readonly Vector3 MonsterStartPoint = new Vector3(-260, 41.5f, -32);

    private void Start()
    {
        Room room = PhotonNetwork.CurrentRoom;
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
                    yield return new WaitWhile(() => FindObjectsOfType<PlayerController>().Length < PhotonNetwork.CurrentRoom.PlayerCount);
                    PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
                    for(int i = 0; i < playerControllers.Length; i++)
                    {
                        Player player = playerControllers[i].photonView.Owner;
                        if (player != PhotonNetwork.LocalPlayer)
                        {
                            int index = i;
                            _gageController?.SetName(player.NickName, index);
                            playerControllers[i].Initialize((current, full) => _gageController?.SetLife(current, full, index));
                            _otherPlayers.Add(playerControllers[i]);
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        if(_playerController != null)
        {
            _gageController.SetStamina(_playerController.currentStamina, _playerController.fullStamina);
        }
    }

    private void SetLife(int current, int full)
    {
        _gageController?.SetLife(current, full);
        if (current != full && current == 0)
        {
            StartCoroutine(DoRevive());
            IEnumerator DoRevive()
            {
                yield return new WaitForSeconds(RespawnTime);
                if(_playerController != null)
                {
                    _playerController.Revive(PlayerStartPoint);
                }
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(WaitNewPlayerController());
        IEnumerator WaitNewPlayerController()
        {
            yield return new WaitWhile(() => FindObjectsOfType<PlayerController>().Length < PhotonNetwork.CurrentRoom.PlayerCount);
            PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
            for (int i = 0; i < playerControllers.Length; i++)
            {
                if(_otherPlayers.Contains(playerControllers[i]) == false)
                {
                    int index = i;
                    _gageController?.SetName(newPlayer.NickName, index);
                    playerControllers[i].Initialize((current, full) => _gageController?.SetLife(current, full, index));
                    _otherPlayers.Add(playerControllers[i]);
                }
            }
        }
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
}