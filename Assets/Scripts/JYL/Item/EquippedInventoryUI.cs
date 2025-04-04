using UnityEngine;

//장착중 인벤 UI
public class EquippedInventoryUI : BaseInventory
{
    public EquipslotCtrl[] equipSlot;

    private void Awake()
    {
        invenType = InvenType.Equipped;
    }

    private void Start()
    {
        for (int i = 0; i < equipSlot.Length; i++)
        {
            equipSlot[i].SetType((EquipSlot)i);
        }

        InvenToryCtrl.Instance.equippedUiSlot = equipSlot;


        //// 초기 세팅
        RefreshUI();

        //InvenToryCtrl.Instance.DebugTest();
    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.equippedInventory;

        UIManager.Instance.StackUIOpen(UIType.EquipInfoUI);

        // 변화 감지 이벤트 연결
        InvenToryCtrl.Instance.OnInventoryChanged += RefreshUI;

        RefreshUI();
    }

    private void OnDisable()
    {
        UIManager.Instance.CloseAll();
        InvenToryCtrl.Instance.OnInventoryChanged -= RefreshUI;
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

            var item = (i < items.Count && items[i] != null) ? items[i] : ItemDataBase.Instance.EmptyItem;


            equipSlot[i].SlotListSetting(item);
        }
    }



    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
