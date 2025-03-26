using UnityEngine;

public enum InvenType
{
    Inven,
    Box
}

//플레이어의 모든 인벤토리
public class InvenToryCtrl : MonoBehaviour
{
    //현재 장착한 장비 인벤토리

    //현재 흭득한 장비 인벤토리
        
    //현재 가지고있을 인벤토리
    public InventoryItems inventoryItems;
    //사물함 인벤토리
    public BoxInvenTory boxInvenTory;

    public static InvenToryCtrl Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ChangeItemByKey(InvenType fromType, ItemImageNumber itemKey)
    {
        BaseInventory from = (fromType == InvenType.Inven) ? inventoryItems : boxInvenTory;
        BaseInventory to = (fromType == InvenType.Inven) ? boxInvenTory : inventoryItems;

        BaseItem original = ItemDataBase.Instance.itemDB[itemKey];

        int fromIndex = from.Items.FindIndex(i => i.name == original.name);

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


}
