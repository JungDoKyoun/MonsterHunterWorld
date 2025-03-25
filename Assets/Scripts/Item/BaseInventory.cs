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

    public bool TryAddItemToInventory(BaseItem newItem)
    {
        if (newItem == null || newItem.type == ItemType.Empty)
            return false;

        // 1. 같은 이름의 아이템이 있으면 → count 증가
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
        int emptyIndex = items.FindIndex(i => i.type == ItemType.Empty);

        if (emptyIndex >= 0)
        {
            // 새로운 BaseItem 생성해서 넣어줘야 참조 충돌 없음
            items[emptyIndex] = new BaseItem
            {
                image = newItem.image,
                name = newItem.name,
                type = newItem.type,
                rarity = newItem.rarity,
                count = 1,
                maxCount = newItem.maxCount,
                allCount = newItem.allCount,
                color = newItem.color,
                tooltip = newItem.tooltip,
                price = newItem.price
            };

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

    //아이템 추가
    public void AddItem(List<BaseItem> currentItems, BaseItem changeItems)
    {
        int index = currentItems.FindIndex(i => i == emptyItem);
        if (index >= 0)
        {
            Debug.Log(changeItems.name);
            currentItems[index] = changeItems;
        }
        else
        {
            currentItems.Add(changeItems);
        }
    }

    //아이템 삭제
    public void RemoveItem(List<BaseItem> currentItems, BaseItem changeItems)
    {
        int index = currentItems.FindIndex(i => i == changeItems);
        if (index >= 0)
        {
            currentItems[index] = emptyItem;
        }
    }

    //아이템 교환
public bool ChangeItem(List<BaseItem> current, ItemImageNumber itemKey)
{
    if (!ItemDataBase.Instance.itemDB.ContainsKey(itemKey))
    {
        Debug.LogWarning("itemDB에 해당 키가 없습니다.");
        return false;
    }

    BaseItem baseItem = ItemDataBase.Instance.itemDB[itemKey];

    // 1. 이미 같은 아이템이 있으면 → count++
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

    // 2. 빈 슬롯이 있으면 → 복사해서 추가
    int emptyIndex = current.FindIndex(i => i.type == ItemType.Empty);
    if (emptyIndex >= 0)
    {
        BaseItem copy = baseItem.Clone();
        copy.count = 1; // 복사할 때 count 조정 필요
        current[emptyIndex] = copy;
        return true;
    }

    Debug.Log("슬롯이 가득 찼습니다.");
    return false;
}



    //아이템 슬롯 갱신
    public virtual void RefreshUI(List<GameObject> slots, List<BaseItem> item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var slotComp = slots[i].GetComponent<ItemSlot>();
            if (slotComp == null)
            {
                Debug.LogWarning($"슬롯 {i}번에 ItemSlot 컴포넌트가 없습니다.");
                continue;
            }

            // item[i]가 null이면 emptyItem으로 대체
            BaseItem currentItem =
                (i < item.Count && item[i] != null) ? item[i] : ItemDataBase.Instance.emptyItem;

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
