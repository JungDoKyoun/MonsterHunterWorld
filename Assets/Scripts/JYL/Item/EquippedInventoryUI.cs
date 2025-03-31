using UnityEngine;

//장착중 인벤 UI
public class EquippedInventoryUI : BaseInventory
{
    public GameObject[] equipSlot;

    private void Awake()
    {
        invenType = InvenType.Equipped;
        //// 리스트 크기 보정 (슬롯 수와 같게)
        //EnsureEquipListSize();
    }
    private void Start()
    {

        // 변화 감지 이벤트 연결
        InvenToryCtrl.Instance.OnInventoryChanged += RefreshUI;

        for (int i = 0; i < equipSlot.Length; i++)
        {
            var slotCtrl = equipSlot[i].GetComponent<EquipslotCtrl>();
            slotCtrl.SetType((EquipSlot)i);
        }

        // 초기 세팅
        RefreshUI();

    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.equippedInventory;
        RefreshUI();

        UIManager.Instance.StackUIOpen(UIType.EquipInfoUI);  
    }

    private void OnDisable()
    {
        UIManager.Instance.CloseAll();

    }

    // UI 갱신
    public void RefreshUI()
    {
        for (int i = 0; i < equipSlot.Length; i++)
        {
            var slotCtrl = equipSlot[i].GetComponent<EquipslotCtrl>();
            if (slotCtrl == null)
            {
                Debug.LogWarning($"[EquippedInventoryUI] {i}번 슬롯에 EquipslotCtrl 없음");
                continue;
            }

            var item = (i < items.Count && items[i] != null)
                ? items[i]
                : ItemDataBase.Instance.emptyItem;

            
            slotCtrl.SlotListSetting(item);
        }
    }



    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
