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
            GameObject gameObject = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(-173, 86.05f, 132.289f), Quaternion.identity, 0);
            FindObjectOfType<CinemachineFreeLook>().Set(gameObject.transform);
        }
    }

    private void OnGUI()
    {
        
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