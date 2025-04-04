using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TrapCtrl : MonoBehaviourPun
{
    [SerializeField] GameObject end; // Ȱ��ȭ�� ����

    void Start()
    {
        end.SetActive(false);
    }

    // �ִϸ��̼� �̺�Ʈ�� ���⼭ ȣ���
    public void OnStartAnimationEnd()
    {
       var obj = PhotonNetwork.Instantiate("Effects/Trap/Explosion", transform.position, Quaternion.identity);

        end.SetActive(true);
        Debug.Log("���� Ȱ��ȭ��");
        
        StartCoroutine(EffectDestroy(obj, 1f));
        //TODO : ����� �ݶ��̴� Ȱ��ȭ ����������� ���� ���ŷӰ� ���ص� ������Ű���.
    }

    IEnumerator EffectDestroy(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        PhotonNetwork.Destroy(obj);
    }



    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!trapActivated && other.CompareTag("Monster"))
    //    {
    //        trapActivated = true;

    //        Debug.Log("���Ͱ� ������ �ɸ�!");

    //        // ��Ʈ��ũ�� ��� Ŭ���̾�Ʈ�� Ʈ�� ���� �˸�
    //        photonView.RPC(nameof(StartTrapLifetime), RpcTarget.All);
    //    }
    //}

    //[PunRPC]
    //void StartTrapLifetime()
    //{
    //    Debug.Log("���� �ߵ� �� ���� �ð� �� �ı� ����");
    //    StartCoroutine(DestroyAfterDelay());
    //}

    //IEnumerator DestroyAfterDelay()
    //{
    //    yield return new WaitForSeconds(trapLifetime);

    //    // ��Ʈ��ũ �� �ı�
    //    if (photonView.IsMine)
    //    {
    //        PhotonNetwork.Destroy(gameObject);
    //    }
    //}
}
