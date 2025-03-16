using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _playerPrefab;

    private void Start()
    {
        Room room = PhotonNetwork.CurrentRoom;
        if (room != null && _playerPrefab != null)
        {
            GameObject gameObject = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
            FindObjectOfType<CinemachineFreeLook>().Set(gameObject.transform);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
    }

    public override void OnPlayerLeftRoom(Player player)
    {
    }
}