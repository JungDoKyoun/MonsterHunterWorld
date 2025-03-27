using System.Collections;
using UnityEngine;

//현재 가지고있을 아이템 인벤토리
public class InventoryItems : BaseInventory, IClosableUI
{
    public GameObject upSlot;
    public GameObject downSlot;
    
    public bool IsOpen => gameObject.activeSelf;

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
        UIManager.Instance.RegisterUI(this);
    }

    private void OnDisable()
    {
        UIManager.Instance.UnregisterUI(this);
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
