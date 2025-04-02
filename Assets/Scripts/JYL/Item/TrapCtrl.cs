using Photon.Pun;
using System.Collections;
using UnityEngine;

public class TrapCtrl : MonoBehaviourPun
{
    [SerializeField] GameObject end; // Ȱ��ȭ�� ����
    public float trapLifetime = 5f;

    private bool trapActivated = false;

    void Start()
    {
        end.SetActive(false);
    }

    // �ִϸ��̼� �̺�Ʈ�� ���⼭ ȣ���
    public void OnStartAnimationEnd()
    {
        end.SetActive(true);
        Debug.Log("���� Ȱ��ȭ��");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!trapActivated && other.CompareTag("Monster"))
        {
            trapActivated = true;

            Debug.Log("���Ͱ� ������ �ɸ�!");

            // ��Ʈ��ũ�� ��� Ŭ���̾�Ʈ�� Ʈ�� ���� �˸�
            photonView.RPC(nameof(StartTrapLifetime), RpcTarget.All);
        }
    }

    [PunRPC]
    void StartTrapLifetime()
    {
        Debug.Log("���� �ߵ� �� ���� �ð� �� �ı� ����");
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(trapLifetime);

        // ��Ʈ��ũ �� �ı�
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
