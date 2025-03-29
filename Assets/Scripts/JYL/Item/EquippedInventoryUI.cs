using UnityEngine;

//장착중 인벤 UI
public class EquippedInventoryUI : BaseInventory
{
    public GameObject[] equipSlot;
    public bool IsOpen => gameObject.activeSelf;

    private void Start()
    {
        invenType = InvenType.Equipped;



        items = InvenToryCtrl.Instance.equippedInventory;

        // 리스트 크기 보정 (슬롯 수와 같게)
        EnsureEquipListSize();

        // 변화 감지 이벤트 연결
        InvenToryCtrl.Instance.OnEquippedChanged += RefreshUI;

        // 초기 세팅
        RefreshUI();

    }

    // 리스트 크기가 부족하면 빈 슬롯으로 채움
    private void EnsureEquipListSize()
    {
        int targetCount = equipSlot.Length;

        while (items.Count < targetCount)
        {
            items.Add(ItemDataBase.Instance.emptyItem);
        }
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



    public void EquipItem(BaseItem value)
    {
        var index = (int)value.GetEquipSlot();

        if (index < 0 || index >= equipSlot.Length || value.name == "")
        {
            Debug.Log("값이 잘못됬습니다.");
            return;
        }

        equipSlot[index].GetComponent<EquipslotCtrl>().SlotListSetting(value);
        //items[index] = value;
    }


    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
