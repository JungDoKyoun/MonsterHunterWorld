using UnityEngine;

public enum InvenType
{
    Inven,
    Box,
    Equipped,
    EquipBox
}

//�÷��̾��� ��� �κ��丮
public class InvenToryCtrl : MonoBehaviour
{
    //���� ������ ��� �κ��丮
    [SerializeField] EquippedInventoryUI equippedInventoryUI;
    public EquippedInventoryUI EquippedInventoryUI => equippedInventoryUI;

    //���� ŉ���� ��� �κ��丮
    [SerializeField] EquipInventoryUI equipInventoryUI;
    public EquipInventoryUI EquipInventoryUI => equipInventoryUI;
    
    //���� ���� UI
    [SerializeField] EquipItemToolTipCtrl equipItemToolTipCtrl;
    public EquipItemToolTipCtrl EquipItemToolTipCtrl => equipItemToolTipCtrl;

    //���� ���������� �κ��丮
    [SerializeField] InventoryItems inventoryItems;
    public InventoryItems InventoryItems => inventoryItems;
    //�繰�� �κ��丮
    [SerializeField] BoxInvenTory boxInvenTory;
    public BoxInvenTory BoxInvenTory => boxInvenTory;

    //�κ��� ���� UI
    [SerializeField] ItemToolTipCtrl itemToolTipCtrl;
    public ItemToolTipCtrl ItemToolTipCtrl => itemToolTipCtrl;


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


    //���� �κ��丮�� â�� �κ��丮 ����
    public void ChangeItemByKey(InvenType fromType, ItemName itemKey)
    {
        if (fromType == InvenType.Equipped || fromType == InvenType.EquipBox)
        {
            Debug.LogError("��� �κ��丮������ �������� ��ȯ�� �� �����ϴ�.");
            return;
        }

        BaseInventory from = (fromType == InvenType.Inven) ? inventoryItems : boxInvenTory;
        BaseInventory to = (fromType == InvenType.Inven) ? boxInvenTory : inventoryItems;

        //BaseItem original = ItemDataBase.Instance.itemDB[itemKey];
        //�� �ڵ忡�� Ŭ�� �ż���(�ּ������� �������� ��ȯ�� ��ȯ) �����а� ����ؼ� �ٲ�
        BaseItem original = ItemDataBase.Instance.GetItem(itemKey);

        int fromIndex = from.Items.FindIndex(i => i.id == original.id);

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


    public void ChangeEquipItemByKey(InvenType fromType, ItemName itemKey)
    {
        if (fromType == InvenType.Equipped || fromType == InvenType.EquipBox)
        {
            Debug.LogError("��� �κ��丮������ �������� ��ȯ�� �� �����ϴ�.");
            return;
        }

        BaseInventory from = (fromType == InvenType.Inven) ? inventoryItems : boxInvenTory;
        BaseInventory to = (fromType == InvenType.Inven) ? boxInvenTory : inventoryItems;

        //BaseItem original = ItemDataBase.Instance.itemDB[itemKey];
        //�� �ڵ忡�� Ŭ�� �ż���(�ּ������� �������� ��ȯ�� ��ȯ) �����а� ����ؼ� �ٲ�
        BaseItem original = ItemDataBase.Instance.GetItem(itemKey);

        int fromIndex = from.Items.FindIndex(i => i.id == original.id);

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


    //��� �κ��丮���� ��� ���� �ϱ�
    public void EquipItem(ItemName itemKey)
    {
        BaseItem item = ItemDataBase.Instance.GetItem(itemKey);

        if (item.type != ItemType.Weapon &&
            item.type != ItemType.Armor)
        {
            Debug.LogError("��� �������� �ƴմϴ�.");
            return;
        }
        if (equippedInventoryUI.Items.FindIndex(i => i.name == item.name) >= 0)
        {
            Debug.LogError("�̹� ������ �������Դϴ�.");
            return;
        }



        equippedInventoryUI.ChangeItem(equippedInventoryUI.Items, itemKey);
        equippedInventoryUI.EquipItem(item);

        //equipInventoryUI.ChangeItem(equipInventoryUI.Items, itemKey);

        equippedInventoryUI.RefreshUI();
        equipInventoryUI.RefreshUI();
    }

    //��� �κ��丮���� ��� ���� �ϱ�
    public void UnEquipItem(ItemName itemKey)
    {
        BaseItem item = ItemDataBase.Instance.GetItem(itemKey);

        
        if (item.type != ItemType.Weapon && item.type != ItemType.Armor)
        {
            Debug.LogError("��� �������� �ƴմϴ�.");
            return;
        }
        if (equippedInventoryUI.Items.FindIndex(i => i.name == item.name) < 0)
        {
            Debug.LogError("������ �������� �ƴմϴ�.");
            return;
        }

        equippedInventoryUI.ChangeItem(equippedInventoryUI.Items, itemKey);
        equipInventoryUI.ChangeItem(equipInventoryUI.Items, itemKey);
        equippedInventoryUI.RefreshUI();
        equipInventoryUI.RefreshUI();
    }




}
