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

//플레이어의 모든 인벤토리
public class InvenToryCtrl : MonoBehaviour
{
    //현재 장착한 장비 인벤토리
    public List<BaseItem> equippedInventory = new List<BaseItem>();

    //현재 흭득한 장비 인벤토리
    public List<BaseItem> equipInventory = new List<BaseItem>();

    public EquipslotCtrl[] equippedUiSlot;
    //장비용 툴팁 UI
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



    //현재 가지고있을 인벤토리
    public List<BaseItem> inventory = new List<BaseItem>();

    //사물함 인벤토리
    public List<BaseItem> boxInven = new List<BaseItem>();

    //퀵슬롯 인벤토리
    public List<BaseItem> quickSlotItem = new List<BaseItem>();

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

    

   public void DebugTest()
    {
        if (equippedUiSlot == null || equippedUiSlot.Length == 0)
        {
            Debug.LogError("equippedUiSlot이 연결되지 않았습니다! Inspector에서 확인해주세요.");
        }

        Debug.Log("equippedUiSlot이 연결 잘 되었음.");
    }

    private void Start()
    {
        LoadInventoryFromFirebase();
    }

    

    public void SlotSetting(EquipslotCtrl[] slots)
    {

        // 자동으로 슬롯 UI 배열 초기화
        if (slots == null || slots.Length == 0)
        {
            equippedUiSlot = slots
                .OrderBy(slot => slot.Type)
                .ToArray();
            Debug.Log($"[InvenToryCtrl] equippedUiSlot 자동 초기화 완료 ({equippedUiSlot.Length}개)");
        }
    }

    public async void SaveInventoryToFirebase()
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null) return;

        // 인벤토리가 null이거나 비어있으면 기본값으로 초기화
        if (inventory == null || inventory.Count == 0)
            InvenInit(inventory, (int)InvenSize.Inventory);
        if (boxInven == null || boxInven.Count == 0)
            InvenInit(boxInven, (int)InvenSize.BoxInven);
        if (equipInventory == null || equipInventory.Count == 0)
            InvenInit(equipInventory, (int)InvenSize.EquipInven);
        if (equippedInventory == null || equippedInventory.Count == 0)
            InvenInit(equippedInventory, (int)InvenSize.EquipedInven);

        //인벤토리별 저장
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

        //슬롯세팅
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

        Debug.Log("인벤토리 저장 완료");
    }

    public async void LoadInventoryFromFirebase()
    {
        var user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null) return;

        DatabaseReference root = FirebaseDatabase.DefaultInstance.RootReference;

        // 1. inventory 리스트 불러오기
        var inventoryData = await root.Child(user.UserId).Child("inventoryData").GetValueAsync();
        if (inventoryData.Exists)
            InvenToryCtrl.Instance.inventory = ParseCSVToItemList(inventoryData.Value.ToString(), (int)InvenSize.Inventory);

        // 2. boxInven 리스트 불러오기
        var boxData = await root.Child(user.UserId).Child("BoxInvenData").GetValueAsync();
        if (boxData.Exists)
            InvenToryCtrl.Instance.boxInven = ParseCSVToItemList(boxData.Value.ToString(), (int)InvenSize.BoxInven);

        // 3. equipInventory 리스트 불러오기
        var equipData = await root.Child(user.UserId).Child("EquipInventoryData").GetValueAsync();
        if (equipData.Exists)
            InvenToryCtrl.Instance.equipInventory = ParseCSVToItemList(equipData.Value.ToString(), (int)InvenSize.EquipInven);

        // 4. equippedInventory 슬롯별 불러오기
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
             Debug.Log("인벤토리 로드 완료");
        else
        {
            Debug.Log("??? 뭔가잘못됨");
        }
    }


    // CSV 파싱 함수
    List<BaseItem> ParseCSVToItemList(string csv, int targetSize)
    {
        var result = new List<BaseItem>();
        var lines = csv.Split('\n');

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // [1,5] 형태를 처리
            string trimmed = line.Trim(' ', '[', ']', '\r'); // 공백, 괄호 제거
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

        // 부족한 슬롯은 Empty로 채움
        while (result.Count < targetSize)
        {
            result.Add(ItemDataBase.Instance.EmptyItem.Clone());
        }

        return result;
    }


    //단일 아이템 불러오기 함수
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

        //장비인벤 마지막은 박스
        for (int i = 0; i < equippedInventory.Count - 1; i++)
        {
            OnEquippedChanged?.Invoke(equippedInventory[i].id);
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
            list.Add(ItemDataBase.Instance.EmptyItem);
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
            Debug.LogWarning("[장착 실패] 적절한 슬롯을 찾을 수 없습니다.");
            return false;
        }

        var targetSlot = equippedUiSlot[slotIndex];
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
            list.Add(ItemDataBase.Instance.EmptyItem);
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


    public void LoadQuickSlotItemsFromInventory()
    {
        quickSlotItem.Clear(); // 기존 퀵슬롯 초기화

        foreach (var item in inventory)
        {
            if (item == null || item.id == ItemName.Empty) continue;

            // 조건: trap, meat, position 이름 포함 아이템만 허용
            string name = item.name.ToLower();

            if (name.Contains("trap") || 
                name.Contains("meat") || 
                name.Contains("position"))
            {
                quickSlotItem.Add(item.Clone());
            }
        }

        Debug.Log($"퀵슬롯에 {quickSlotItem.Count}개의 아이템을 불러왔습니다.");
    }

}
