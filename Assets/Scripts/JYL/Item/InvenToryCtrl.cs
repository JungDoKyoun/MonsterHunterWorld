using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InvenType
{
    Inven,
    Box,
    Equipped,
    EquipBox
}

public enum InvenSize
{
    Inventory = 24,
    BoxInven = 100,
    EquipInven = 50,
    EquipedInven = 9
}

//플레이어의 모든 인벤토리
public class InvenToryCtrl : MonoBehaviour
{
    //현재 장착한 장비 인벤토리
    public List<BaseItem> equippedInventory = new List<BaseItem>();

    //현재 흭득한 장비 인벤토리
    public List<BaseItem> equipInventory = new List<BaseItem>();

    public EquipslotCtrl[] equippedUIslot;
    //장비용 툴팁 UI
    [SerializeField] EquipItemToolTipCtrl equipItemToolTipCtrl;
    public EquipItemToolTipCtrl EquipItemToolTipCtrl { get; set; }



    //현재 가지고있을 인벤토리
    public List<BaseItem> inventory = new List<BaseItem>();

    //사물함 인벤토리
    public List<BaseItem> boxInven = new List<BaseItem>();

    //인벤용 툴팁 UI
    [SerializeField] ItemToolTipCtrl itemToolTipCtrl;
    public ItemToolTipCtrl ItemToolTipCtrl { get; set; }

    //이벤트
    public System.Action OnInventoryChanged;
    public System.Action<ItemName> OnEquippedChanged;

