using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

//장비 인벤토리 UI
public class EquipInventoryUI : BaseInventory
{
    //[SerializeField] GameObject slotParent;

    public bool IsOpen => gameObject.activeSelf;

    private void Start()
    {

        invenType = InvenType.EquipBox;

        SlotSetting(parrentObj, invenType);
        items = InvenToryCtrl.Instance.equipInventory;

        //InvenInit(items);

        GetItemToInventory(items,ItemDataBase.Instance.GetItem(ItemName.HuntersKnife_I));
        GetItemToInventory(items,ItemDataBase.Instance.GetItem(ItemName.LeatherVest));


        InvenToryCtrl.Instance.OnInventoryChanged += () =>
        {
            RefreshUI(items);
        };

        //Debug.Log(items.Count);

        //가지고있는 아이템이 있는경우
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
            }
        }
    }

    private void OnEnable()
    {
        StartCoroutine(DelayedRefresh());
    }

    IEnumerator DelayedRefresh()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshUI(items); 
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

}
