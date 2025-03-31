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
    public List<BaseItem> equippedInventory = new List<BaseItem>();

    //���� ŉ���� ��� �κ��丮
    public List<BaseItem> equipInventory = new List<BaseItem>();

    public EquipslotCtrl[] equippedUIslot;
    //���� ���� UI
    [SerializeField] EquipItemToolTipCtrl equipItemToolTipCtrl;
    public EquipItemToolTipCtrl EquipItemToolTipCtrl { get; set; }



    //���� ���������� �κ��丮
    public List<BaseItem> inventory = new List<BaseItem>();

    //�繰�� �κ��丮
    public List<BaseItem> boxInven = new List<BaseItem>();

    //�κ��� ���� UI
    [SerializeField] ItemToolTipCtrl itemToolTipCtrl;
    public ItemToolTipCtrl ItemToolTipCtrl { get; set; }

    //�̺�Ʈ
    public System.Action OnInventoryChanged;
    public System.Action<ItemName> OnEquippedChanged;

    //�̱���
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


        //������ ŉ��
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(inventory, ItemDataBase.Instance.GetItem(ItemName.PitfallTrap));

        //����κ� ������ŉ��
        for (int i = (int)ItemName.HuntersKnife_I; i <= (int)ItemName.AnjaGreavesS; i++)
        {
            GetItemToInventory(equipInventory, ItemDataBase.Instance.GetItem((ItemName)i));
        }

        
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
            Debug.LogWarning("[���� ����] ������ ������ ã�� �� �����ϴ�.");
            return false;
        }

        var targetSlot = equippedUIslot[slotIndex];
        if (!targetSlot.IsCorrectType(item))
        {
            Debug.LogWarning("[���� ����] ���� Ÿ���� ���� �ʽ��ϴ�.");
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
            Debug.Log("�̹� ������ ���� �Դϴ�.");
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
    //    Debug.Log("���� �� ���� ����: " + emptyCount);


    //    if (!ItemDataBase.Instance.itemDB.ContainsKey(itemKey))
    //    {
    //        Debug.LogWarning("itemDB�� �ش� Ű�� �����ϴ�.");
    //        return false;
    //    }

    //    BaseItem baseItem = ItemDataBase.Instance.GetItem(itemKey);

    //    // 1. �̹� ���� �������� ������ -> count++
    //    int index = toList.FindIndex(i => i.name == baseItem.name);
    //    if (index >= 0)
    //    {
    //        return false;
    //    }

    //    EquipSlot slot = (EquipSlot)baseItem.GetEquipSlot();


    //    // 2. �� ������ ������ -> �����ؼ� �߰�
    //    int emptyIndex = toList.FindIndex(i => i.type == ItemType.Empty);

    //    if (emptyIndex >= 0)
    //    {
    //        //�ڲ� ���������� �����ؼ� �־��ָ� ������ �������� count�� ���� ������
    //        //�׷��� Ŭ�и޼��� ����
    //        BaseItem copy = baseItem.Clone();
    //        copy.count = 1; // ������ �� count ���� �ʿ�

    //        Debug.Log($"[ChangeItem] ���ο� ������ �߰���: {copy.name} �� ���� {emptyIndex}");
    //        toList[emptyIndex] = copy;
    //        return true;
    //    }

    //    Debug.Log("������ ���� á���ϴ�.");
    //    return false;
    //}

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
