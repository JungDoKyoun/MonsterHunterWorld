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

//�÷��̾��� ��� �κ��丮
public class InvenToryCtrl : MonoBehaviour
{
    //���� ������ ��� �κ��丮
    //[SerializeField] EquippedInventoryUI equippedInventoryUI;
    //public EquippedInventoryUI EquippedInventoryUI => equippedInventoryUI;

    public List<BaseItem> equippedInventory = new List<BaseItem>();

    //���� ŉ���� ��� �κ��丮
    //[SerializeField] EquipInventoryUI equipInventoryUI;
    //public EquipInventoryUI EquipInventoryUI => equipInventoryUI;
    
    public List<BaseItem> equipInventory = new List<BaseItem>();


    //���� ���� UI
    [SerializeField] EquipItemToolTipCtrl equipItemToolTipCtrl;
    public EquipItemToolTipCtrl EquipItemToolTipCtrl => equipItemToolTipCtrl;

    //���� ���������� �κ��丮
    //[SerializeField] InventoryItems inventoryItems;
    //public InventoryItems InventoryItems => inventoryItems;

    public List<BaseItem> inventory = new List<BaseItem>();

    //�繰�� �κ��丮
    //[SerializeField] BoxInvenTory boxInvenTory;
    //public BoxInvenTory BoxInvenTory => boxInvenTory;

    public List<BaseItem> boxInven = new List<BaseItem>();


    //�κ��� ���� UI
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


    //���� �κ��丮�� â�� �κ��丮 ����
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

    //������ ���� =>��ĭ �ڷ� �̷�
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

    //������ ��ȯ
    public bool ChangeItem(List<BaseItem> current, ItemName itemKey)
    {
        int emptyCount = current.Count(i => i.type == ItemType.Empty);
        Debug.Log("���� �� ���� ����: " + emptyCount);


        if (!ItemDataBase.Instance.itemDB.ContainsKey(itemKey))
        {
            Debug.LogWarning("itemDB�� �ش� Ű�� �����ϴ�.");
            return false;
        }

        BaseItem baseItem = ItemDataBase.Instance.GetItem(itemKey);

        // 1. �̹� ���� �������� ������ -> count++
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
                Debug.Log("�ִ� ���� ����");
                return false;
            }
        }

        // 2. �� ������ ������ -> �����ؼ� �߰�
        int emptyIndex = current.FindIndex(i => i.type == ItemType.Empty);

        if (emptyIndex >= 0)
        {
            //�ڲ� ���������� �����ؼ� �־��ָ� ������ �������� count�� ���� ������
            //�׷��� Ŭ�и޼��� ����
            BaseItem copy = baseItem.Clone();
            copy.count = 1; // ������ �� count ���� �ʿ�

            Debug.Log($"[ChangeItem] ���ο� ������ �߰���: {copy.name} �� ���� {emptyIndex}");
            current[emptyIndex] = copy;
            return true;
        }

        Debug.Log("������ ���� á���ϴ�.");
        return false;
    }

}
