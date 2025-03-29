using System.Collections;
using UnityEngine;

//현재 가지고있을 아이템 인벤토리
public class InventoryItems : BaseInventory
{
    public GameObject upSlot;
    public GameObject downSlot;

    public bool IsOpen => gameObject.activeSelf;

    void Start()
    {
        invenType = InvenType.Inven;

        //슬롯 세팅
        SlotSetting(upSlot, invenType);

        items = InvenToryCtrl.Instance.inventory;
        //인벤 세팅
        //InvenInit(items);

        //아이템 흭득
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.PitfallTrap));
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        
        InvenToryCtrl.Instance.OnInventoryChanged += () =>
        {
            RefreshUI(items);
        };

        //가지고있는 아이템이 있는경우
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
            }
        }
        //없으면 고대로 하면됨


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


}
