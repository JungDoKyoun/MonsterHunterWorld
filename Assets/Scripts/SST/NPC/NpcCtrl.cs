using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NpcCtrl : MonoBehaviour
{
    // NPC Ÿ�Կ� ���� ���� �и�
    public enum Type
    {
        SingleQuest, // �̱� ����Ʈ ����
        Create, // ����Ʈ(��) ����
        Join // ����Ʈ(��) ����
    }

    public Type npcType;
    public bool isPlayerInRange = false;

    // NPC ���� ���� ��ȭ �̺�Ʈ
    // 1. �Ű����� : ���°� ��ȭ�� NPC, 2. ���� ����( true : ���� , false : ����)
    public static event Action<NpcCtrl, bool> OnNpcDetectionChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // ���� ���� ��ȭ �̺�Ʈ�� �߻����� ������(�� : LobbyManager)���� �˸�
            OnNpcDetectionChanged?.Invoke(this, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            OnNpcDetectionChanged?.Invoke(this, false);
        }
    }
}