    //싱글톤
    public static InvenToryCtrl Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;




    }

    private void Start()
    {
        InvenInit(inventory, (int)InvenSize.Inventory);
        InvenInit(boxInven, (int)InvenSize.BoxInven);
        InvenInit(equipInventory, (int)InvenSize.EquipInven);
        InvenInit(equippedInventory, (int)InvenSize.EquipedInven);


        //아이템 흭득
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.PitfallTrap));

        //장비인벤 아이템흭득
        for (int i = (int)ItemName.HuntersKnife_I; i <= (int)ItemName.AnjaGreavesS; i++)
        {
            GetItemToInventory(equipInventory, ItemDataBase.Instance.GetItem((ItemName)i));
        }

        
    }

    /// <summary>
    /// 인벤 빈 아이템 세팅
    /// </summary>
    /// <param name="list">세팅할 인벤 </param>
    /// <param name="count">세팅할 인벤 크기</param>
    void InvenInit(List<BaseItem> list, int count)
    {
        for (int i = 0; i < count; i++)
        {
            list.Add(ItemDataBase.Instance.emptyItem);
        }
    }


    /// <summary>
    /// 소지 인벤토리와 창고 인벤토리 전용
    /// </summary>
    /// <param name="from">보낼 인벤</param>
    /// <param name="to">받을 인벤</param>
    /// <param name="itemKey"> 아이템 키 </param>
    public void ChangeItemByKey(List<BaseItem> from, List<BaseItem> to, ItemName itemKey)
    {
        BaseItem original = ItemDataBase.Instance.GetItem(itemKey);

        int fromIndex = from.FindIndex(i => i.id == original.id);

        if (fromIndex >= 0)
        {
            from[fromIndex].count--;
            if (from[fromIndex].count <= 0)
            {
                from[fromIndex] = ItemDataBase.Instance.emptyItem;
            }
        }

        ChangeItem(to, itemKey);

        CompactItemList(from);
        CompactItemList(to);

    }

    public void EquipItemByKey(List<BaseItem> from, List<BaseItem> to, ItemName itemKey)
    {
        BaseItem baseItem = ItemDataBase.Instance.GetItem(itemKey);

        if (TryEquipItem(baseItem))
        {
            int fromIndex = from.FindIndex(i => i.id == baseItem.id);

            if (fromIndex >= 0)
            {
                from[fromIndex].count--;
                if (from[fromIndex].count <= 0)
                {
                    from[fromIndex] = ItemDataBase.Instance.emptyItem;
                }
            }
        }

        CompactItemList(from);
        CompactItemList(to);

    }

    public bool TryEquipItem(BaseItem item)
    {

        //Debug.Log(equippedUIslot.Length);
        

        var slotIndex = GetSlotIndexFromItem(item);
        if (slotIndex < 0 || slotIndex >= equippedUIslot.Length)
        {
            Debug.LogWarning("[장착 실패] 적절한 슬롯을 찾을 수 없습니다.");
            return false;
        }

        var targetSlot = equippedUIslot[slotIndex];
        if (!targetSlot.IsCorrectType(item))
        {
            Debug.LogWarning("[장착 실패] 슬롯 타입이 맞지 않습니다.");
            return false;
        }

        if (equippedInventory[slotIndex].name == "")
        {
            equippedInventory[slotIndex] = item.Clone();

            OnInventoryChanged?.Invoke();
            OnEquippedChanged?.Invoke(item.id);
            return true;
        }
        else
        {
            Debug.Log("이미 장착된 부위 입니다.");
            return false;
        }
    }

    private int GetSlotIndexFromItem(BaseItem item)
    {
        if (item is Armor armor)
            return (int)armor.equipType;
        if (item is Weapon)
            return (int)EquipSlot.Weapon;

        return -1;
    }


    //public bool ChangeEquipItem(List<BaseItem> toList, ItemName itemKey)
    //{
    //    int emptyCount = toList.Count(i => i.type == ItemType.Empty);
    //    Debug.Log("현재 빈 슬롯 개수: " + emptyCount);


    //    if (!ItemDataBase.Instance.itemDB.ContainsKey(itemKey))
    //    {
    //        Debug.LogWarning("itemDB에 해당 키가 없습니다.");
    //        return false;
    //    }

    //    BaseItem baseItem = ItemDataBase.Instance.GetItem(itemKey);

    //    // 1. 이미 같은 아이템이 있으면 -> count++
    //    int index = toList.FindIndex(i => i.name == baseItem.name);
    //    if (index >= 0)
    //    {
    //        return false;
    //    }

    //    EquipSlot slot = (EquipSlot)baseItem.GetEquipSlot();


    //    // 2. 빈 슬롯이 있으면 -> 복사해서 추가
    //    int emptyIndex = toList.FindIndex(i => i.type == ItemType.Empty);

    //    if (emptyIndex >= 0)
    //    {
    //        //자꾸 같은아이템 복사해서 넣어주면 참조가 같아져서 count가 같이 증가함
    //        //그래서 클론메서드 만듬
    //        BaseItem copy = baseItem.Clone();
    //        copy.count = 1; // 복사할 때 count 조정 필요

    //        Debug.Log($"[ChangeItem] 새로운 아이템 추가됨: {copy.name} → 슬롯 {emptyIndex}");
    //        toList[emptyIndex] = copy;
    //        return true;
    //    }

    //    Debug.Log("슬롯이 가득 찼습니다.");
    //    return false;
    //}

    /// <summary>
    /// 아이템 정렬 =>빈칸 뒤로 미룸
    /// </summary>
    /// <param name="list">정렬할 인벤토리</param>
    void CompactItemList(List<BaseItem> list)
    {
        var validItems = list.Where(i => i.type != ItemType.Empty).ToList();
        int emptyCount = list.Count - validItems.Count;

        list = new List<BaseItem>(validItems);

        for (int i = 0; i < emptyCount; i++)
        {
            list.Add(ItemDataBase.Instance.emptyItem);
        }
    }

    /// <summary>
    /// 아이템 교환
    /// </summary>
    /// <param name="current">받을 인벤토리</param>
    /// <param name="itemKey">아이템 키</param>
    /// <returns></returns>
    public bool ChangeItem(List<BaseItem> toList, ItemName itemKey)
    {
        int emptyCount = toList.Count(i => i.type == ItemType.Empty);
        Debug.Log("현재 빈 슬롯 개수: " + emptyCount);


        if (!ItemDataBase.Instance.itemDB.ContainsKey(itemKey))
        {
            Debug.LogWarning("itemDB에 해당 키가 없습니다.");
            return false;
        }

        BaseItem baseItem = ItemDataBase.Instance.GetItem(itemKey);

        // 1. 이미 같은 아이템이 있으면 -> count++
        int index = toList.FindIndex(i => i.name == baseItem.name);
        if (index >= 0)
        {
            if (toList[index].count < toList[index].maxCount)
            {
                toList[index].count++;
                return true;
            }
            else
            {
                Debug.Log("최대 수량 도달");
                return false;
            }
        }

        // 2. 빈 슬롯이 있으면 -> 복사해서 추가
        int emptyIndex = toList.FindIndex(i => i.type == ItemType.Empty);

        if (emptyIndex >= 0)
        {
            //자꾸 같은아이템 복사해서 넣어주면 참조가 같아져서 count가 같이 증가함
            //그래서 클론메서드 만듬
            BaseItem copy = baseItem.Clone();
            copy.count = 1; // 복사할 때 count 조정 필요

            Debug.Log($"[ChangeItem] 새로운 아이템 추가됨: {copy.name} → 슬롯 {emptyIndex}");
            toList[emptyIndex] = copy;
            return true;
        }

        Debug.Log("슬롯이 가득 찼습니다.");
        return false;
    }

    //흭득 아이템 인벤토리로 넣기
    public bool GetItemToInventory(List<BaseItem> list, BaseItem newItem)
    {
        //Debug.Log(newItem.name + " 흭득!");

        if (newItem == null || newItem.type == ItemType.Empty)
            return false;

        // 1. 같은 이름의 아이템이 있으면 -> count 증가
        int index = list.FindIndex(i => i.name == newItem.name);

        if (index >= 0)
        {
            var target = list[index];
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
        int emptyIndex = list.FindIndex(i => i.id == ItemName.Empty);

        if (emptyIndex >= 0)
        {
            // 새로운 BaseItem 생성해서 넣어줘야 참조 충돌 없음
            BaseItem copy = newItem.Clone();
            copy.count = 1; // 새로 들어오는 아이템이니까 수량은 1로 초기화
            list[emptyIndex] = copy;

            return true;
        }

        Debug.Log("빈 슬롯 없음");
        return false;
    }

}
