using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TrapCtrl2 : MonoBehaviourPun
{
    public float trapLifetime = 5f;

    private bool trapActivated = false;
    private PhotonView parentPhotonView;

    private void Awake()
    {
        // 부모 오브젝트에 있는 PhotonView를 명확하게 가져옴
        if (transform.parent != null)
        {
            parentPhotonView = transform.parent.GetComponent<PhotonView>();

            if (parentPhotonView == null)
                Debug.LogWarning("부모 오브젝트에 PhotonView가 없습니다!");
        }
        else
        {
            Debug.LogWarning("이 오브젝트는 부모를 가지고 있지 않습니다.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!trapActivated && other.CompareTag("Monster"))
        {
            trapActivated = true;

            Debug.Log($"몬스터가 함정에 걸림! 태그: {other.tag}");

            // 소유권 확인 후 부모 오브젝트 삭제 코루틴 시작
            if (parentPhotonView != null && PhotonNetwork.IsConnected)
            {
                Debug.Log($"부모 PhotonView ID: {parentPhotonView.ViewID}, IsMine: {parentPhotonView.IsMine}");
                StartCoroutine(DestroyParentAfterDelay());
            }
            else
            {
                Debug.LogError("PhotonView 또는 PhotonNetwork 상태가 올바르지 않아 파괴 실패.");
            }
        }
    }

    private IEnumerator DestroyParentAfterDelay()
    {
        yield return new WaitForSeconds(trapLifetime);

        if (parentPhotonView != null && parentPhotonView.IsMine)
        {
            Debug.Log($"부모 오브젝트 '{parentPhotonView.gameObject.name}' 삭제 시작");
            PhotonNetwork.Destroy(parentPhotonView.gameObject); // 자식 포함 전부 삭제됨
        }
        else
        {
            Debug.LogWarning("부모 PhotonView 소유권이 없어서 삭제할 수 없습니다.");
        }
    }
}
