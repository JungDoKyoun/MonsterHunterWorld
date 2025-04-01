using Firebase.Auth;
using Firebase.Database;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static UnityEditor.Progress;

public enum InvenType
{
    Inven,
    Box,
    Equipped,
    EquipBox,
    QuickSlot
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

    public EquipslotCtrl[] equippedUiSlot;
    //���� ���� UI
    [SerializeField] EquipItemToolTipCtrl equipItemToolTipCtrl;
    public EquipItemToolTipCtrl EquipItemToolTipCtrl 
    {
        get
        {
            return equipItemToolTipCtrl;
        }
        set
        {
            equipItemToolTipCtrl = value;
        }
    }



    //���� ���������� �κ��丮
    public List<BaseItem> inventory = new List<BaseItem>();

    //�繰�� �κ��丮
    public List<BaseItem> boxInven = new List<BaseItem>();

    //������ �κ��丮
    public List<BaseItem> quickSlotItem = new List<BaseItem>();

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

    

   public void DebugTest()
    {
        if (equippedUiSlot == null || equippedUiSlot.Length == 0)
        {
            Debug.LogError("equippedUiSlot�� ������� �ʾҽ��ϴ�! Inspector���� Ȯ�����ּ���.");
        }

        Debug.Log("equippedUiSlot�� ���� �� �Ǿ���.");
    }

    private void Start()
    {
        LoadInventoryFromFirebase();
    }

    

    public void SlotSetting(EquipslotCtrl[] slots)
    {

        // �ڵ����� ���� UI �迭 �ʱ�ȭ
        if (slots == null || slots.Length == 0)
        {
            equippedUiSlot = slots
                .OrderBy(slot => slot.Type)
                .ToArray();
            Debug.Log($"[InvenToryCtrl] equippedUiSlot �ڵ� �ʱ�ȭ �Ϸ� ({equippedUiSlot.Length}��)");
        }
    }

