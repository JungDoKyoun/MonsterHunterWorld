using UnityEngine;

//장착중 인벤 UI
public class EquippedInventoryUI : BaseInventory
{
    public EquipslotCtrl[] equipSlot;

    private void Awake()
    {
        invenType = InvenType.Equipped;
        //// 리스트 크기 보정 (슬롯 수와 같게)
        //EnsureEquipListSize();
    }

    private void Start()
    {


        for (int i = 0; i < equipSlot.Length; i++)
        {
            equipSlot[i].SetType((EquipSlot)i);
        }
        InvenToryCtrl.Instance.equippedUiSlot = equipSlot;

        Debug.Log(equipSlot.Length);

        // 초기 세팅
        RefreshUI();

    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.equippedInventory;
        RefreshUI();

        UIManager.Instance.StackUIOpen(UIType.EquipInfoUI);

        // 변화 감지 이벤트 연결
        InvenToryCtrl.Instance.OnInventoryChanged += RefreshUI;

    }

    private void OnDisable()
    {
        UIManager.Instance.CloseAll();
        InvenToryCtrl.Instance.OnInventoryChanged -= RefreshUI;
        InvenToryCtrl.Instance.SaveInventoryToFirebase();

        foreach (var item in items)
        {
            Debug.Log(item.name);
        }

    }

    // UI 갱신
    public void RefreshUI()
    {
        for (int i = 0; i < equipSlot.Length - 1; i++)
        {

            if (equipSlot[i] == null)
            {
                Debug.LogWarning($"[EquippedInventoryUI] {i}번 슬롯에 EquipslotCtrl 없음");
                continue;
            }

            var item = (i < items.Count && items[i] != null) ? items[i] : ItemDataBase.Instance.emptyItem;


            equipSlot[i].SlotListSetting(item);
        }
    }



    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
