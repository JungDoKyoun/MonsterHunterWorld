using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//장착중 인벤 UI
public class EquippedInventoryUI : BaseInventory
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
}
