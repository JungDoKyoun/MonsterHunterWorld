using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NpcCtrl : MonoBehaviour
{
    // NPC 타입에 따른 역할 분리
    public enum Type
    {
        SingleQuest, // 싱글 퀘스트 생성
        Create, // 퀘스트(룸) 생성
        Join // 퀘스트(룸) 입장
    }

    public Type npcType;
    public bool isPlayerInRange = false;

    // NPC 감지 상태 변화 이벤트
    // 1. 매개변수 : 상태가 변화한 NPC, 2. 감지 상태( true : 들어옴 , false : 나감)
    public static event Action<NpcCtrl, bool> OnNpcDetectionChanged;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            // 감지 상태 변화 이벤트를 발생시켜 구독자(예 : LobbyManager)에게 알림
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
