using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//������ �κ� UI
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
            Debug.Log("�ε��� �߸��־����ϴ�.");
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
    //            Debug.LogWarning($"���� {i}���� ItemSlot ������Ʈ�� �����ϴ�.");
    //            continue;
    //        }
    //        if (items[i] != null)
    //        {
    //            slotComp.SlotSetItem(items[i]);
    //        }
    //    }
    //}

}
