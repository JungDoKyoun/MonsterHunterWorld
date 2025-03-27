using UnityEngine;

public enum InvenType
{
    Inven,
    Box
}

//�÷��̾��� ��� �κ��丮
public class InvenToryCtrl : MonoBehaviour
{
    //���� ������ ��� �κ��丮

    //���� ŉ���� ��� �κ��丮
        
    //���� ���������� �κ��丮
    public InventoryItems inventoryItems;
    //�繰�� �κ��丮
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
