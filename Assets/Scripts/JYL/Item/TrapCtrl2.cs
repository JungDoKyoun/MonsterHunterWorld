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
        // �θ� ������Ʈ�� �ִ� PhotonView�� ��Ȯ�ϰ� ������
        if (transform.parent != null)
        {
            parentPhotonView = transform.parent.GetComponent<PhotonView>();

            if (parentPhotonView == null)
                Debug.LogWarning("�θ� ������Ʈ�� PhotonView�� �����ϴ�!");
        }
        else
        {
            Debug.LogWarning("�� ������Ʈ�� �θ� ������ ���� �ʽ��ϴ�.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!trapActivated && other.CompareTag("Monster"))
        {
            trapActivated = true;

            Debug.Log($"���Ͱ� ������ �ɸ�! �±�: {other.tag}");

            // ������ Ȯ�� �� �θ� ������Ʈ ���� �ڷ�ƾ ����
            if (parentPhotonView != null && PhotonNetwork.IsConnected)
            {
                Debug.Log($"�θ� PhotonView ID: {parentPhotonView.ViewID}, IsMine: {parentPhotonView.IsMine}");
                StartCoroutine(DestroyParentAfterDelay());
            }
            else
            {
                Debug.LogError("PhotonView �Ǵ� PhotonNetwork ���°� �ùٸ��� �ʾ� �ı� ����.");
            }
        }
    }

    private IEnumerator DestroyParentAfterDelay()
    {
        yield return new WaitForSeconds(trapLifetime);

        if (parentPhotonView != null && parentPhotonView.IsMine)
        {
            Debug.Log($"�θ� ������Ʈ '{parentPhotonView.gameObject.name}' ���� ����");
            PhotonNetwork.Destroy(parentPhotonView.gameObject); // �ڽ� ���� ���� ������
        }
        else
        {
            Debug.LogWarning("�θ� PhotonView �������� ��� ������ �� �����ϴ�.");
        }
    }
}