    public async void SaveInventoryToFirebase()
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null) return;

        // �κ��丮�� null�̰ų� ��������� �⺻������ �ʱ�ȭ
        if (inventory == null || inventory.Count == 0)
            InvenInit(inventory, (int)InvenSize.Inventory);
        if (boxInven == null || boxInven.Count == 0)
            InvenInit(boxInven, (int)InvenSize.BoxInven);
        if (equipInventory == null || equipInventory.Count == 0)
            InvenInit(equipInventory, (int)InvenSize.EquipInven);
        if (equippedInventory == null || equippedInventory.Count == 0)
            InvenInit(equippedInventory, (int)InvenSize.EquipedInven);

        //�κ��丮�� ����
        StringBuilder sb = new StringBuilder();
        AppendItemListToCSV(sb, inventory);

        string csvData = sb.ToString();
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("inventoryData")
            .SetValueAsync(csvData);

        sb = new StringBuilder();
        AppendItemListToCSV(sb, boxInven);

        csvData = sb.ToString();
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("BoxInvenData")
            .SetValueAsync(csvData);

        sb = new StringBuilder();
        AppendItemListToCSV(sb, equipInventory);

        csvData = sb.ToString();
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("EquipInventoryData")
            .SetValueAsync(csvData);

        sb = new StringBuilder();
        AppendItemListToCSV(sb, equippedInventory);

        csvData = sb.ToString();
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("EquippedInventoryData")
            .SetValueAsync(csvData);

        //���Լ���
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("Weapon")
            .SetValueAsync(((int)equippedInventory[(int)EquipSlot.Weapon].id));
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("Head")
            .SetValueAsync(((int)equippedInventory[(int)EquipSlot.Head].id));
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("Breast")
            .SetValueAsync(((int)equippedInventory[(int)EquipSlot.Chest].id));
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("Hand")
            .SetValueAsync(((int)equippedInventory[(int)EquipSlot.Arms].id));
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("Waist")
            .SetValueAsync(((int)equippedInventory[(int)EquipSlot.Waist].id));
        await FirebaseDatabase.DefaultInstance.RootReference
            .Child(user.UserId)
            .Child("Leg")
            .SetValueAsync(((int)equippedInventory[(int)EquipSlot.Legs].id));

        OnInventoryChanged?.Invoke();

        Debug.Log("�κ��丮 ���� �Ϸ�");
    }

    public async void LoadInventoryFromFirebase()
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null) return;

        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;

        // 1. inventory ����Ʈ �ҷ�����
        var inventoryData = await root.Child(user.UserId).Child("inventoryData").GetValueAsync();
        if (inventoryData.Exists)
            InvenToryCtrl.Instance.inventory = ParseCSVToItemList(inventoryData.Value.ToString(), (int)InvenSize.Inventory);

        // 2. boxInven ����Ʈ �ҷ�����
        var boxData = await root.Child(user.UserId).Child("BoxInvenData").GetValueAsync();
        if (boxData.Exists)
            InvenToryCtrl.Instance.boxInven = ParseCSVToItemList(boxData.Value.ToString(), (int)InvenSize.BoxInven);

        // 3. equipInventory ����Ʈ �ҷ�����
        var equipData = await root.Child(user.UserId).Child("EquipInventoryData").GetValueAsync();
        if (equipData.Exists)
            InvenToryCtrl.Instance.equipInventory = ParseCSVToItemList(equipData.Value.ToString(), (int)InvenSize.EquipInven);

        // 4. equippedInventory ���Ժ� �ҷ�����
        var equippedList = new List<BaseItem>(new BaseItem[(int)InvenSize.EquipedInven]);
        equippedList[(int)EquipSlot.Weapon] = GetItemFromKey(await root.Child(user.UserId).Child("Weapon").GetValueAsync());
        equippedList[(int)EquipSlot.Head] = GetItemFromKey(await root.Child(user.UserId).Child("Head").GetValueAsync());
        equippedList[(int)EquipSlot.Chest] = GetItemFromKey(await root.Child(user.UserId).Child("Breast").GetValueAsync());
        equippedList[(int)EquipSlot.Arms] = GetItemFromKey(await root.Child(user.UserId).Child("Hand").GetValueAsync());
        equippedList[(int)EquipSlot.Waist] = GetItemFromKey(await root.Child(user.UserId).Child("Waist").GetValueAsync());
        equippedList[(int)EquipSlot.Legs] = GetItemFromKey(await root.Child(user.UserId).Child("Leg").GetValueAsync());
        equippedList[(int)EquipSlot.neck] = ItemDataBase.Instance.EmptyItem;
        equippedList[(int)EquipSlot.band] = ItemDataBase.Instance.EmptyItem;

        InvenToryCtrl.Instance.equippedInventory = equippedList;

        InvenToryCtrl.Instance.OnInventoryChanged?.Invoke();
        for (int i = 0; i < InvenToryCtrl.Instance.equippedInventory.Count - 1; i++)
        {
            var item = InvenToryCtrl.Instance.equippedInventory[i];
            if (item != null && item.id != ItemName.Empty)
                InvenToryCtrl.Instance.OnEquippedChanged?.Invoke(item.id);
        }

        if (equippedInventory != null)
             Debug.Log("�κ��丮 �ε� �Ϸ�");
        else
        {
            Debug.Log("??? �����߸���");
        }
    }


    // CSV �Ľ� �Լ�
    List<BaseItem> ParseCSVToItemList(string csv, int targetSize)
    {
        var result = new List<BaseItem>();
        var lines = csv.Split('\n');

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // [1,5] ���¸� ó��
            string trimmed = line.Trim(' ', '[', ']', '\r'); // ����, ��ȣ ����
            var parts = trimmed.Split(',');

            if (parts.Length >= 2 &&
                int.TryParse(parts[0], out int id) &&
                int.TryParse(parts[1], out int count))
            {
                var item = ItemDataBase.Instance.GetItem((ItemName)id).Clone();
                item.count = count;
                result.Add(item);
            }
        }

        // ������ ������ Empty�� ä��
        while (result.Count < targetSize)
        {
            result.Add(ItemDataBase.Instance.EmptyItem.Clone());
        }

        return result;
    }


    //���� ������ �ҷ����� �Լ�
    BaseItem GetItemFromKey(DataSnapshot snapshot)
    {
        if (snapshot.Exists && int.TryParse(snapshot.Value.ToString(), out int id))
        {
            return ItemDataBase.Instance.GetItem((ItemName)id);
        }
        return ItemDataBase.Instance.EmptyItem ;
    }


    void AppendItemListToCSV(StringBuilder sb,List<BaseItem> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = ItemDataBase.Instance.EmptyItem;
            }
            sb.AppendLine($"[{(int)items[i].id},{items[i].count}] ");
        }        
    }

    void LoadFromCSV(string csvData)
    {
        List<BaseItem> currentList = null;

        foreach (string line in csvData.Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.StartsWith("["))
            {
                string label = line.Trim('[', ']', '\r');
                currentList = label switch
                {
                    "Inventory" => inventory,
                    "BoxInven" => boxInven,
                    "EquipInventory" => equipInventory,
                    "EquippedInventory" => equippedInventory,
                    _ => null
                };
                currentList?.Clear();
                continue;
            }

            if (currentList == null) continue;

            var split = line.Split(',');
            if (split.Length < 2) continue;

            int id = int.Parse(split[0]);
            int count = int.Parse(split[1]);

            var item = ItemDataBase.Instance.GetItem((ItemName)id).Clone();
            item.count = count;
            currentList.Add(item);
        }

        OnInventoryChanged?.Invoke();

        //����κ� �������� �ڽ�
        for (int i = 0; i < equippedInventory.Count - 1; i++)
        {
            OnEquippedChanged?.Invoke(equippedInventory[i].id);
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
            list.Add(ItemDataBase.Instance.EmptyItem);
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
                from[fromIndex] = ItemDataBase.Instance.EmptyItem;

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
                    from[fromIndex] = ItemDataBase.Instance.EmptyItem;
                }
            }
        }

        CompactItemList(from);
        CompactItemList(to);

    }

    public bool TryEquipItem(BaseItem item)
    {
        var slotIndex = GetSlotIndexFromItem(item);
        if (slotIndex < 0 || slotIndex >= equippedUiSlot.Length)
        {
            Debug.LogWarning("[���� ����] ������ ������ ã�� �� �����ϴ�.");
            return false;
        }

        var targetSlot = equippedUiSlot[slotIndex];
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
            list.Add(ItemDataBase.Instance.EmptyItem);
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


    public void LoadQuickSlotItemsFromInventory()
    {
        quickSlotItem.Clear(); // ���� ������ �ʱ�ȭ

        foreach (var item in inventory)
        {
            if (item == null || item.id == ItemName.Empty) continue;

            // ����: trap, meat, position �̸� ���� �����۸� ���
            string name = item.name.ToLower();

            if (name.Contains("trap") || 
                name.Contains("meat") || 
                name.Contains("position"))
            {
                quickSlotItem.Add(item.Clone());
            }
        }

        Debug.Log($"�����Կ� {quickSlotItem.Count}���� �������� �ҷ��Խ��ϴ�.");
    }

}
