using System.Collections;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab;
    [SerializeField]
    private GameObject _monsterPrefab;

    private PlayerController _playerController = null;

    private static readonly Vector3 PlayerStartPoint = new Vector3(-260, 41.5f, -43);
    private static readonly Vector3 MonsterStartPoint = new Vector3(-260, 41.5f, -32);

    private void Start()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null && _playerPrefab != null)
        {
            GameObject gameObject = PhotonNetwork.Instantiate(_playerPrefab.name, PlayerStartPoint, Quaternion.identity, 0);
            FindObjectOfType<CinemachineFreeLook>().Set(gameObject.transform);
            _playerController = gameObject.GetComponent<PlayerController>();
            if (_playerController != null)
            {
                _playerController.attackable = true;
                _playerController.Initialize(SetLife);
            }
            if(PhotonNetwork.IsMasterClient == true && _monsterPrefab != null)
            {
                PhotonNetwork.InstantiateRoomObject(_monsterPrefab.name, MonsterStartPoint, Quaternion.identity, 0);
            }
        }
    }

    private void SetLife(int current, int full)
    {
        if(current != full && current == 0)
        {
            StartCoroutine(DoRevive());
            IEnumerator DoRevive()
            {
                Debug.Log("ªÁ∏¡");
                yield return new WaitForSeconds(2f);
                if(_playerController != null)
                {
                    Debug.Log("∫Œ»∞");
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
}