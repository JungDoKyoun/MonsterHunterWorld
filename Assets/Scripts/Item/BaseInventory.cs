using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInventory : MonoBehaviour
{
    // 인벤토리 타입
    protected InvenType invenType;

    // 빈 아이템 
    protected BaseItem emptyItem;
    
    // 아이템 목록
    //List<BaseItem> items { get; set; }
    protected List<BaseItem> items = new List<BaseItem>();
    public List<BaseItem> Items
    {
        get => items;
        set => items = value;
    }
   
    //인벤토리 내 슬롯
    protected List<GameObject> slot = new List<GameObject>();

    protected bool isInvenOpen = false;
    public bool IsInvenOpen { get => isInvenOpen; set => isInvenOpen = value; }

    //슬롯 갯수
    protected int invenMaxSize;

    private void Start()
    {
        emptyItem = ItemDataBase.Instance.emptyItem;
    }

    public void SlotSetting(InvenType type)
    {
        //자식 오브젝트 연결
        var objs = GetComponentsInChildren<ItemSlot>();
        foreach (var o in objs)
        {
            o.SetInvenType(invenType);
            slot.Add(o.gameObject);
        }
    }

    public void InvenOpen()
    {
     

        isInvenOpen = true;
        gameObject.SetActive(true);


    }

    public void InvenClose()
    {
        isInvenOpen = false;
        gameObject.SetActive(false);
    }
}
