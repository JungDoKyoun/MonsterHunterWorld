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
    //[SerializeField] EquippedInventoryUI equippedInventoryUI;
    //public EquippedInventoryUI EquippedInventoryUI => equippedInventoryUI;

    public List<BaseItem> equippedInventory = new List<BaseItem>();

    //현재 흭득한 장비 인벤토리
    //[SerializeField] EquipInventoryUI equipInventoryUI;
    //public EquipInventoryUI EquipInventoryUI => equipInventoryUI;
    
    public List<BaseItem> equipInventory = new List<BaseItem>();


    //장비용 툴팁 UI
    [SerializeField] EquipItemToolTipCtrl equipItemToolTipCtrl;
    public EquipItemToolTipCtrl EquipItemToolTipCtrl => equipItemToolTipCtrl;

    //현재 가지고있을 인벤토리
    //[SerializeField] InventoryItems inventoryItems;
    //public InventoryItems InventoryItems => inventoryItems;

    public List<BaseItem> inventory = new List<BaseItem>();

    //사물함 인벤토리
    //[SerializeField] BoxInvenTory boxInvenTory;
    //public BoxInvenTory BoxInvenTory => boxInvenTory;

    public List<BaseItem> boxInven = new List<BaseItem>();


    //인벤용 툴팁 UI
    [SerializeField] ItemToolTipCtrl itemToolTipCtrl;
    public ItemToolTipCtrl ItemToolTipCtrl => itemToolTipCtrl;

    public System.Action OnInventoryChanged;
    public System.Action OnEquippedChanged;

    public static InvenToryCtrl Instance;

    private void Awake()
    {
        if (Instance != null )
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InvenInit(inventory, (int)InvenSize.Inventory);
        InvenInit(boxInven, (int)InvenSize.BoxInven);
        InvenInit(equipInventory, (int)InvenSize.EquipInven);
        InvenInit(equippedInventory, (int)InvenSize.EquipedInven);

    }

    void InvenInit(List<BaseItem> list ,int count)
    {
        for (int i = 0; i < count; i++)
        {
            list.Add(ItemDataBase.Instance.emptyItem);
        }
    }


    //소지 인벤토리와 창고 인벤토리 전용
    public void ChangeItemByKey(List<BaseItem> from, List<BaseItem> to ,ItemName itemKey)
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

    //아이템 정렬 =>빈칸 뒤로 미룸
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

    //아이템 교환
    public bool ChangeItem(List<BaseItem> current, ItemName itemKey)
    {
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

}
