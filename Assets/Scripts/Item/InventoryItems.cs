using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//현재 가지고있을 아이템 인벤토리
public class InventoryItems : MonoBehaviour
{
    InvenType invenType = InvenType.Inven;  

    //가지고있을 아이템 목록
    List<BaseItem> items = new List<BaseItem>();

    //인벤토리 슬롯
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

        //테스트용 아이템 추가
        foreach (var item in ItemDataBase.Instance.items.ToList())
        {
            items.Add(item);
            //Debug.Log(item.name);
        }

        InvenToryCtrl.Instance.AddItemByName(items, "회복약");

        //자식 오브젝트 연결
        var objs = GetComponentsInChildren<ItemSlot>();
        foreach (var item in objs)
        {
            item.SetInvenType(invenType);
            slot.Add(item.gameObject);
        }


        //가지고있는 아이템이 있는경우
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
            }
        }
        //없으면 고대로 하면됨

    }

}
