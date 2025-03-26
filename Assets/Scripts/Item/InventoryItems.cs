using System.Collections;
using UnityEngine;

//현재 가지고있을 아이템 인벤토리
public class InventoryItems : BaseInventory
{
    public GameObject upSlot;
    public GameObject downSlot;

    void Start()
    {
        invenType = InvenType.Inven;

        //슬롯 세팅
        SlotSetting(upSlot, invenType);
        //인벤 세팅
        InvenInit();

        //아이템 흭득
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.RecoveryPotion));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.RecoveryPotion));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.WellCookedMeat));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.VineTrap));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.WellCookedMeat));

        //가지고있는 아이템이 있는경우
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
                Debug.Log(items[i].name + "이거있따아아아아아아아아앙아아아아아아아아");
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
        RefreshUI();
    }

}
