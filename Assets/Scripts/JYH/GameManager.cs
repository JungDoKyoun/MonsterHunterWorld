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
        PlayerController.CreateAction += AddPlayer;
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
                    //�濡 ���Ա� ������, ������ ���� �ٲپ�ξ����Ƿ� �����ų������ �ణ�� ������ ����
                    yield return new WaitUntil(() => PhotonNetwork.InRoom); //��� �ٽ� �η� ���±��� ���
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