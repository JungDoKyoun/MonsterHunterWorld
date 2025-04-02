using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TrapCtrl : MonoBehaviourPun
{
    [SerializeField] GameObject end; // 활성화될 파츠

    void Start()
    {
        end.SetActive(false);
    }

    // 애니메이션 이벤트가 여기서 호출됨
    public void OnStartAnimationEnd()
    {
        end.SetActive(true);
        Debug.Log("함정 활성화됨");
        //TODO : 여기다 콜라이더 활성화 시켜줬었으면 굳이 번거롭게 안해도 됬었던거같음.
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!trapActivated && other.CompareTag("Monster"))
    //    {
    //        trapActivated = true;

    //        Debug.Log("몬스터가 함정에 걸림!");

    //        // 네트워크상 모든 클라이언트에 트랩 실행 알림
    //        photonView.RPC(nameof(StartTrapLifetime), RpcTarget.All);
    //    }
    //}

    //[PunRPC]
    //void StartTrapLifetime()
    //{
    //    Debug.Log("함정 발동 → 일정 시간 후 파괴 예정");
    //    StartCoroutine(DestroyAfterDelay());
    //}

    //IEnumerator DestroyAfterDelay()
    //{
    //    yield return new WaitForSeconds(trapLifetime);

    //    // 네트워크 상 파괴
    //    if (photonView.IsMine)
    //    {
    //        PhotonNetwork.Destroy(gameObject);
    //    }
    //}
}
