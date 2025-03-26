using UnityEngine;

public enum InvenType
{
    Inven,
    Box,
    Equipped,
    EquipBox
}

//플레이어의 모든 인벤토리
public class InvenToryCtrl : MonoBehaviour
{
    //현재 장착한 장비 인벤토리
    [SerializeField] EquippedInventoryUI equippedInventoryUI;
    public EquippedInventoryUI EquippedInventoryUI => equippedInventoryUI;

    //현재 흭득한 장비 인벤토리
    [SerializeField] EquipInventoryUI equipInventoryUI;
    public EquipInventoryUI EquipInventoryUI => equipInventoryUI;

    //현재 가지고있을 인벤토리
    [SerializeField] InventoryItems inventoryItems;
    public InventoryItems InventoryItems => inventoryItems;
    //사물함 인벤토리
    [SerializeField] BoxInvenTory boxInvenTory;
    public BoxInvenTory BoxInvenTory => boxInvenTory;

    //인벤용 툴팁 UI
    [SerializeField] ItemToolTipCtrl itemToolTipCtrl;
    public ItemToolTipCtrl ItemToolTipCtrl => itemToolTipCtrl;

    //장비용 툴팁 UI
    [SerializeField] EquipItemToolTipCtrl equipItemToolTipCtrl;
    public EquipItemToolTipCtrl EquipItemToolTipCtrl => equipItemToolTipCtrl;

    public static InvenToryCtrl Instance;

    private void Awake()
    {
        if (Instance != null )
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    //소지 인벤토리와 창고 인벤토리 전용
    public void ChangeItemByKey(InvenType fromType, ItemImageNumber itemKey)
    {
        if (fromType == InvenType.Equipped || fromType == InvenType.EquipBox)
        {
            Debug.LogError("장비 인벤토리에서는 아이템을 교환할 수 없습니다.");
            return;
        }

        BaseInventory from = (fromType == InvenType.Inven) ? inventoryItems : boxInvenTory;
        BaseInventory to = (fromType == InvenType.Inven) ? boxInvenTory : inventoryItems;

        //BaseItem original = ItemDataBase.Instance.itemDB[itemKey];
        //위 코드에서 클론 매서드(주소형에서 값형으로 변환후 반환) 만들어둔거 사용해서 바꿈
        BaseItem original = ItemDataBase.Instance.GetItem(itemKey);

        int fromIndex = from.Items.FindIndex(i => i.key == original.key);

        if (fromIndex >= 0)
        {
            from.Items[fromIndex].count--;
            if (from.Items[fromIndex].count <= 0)
            {
                from.Items[fromIndex] = ItemDataBase.Instance.emptyItem;
            }
        }

        to.ChangeItem(to.Items, itemKey);

        inventoryItems.CompactItemList();
        boxInvenTory.CompactItemList();

        inventoryItems.RefreshUI();
        boxInvenTory.RefreshUI();
    }


    //장비 인벤토리에서 장비 장착 하기
    public void EquipItem(ItemImageNumber itemKey)
    {
        BaseItem item = ItemDataBase.Instance.GetItem(itemKey);

        if (item.type != ItemType.Weapon &&
            item.type != ItemType.Armor)
        {
            Debug.LogError("장비 아이템이 아닙니다.");
            return;
        }
        if (equippedInventoryUI.Items.FindIndex(i => i.name == item.name) >= 0)
        {
            Debug.LogError("이미 장착한 아이템입니다.");
            return;
        }

        equippedInventoryUI.ChangeItem(equippedInventoryUI.Items, itemKey);
        equipInventoryUI.ChangeItem(equipInventoryUI.Items, itemKey);
        equippedInventoryUI.RefreshUI();
        equipInventoryUI.RefreshUI();
    }

    //장비 인벤토리에서 장비 해제 하기
    public void UnEquipItem(ItemImageNumber itemKey)
    {
        BaseItem item = ItemDataBase.Instance.GetItem(itemKey);

        
        if (item.type != ItemType.Weapon && item.type != ItemType.Armor)
        {
            Debug.LogError("장비 아이템이 아닙니다.");
            return;
        }
        if (equippedInventoryUI.Items.FindIndex(i => i.name == item.name) < 0)
        {
            Debug.LogError("장착한 아이템이 아닙니다.");
            return;
        }

        equippedInventoryUI.ChangeItem(equippedInventoryUI.Items, itemKey);
        equipInventoryUI.ChangeItem(equipInventoryUI.Items, itemKey);
        equippedInventoryUI.RefreshUI();
        equipInventoryUI.RefreshUI();
    }




}
