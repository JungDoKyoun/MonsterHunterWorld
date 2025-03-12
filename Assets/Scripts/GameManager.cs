using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Cinemachine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private CinemachineFreeLook _freeLockCamera;
    [SerializeField]
    private GameObject _playerPrefab;

    private GameObject _playerObject = null;

    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(SpawnPlayerWhenConnected());
        IEnumerator SpawnPlayerWhenConnected()
        {
            yield return new WaitUntil(predicate: () => PhotonNetwork.InRoom);
            if (_playerPrefab != null)
            {
                _playerObject = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
                _freeLockCamera.Set(_playerObject.transform);
            }
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
        Destroy(_playerObject);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
}