using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//장착중 인벤 UI
public class EquippedInventoryUI : BaseInventory
{
    public GameObject[] equipSlot;
    private void Start()
    {
        invenType = InvenType.Equipped;

        SlotSetting(gameObject, invenType);
        InvenInit();
    }
    public void EquipItem(BaseItem item, int index)
    {
        if (index < 0 || index >= equipSlot.Length)
        {
            Debug.Log("인덱스 잘못넣었습니다.");
            return;
        }
        items[index] = item;
    }

    //public void RefreshUI()
    //{
    //    for (int i = 0; i < equipSlot.Length; i++)
    //    {
    //        var slotComp = equipSlot[i].GetComponent<ItemSlot>();
    //        if (slotComp == null)
    //        {
    //            Debug.LogWarning($"슬롯 {i}번에 ItemSlot 컴포넌트가 없습니다.");
    //            continue;
    //        }
    //        if (items[i] != null)
    //        {
    //            slotComp.SlotSetItem(items[i]);
    //        }
    //    }
    //}

}
