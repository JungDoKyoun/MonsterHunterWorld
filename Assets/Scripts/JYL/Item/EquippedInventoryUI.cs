using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//장착중 인벤 UI
public class EquippedInventoryUI : BaseInventory, IClosableUI
{
    public GameObject[] equipSlot;
    public bool IsOpen => gameObject.activeSelf;

    private void Start()
    {
        invenType = InvenType.Equipped;

        SlotSetting(gameObject, invenType);
        InvenInit();
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

    private void OnEnable()
    {
        RefreshUI();
        //UIManager.Instance.RegisterUI(this);
    }

    private void OnDisable()
    {
        //UIManager.Instance.UnregisterUI(this);
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
