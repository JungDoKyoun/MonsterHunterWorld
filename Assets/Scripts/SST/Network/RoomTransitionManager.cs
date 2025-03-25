using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomType
{
    SingleRoom,         // �̱ۿ� ��
    SingleQuestRoom,    // �̱� ����Ʈ ��       
    MeetingHouse,       // ��ȸ�� ��
    MultiQuestRoom      // ��ȸ�� ��Ƽ����Ʈ ��
}

public class RoomTransitionManager : MonoBehaviourPunCallbacks
{
    private static RoomTransitionManager instance;
    public static RoomTransitionManager Instance { get { Init(); return instance; } }

    // �÷��̾ ������ ������ ���� Ÿ�� ����
    public RoomType nextRoom;
    // ����Ʈ ���� ���, ������ ���� �̸� ����
    public string questRoomName;

    private void Awake()
    {
        Init();
    }

    static void Init()
    {
        if(instance == null)
        {
            GameObject go = GameObject.Find("RoomTransitionManager");
            if( go == null)
            {
                go = new GameObject { name = "RoomTransitionManager" };
                go.AddComponent<RoomTransitionManager>();
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<RoomTransitionManager>();
        }
    }

    // �ܺο��� ȣ��. ������ ������ ���� ����, ���� �� ����
    public void GoToRoom(RoomType roomType, string roomName = "")
    {
        // ���ϴ� �� Ÿ�� ����
        nextRoom = roomType;
        // ����Ʈ �� �̸��� ���� (�ش��ϴ� ���)
        questRoomName = roomName;
        // ���� ���� �����߸�, ������ ���� �����ϹǷ� �� ����
        PhotonNetwork.LeaveRoom();
    }

    // �� ������ ��, ȣ��Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        // �� ���� üũ��. ���� �� Ÿ�Ե� Ȯ���ϱ� ���ؼ� ���
        Debug.Log("���� �������ϴ�. ���ϴ� �� : " +  nextRoom);

        StartCoroutine(WaitForJoinRoom());
    }

    IEnumerator WaitForJoinRoom()
    {
        while (!PhotonNetwork.InLobby)
        {
            yield return null;
        }
        Debug.Log("�κ� �����߽��ϴ�. ���� ���ο� ���� ����/���� �մϴ�");

        // ���ϴ� ���¿� ���� �� ���� �����ϰų� ������
        switch (nextRoom)
        {
            // �̱۷� Ÿ��
            case RoomType.SingleRoom:
                JoinOrCreateRoom("SingleRoom", 1);
                break;
            // �̱� ����Ʈ Ÿ��
            case RoomType.SingleQuestRoom:
                JoinOrCreateRoom(questRoomName, 1);
                break;
            // ��ȸ�� Ÿ��
            case RoomType.MeetingHouse:
                JoinOrCreateRoom("MeetingHouse", 16);
                break;
            // ��Ƽ ����Ʈ(��ȸ��) Ÿ��
            case RoomType.MultiQuestRoom:
                JoinOrCreateRoom(questRoomName, 4);
                break;
        }
    }

    // �־��� �� �̸���, �ִ� �÷��̾� ���� JoinOrCreateRoom�� ȣ����
    // �濡 �����ϰų�, ���� ����
    private void JoinOrCreateRoom(string roomName, int maxPlayers)
    {
        // ���ο� �� �ɼ� ���� : �ִ� �÷��̾� ���� ����
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = maxPlayers };
        // ������ ���� �̸��� ������ �����ϰ�, ������ ����
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // �� �濡 �����ϸ� ȣ��Ǵ� �ݹ�
    public override void OnJoinedRoom()
    {
        // Ȯ�ο�. ���� �� �̸��� ���
        Debug.Log("���ο� �濡 �����߽��ϴ� : " + PhotonNetwork.CurrentRoom.Name);

        // ���ϴ� �� Ÿ�Կ� ���� ���� ��ȯ
        switch (nextRoom)
        {
            // �� Ÿ���� �̱۷�
            case RoomType.SingleRoom:
                // �� Ÿ���� �̱۷��ε� ���� ���� �̸��� SingleRoom�� �ƴ϶��
                if (SceneManager.GetActiveScene().name != "SingleRoom")
                {
                    // SingleRoom ������ ��ȯ
                    SceneManager.LoadScene("SingleRoom");
                }
                break;

            // �̱�����Ʈ�� �����϶��� �� ��ȯ�� ����
            case RoomType.SingleQuestRoom:
                break;

            // ��ȸ�� ���¸� ��ȸ�ҷ� �� ��ȯ
            case RoomType.MeetingHouse:
                if (SceneManager.GetActiveScene().name != "MeetingHouse")
                {
                    SceneManager.LoadScene("MeetingHouse");
                }
                break;

            // ��Ƽ����Ʈ ���¿��� �� ��ȯ�� ����
            case RoomType.MultiQuestRoom:
                break;
        }
    }
}
