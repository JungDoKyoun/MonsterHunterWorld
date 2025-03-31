using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseInventory : MonoBehaviour
{
    // 인벤토리 타입
    protected InvenType invenType;

    // 빈 아이템 
    protected BaseItem emptyItem;

    // 아이템 목록    
    protected List<BaseItem> items = new List<BaseItem>();
    public List<BaseItem> Items
    {
        get => items;
        set => items = value;
    }

    //슬롯 오브젝트의 부모
    public GameObject parrentObj;

    //인벤토리 내 슬롯
    protected List<GameObject> slot = new List<GameObject>();
    public List<GameObject> Slot { get => slot; }

    protected bool isInvenOpen = false;
    public bool IsInvenOpen { get => isInvenOpen; set => isInvenOpen = value; }

    //슬롯 갯수
    protected int invenMaxSize;

    private void Start()
    {
        emptyItem = ItemDataBase.Instance.emptyItem;

    }

    //protected void InvenInit(List<BaseItem> list)
    //{
    //    // 슬롯 수만큼 아이템 리스트에 emptyItem 채워넣기
    //    for (int i = 0; i < slot.Count; i++)
    //    {
    //        list.Add(ItemDataBase.Instance.emptyItem);
    //    }
    //}

   

    //슬롯 세팅
    public void SlotSetting(GameObject current, InvenType type)
    {
        //부모 오브젝트 연결
        parrentObj = current;            

        //자식 오브젝트 연결
        var objs = parrentObj.GetComponentsInChildren<ItemSlot>();
        foreach (var o in objs)
        {
            o.SetInvenType(invenType);
            slot.Add(o.gameObject);
        }
    }

    ////아이템 추가
    //public void AddItem(List<BaseItem> currentItems, BaseItem changeItems)
    //{
    //    int index = currentItems.FindIndex(i => i == emptyItem);
    //    if (index >= 0)
    //    {
    //        Debug.Log(changeItems.name);
    //        currentItems[index] = changeItems;
    //    }
    //    else
    //    {
    //        currentItems.Add(changeItems);
    //    }
    //}

    //아이템 삭제
    public void RemoveItem(List<BaseItem> currentItems, BaseItem changeItems)
    {
        int index = currentItems.FindIndex(i => i == changeItems);
        if (index >= 0)
        {
            Debug.Log($"[RemoveItem] {changeItems.name} 제거됨 -> emptyItem으로 교체");
            currentItems[index] = emptyItem;
        }
    }



    //아이템 슬롯 갱신
    public virtual void RefreshUI(List<BaseItem> list)
    {
        //Debug.Log("[RefreshUI] 호출됨");

        for (int i = 0; i < slot.Count; i++)
        {
            var slotComp = slot[i].GetComponent<ItemSlot>();
            if (slotComp == null)
            {
                Debug.LogWarning($"슬롯 {i}번에 ItemSlot 컴포넌트가 없습니다.");
                continue;
            }

            // item[i]가 null이면 emptyItem으로 대체
            BaseItem currentItem =
                (i < list.Count && list[i] != null) ? list[i] : ItemDataBase.Instance.emptyItem;
            //Debug.Log($"슬롯 {i}: {items[i].name}, 타입: {items[i].type}, count: {items[i].count}");
            slotComp.SlotSetItem(currentItem);
        }
    }

    //아이템 타입별로 정렬
    public void SortItemsByType(List<BaseItem> list)
    {
        // 1. 유효한 아이템만 추림
        var validItems = list.Where(i => i.type != ItemType.Empty).ToList();

        // 2. 타입 기준으로 정렬
        validItems = validItems.OrderBy(i => i.type).ToList();

        // 3. 빈 슬롯 수 계산
        int emptyCount = list.Count - validItems.Count;

        // 4. 리스트 재구성
        list = new List<BaseItem>(validItems);
        for (int i = 0; i < emptyCount; i++)
        {
            list.Add(ItemDataBase.Instance.emptyItem);
        }
    }


    //아이템 정렬 =>빈칸 뒤로 미룸
    public void CompactItemList(List<BaseItem> list)
    {
        var validItems = list.Where(i => i.type != ItemType.Empty).ToList();
        int emptyCount = list.Count - validItems.Count;

        list = new List<BaseItem>(validItems);

        for (int i = 0; i < emptyCount; i++)
        {
            list.Add(ItemDataBase.Instance.emptyItem);
        }
    }

    //인벤토리 열기
    public void InvenOpen()
    {
        isInvenOpen = true;
        gameObject.SetActive(true);
    }

    //인벤토리 닫기
    public void InvenClose()
    {
        foreach (var s in slot)
        {
            s.GetComponent<ItemSlot>().ClearSlot();
        }
        isInvenOpen = false;
        gameObject.SetActive(false);
    }
}
