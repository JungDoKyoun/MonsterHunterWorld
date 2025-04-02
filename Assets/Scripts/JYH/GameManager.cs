using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _monsterPrefab;
    [SerializeField]
    private GageCtrl _gageController;

    private PlayerController _playerController = null;

    private Dictionary<PlayerController, string> otherPlayers = new Dictionary<PlayerController, string>();

    private static readonly float RespawnTime = 5.0f;
    private static readonly Vector3 PlayerStartPoint = new Vector3(-260, 41.5f, -43);
    private static readonly Vector3 MonsterStartPoint = new Vector3(-260, 41.5f, -32);

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayerController.playerAction += CreatePlayer;
    }

    private void Start()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGMType.Boss);
            _gageController?.Set(PhotonNetwork.NickName);
            if(PhotonNetwork.IsMasterClient == true && _monsterPrefab != null)
            {
                PhotonNetwork.InstantiateRoomObject(_monsterPrefab.name, MonsterStartPoint, Quaternion.identity, 0);
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

    private void OnDestroy()
    {
        PlayerController.playerAction -= CreatePlayer;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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

    private void CreatePlayer(PlayerController playerController, string nickname)
    {
        Debug.Log("신규 생성");
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
        PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();
        Debug.Log(playerControllers.Length);
    }
}