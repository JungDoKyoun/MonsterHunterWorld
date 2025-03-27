using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteraction : MonoBehaviour
{
    [SerializeField] GameObject singleQuest;
    [SerializeField] GameObject createQuest;
    [SerializeField] GameObject JoinQuest;

    Vector3 offset = new Vector3(0, 2, 0);

    List<NpcCtrl> activeNpcs = new List<NpcCtrl>();

    private void Start()
    {
        NpcCtrl.OnNpcDetectionChanged += HandleInteractionNPC;
    }

    private void OnDestroy()
    {
        NpcCtrl.OnNpcDetectionChanged -= HandleInteractionNPC;
    }

    private void Update()
    {
        if (activeNpcs.Count > 0)
        {
            Transform playerPos = GameObject.FindGameObjectWithTag("Player").transform;
            NpcCtrl selectedNpc = null;
            float minDistance = Mathf.Infinity;

            foreach (var npc in activeNpcs)
            {
                float distance = Vector3.Distance(npc.transform.position, playerPos.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    selectedNpc = npc;
                }
            }

            if (selectedNpc != null)
            {
                if (selectedNpc.npcType == NpcCtrl.Type.SingleQuest)
                {
                    singleQuest.transform.position = playerPos.position + offset;
                    singleQuest.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            singleQuest.gameObject.SetActive(false);
        }
    }

    private void HandleInteractionNPC(NpcCtrl npc, bool isInteraction)
    {
        if (isInteraction)
        {
            if(!activeNpcs.Contains(npc))
            {
                activeNpcs.Add(npc);
            }
        }
        else
        {
            if(activeNpcs.Contains(npc))
            {
                activeNpcs.Remove(npc);
            }
        }
    }
}
