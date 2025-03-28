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
    //List<BaseItem> items { get; set; }
    protected List<BaseItem> items = new List<BaseItem>();
    public List<BaseItem> Items
    {
        get => items;
        set => items = value;
    }

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

    protected void InvenInit()
    {
        // 슬롯 수만큼 아이템 리스트에 emptyItem 채워넣기
        for (int i = 0; i < slot.Count; i++)
        {
            items.Add(ItemDataBase.Instance.emptyItem);
        }
    }

    //흭득 아이템 인벤토리로 넣기
    public bool GetItemToInventory(BaseItem newItem)
    {
        //Debug.Log(newItem.name + " 흭득!");

        if (newItem == null || newItem.type == ItemType.Empty)
            return false;

        // 1. 같은 이름의 아이템이 있으면 -> count 증가
        int index = items.FindIndex(i => i.name == newItem.name);

        if (index >= 0)
        {
            var target = items[index];
            if (target.count < target.maxCount)
            {
                target.count++;
                return true;
            }
            else
            {
                Debug.Log("최대 수량 초과");
                return false;
            }
        }

        // 2. 아니면 빈 슬롯에 새로 추가
        int emptyIndex = items.FindIndex(i => i.id == ItemName.Empty);

        if (emptyIndex >= 0)
        {
            // 새로운 BaseItem 생성해서 넣어줘야 참조 충돌 없음
            BaseItem copy = newItem.Clone();
            copy.count = 1; // 새로 들어오는 아이템이니까 수량은 1로 초기화
            items[emptyIndex] = copy;

            return true;
        }

        Debug.Log("빈 슬롯 없음");
        return false;
    }


    //슬롯 세팅
    public void SlotSetting(GameObject current, InvenType type)
    {
        //자식 오브젝트 연결
        var objs = current.GetComponentsInChildren<ItemSlot>();
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

    //아이템 교환
    public bool ChangeItem(List<BaseItem> current, ItemName itemKey)
    {
        Debug.Log(current.Count);

        int emptyCount = current.Count(i => i.type == ItemType.Empty);
        Debug.Log("현재 빈 슬롯 개수: " + emptyCount);

        
        if (!ItemDataBase.Instance.itemDB.ContainsKey(itemKey))
        {
            Debug.LogWarning("itemDB에 해당 키가 없습니다.");
            return false;
        }

        BaseItem baseItem = ItemDataBase.Instance.GetItem(itemKey);

        // 1. 이미 같은 아이템이 있으면 -> count++
        int index = current.FindIndex(i => i.name == baseItem.name);
        if (index >= 0)
        {
            if (current[index].count < current[index].maxCount)
            {
                current[index].count++;
                return true;
            }
            else
            {
                Debug.Log("최대 수량 도달");
                return false;
            }
        }

        // 2. 빈 슬롯이 있으면 -> 복사해서 추가
        int emptyIndex = current.FindIndex(i => i.type == ItemType.Empty);

        if (emptyIndex >= 0)
        {
            //자꾸 같은아이템 복사해서 넣어주면 참조가 같아져서 count가 같이 증가함
            //그래서 클론메서드 만듬
            BaseItem copy = baseItem.Clone();
            copy.count = 1; // 복사할 때 count 조정 필요

            Debug.Log($"[ChangeItem] 새로운 아이템 추가됨: {copy.name} → 슬롯 {emptyIndex}");
            current[emptyIndex] = copy;
            return true;
        }

        Debug.Log("슬롯이 가득 찼습니다.");
        return false;
    }



    //아이템 슬롯 갱신
    public virtual void RefreshUI()
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
                (i < items.Count && items[i] != null) ? items[i] : ItemDataBase.Instance.emptyItem;
            //Debug.Log($"슬롯 {i}: {items[i].name}, 타입: {items[i].type}, count: {items[i].count}");
            slotComp.SlotSetItem(currentItem);
        }
    }

    //아이템 타입별로 정렬
    public void SortItemsByType()
    {
        // 1. 유효한 아이템만 추림
        var validItems = items.Where(i => i.type != ItemType.Empty).ToList();

        // 2. 타입 기준으로 정렬
        validItems = validItems.OrderBy(i => i.type).ToList();

        // 3. 빈 슬롯 수 계산
        int emptyCount = items.Count - validItems.Count;

        // 4. 리스트 재구성
        items = new List<BaseItem>(validItems);
        for (int i = 0; i < emptyCount; i++)
        {
            items.Add(ItemDataBase.Instance.emptyItem);
        }
    }


    //아이템 정렬 =>빈칸 뒤로 미룸
    public void CompactItemList()
    {
        var validItems = items.Where(i => i.type != ItemType.Empty).ToList();
        int emptyCount = items.Count - validItems.Count;

        items = new List<BaseItem>(validItems);

        for (int i = 0; i < emptyCount; i++)
        {
            items.Add(ItemDataBase.Instance.emptyItem);
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
