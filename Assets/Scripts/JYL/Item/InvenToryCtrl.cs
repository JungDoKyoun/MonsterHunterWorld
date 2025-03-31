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
    public EquipItemToolTipCtrl EquipItemToolTipCtrl { get; set; }

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
    public ItemToolTipCtrl ItemToolTipCtrl { get; set; }

    public System.Action OnInventoryChanged;
    public System.Action OnEquippedChanged;

    public static InvenToryCtrl Instance;

    private void Awake()
    {
        if (Instance != null)
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

    private void Start()
    {
        //������ ŉ��
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.PitfallTrap));

        //����κ� ������ŉ��
        GetItemToInventory(equipInventory, ItemDataBase.Instance.GetItem(ItemName.HuntersKnife_I));
        GetItemToInventory(equipInventory, ItemDataBase.Instance.GetItem(ItemName.LeatherVest));
    }

    /// <summary>
    /// �κ� �� ������ ����
    /// </summary>
    /// <param name="list">������ �κ� </param>
    /// <param name="count">������ �κ� ũ��</param>
    void InvenInit(List<BaseItem> list, int count)
    {
        for (int i = 0; i < count; i++)
        {
            list.Add(ItemDataBase.Instance.emptyItem);
        }
    }


    /// <summary>
    /// ���� �κ��丮�� â�� �κ��丮 ����
    /// </summary>
    /// <param name="from">���� �κ�</param>
    /// <param name="to">���� �κ�</param>
    /// <param name="itemKey"> ������ Ű </param>
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

    /// <summary>
    /// ������ ���� =>��ĭ �ڷ� �̷�
    /// </summary>
    /// <param name="list">������ �κ��丮</param>
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
    /// ������ ��ȯ
    /// </summary>
    /// <param name="current">���� �κ��丮</param>
    /// <param name="itemKey">������ Ű</param>
    /// <returns></returns>
    public bool ChangeItem(List<BaseItem> toList, ItemName itemKey)
    {
        int emptyCount = toList.Count(i => i.type == ItemType.Empty);
        Debug.Log("���� �� ���� ����: " + emptyCount);


        if (!ItemDataBase.Instance.itemDB.ContainsKey(itemKey))
        {
            Debug.LogWarning("itemDB�� �ش� Ű�� �����ϴ�.");
            return false;
        }

        BaseItem baseItem = ItemDataBase.Instance.GetItem(itemKey);

        // 1. �̹� ���� �������� ������ -> count++
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
                Debug.Log("�ִ� ���� ����");
                return false;
            }
        }

        // 2. �� ������ ������ -> �����ؼ� �߰�
        int emptyIndex = toList.FindIndex(i => i.type == ItemType.Empty);

        if (emptyIndex >= 0)
        {
            //�ڲ� ���������� �����ؼ� �־��ָ� ������ �������� count�� ���� ������
            //�׷��� Ŭ�и޼��� ����
            BaseItem copy = baseItem.Clone();
            copy.count = 1; // ������ �� count ���� �ʿ�

            Debug.Log($"[ChangeItem] ���ο� ������ �߰���: {copy.name} �� ���� {emptyIndex}");
            toList[emptyIndex] = copy;
            return true;
        }

        Debug.Log("������ ���� á���ϴ�.");
        return false;
    }

    //ŉ�� ������ �κ��丮�� �ֱ�
    public bool GetItemToInventory(List<BaseItem> list, BaseItem newItem)
    {
        //Debug.Log(newItem.name + " ŉ��!");

        if (newItem == null || newItem.type == ItemType.Empty)
            return false;

        // 1. ���� �̸��� �������� ������ -> count ����
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
                Debug.Log("�ִ� ���� �ʰ�");
                return false;
            }
        }

        // 2. �ƴϸ� �� ���Կ� ���� �߰�
        int emptyIndex = list.FindIndex(i => i.id == ItemName.Empty);

        if (emptyIndex >= 0)
        {
            // ���ο� BaseItem �����ؼ� �־���� ���� �浹 ����
            BaseItem copy = newItem.Clone();
            copy.count = 1; // ���� ������ �������̴ϱ� ������ 1�� �ʱ�ȭ
            list[emptyIndex] = copy;

            return true;
        }

        Debug.Log("�� ���� ����");
        return false;
    }

}
