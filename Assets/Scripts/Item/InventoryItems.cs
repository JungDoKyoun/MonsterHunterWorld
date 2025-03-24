using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//���� ���������� ������ �κ��丮
public class InventoryItems : MonoBehaviour
{
    InvenType invenType = InvenType.Inven;  

    //���������� ������ ���
    List<BaseItem> items = new List<BaseItem>();

    //�κ��丮 ����
    List<GameObject> slot = new List<GameObject>();

    public List<BaseItem> Items 
    {
        get => items;
        set => items = value;
    }

    bool isInvenOpen = false;
    public bool IsInvenOpen { get => isInvenOpen; set => isInvenOpen = value; }


    void Start()
    {

        //�׽�Ʈ�� ������ �߰�
        foreach (var item in ItemDataBase.Instance.items.ToList())
        {
            items.Add(item);
            //Debug.Log(item.name);
        }

        InvenToryCtrl.Instance.AddItemByName(items, "ȸ����");

        //�ڽ� ������Ʈ ����
        var objs = GetComponentsInChildren<ItemSlot>();
        foreach (var item in objs)
        {
            item.SetInvenType(invenType);
            slot.Add(item.gameObject);
        }


        //�������ִ� �������� �ִ°��
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
            }
        }
        //������ ���� �ϸ��

    }

}
