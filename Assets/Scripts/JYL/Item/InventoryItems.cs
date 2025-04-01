using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//현재 가지고있을 아이템 인벤토리
public class InventoryItems : BaseInventory
{
    public GameObject upSlot;
    public GameObject downSlot;

    public bool IsOpen => gameObject.activeSelf;

    private void Awake()
    {
        invenType = InvenType.Inven;

        //슬롯 세팅
        SlotSetting(upSlot, invenType);
    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.inventory;

        StartCoroutine(DelayedRefresh());


        //인벤 변경시 UI 초기화
        InvenToryCtrl.Instance.OnInventoryChanged += () =>
        {
            RefreshUI(items);
        };

    }

    void OnDisable()
    {
        //인벤 변경시 UI 초기화
        InvenToryCtrl.Instance.OnInventoryChanged -= () =>
        {
            RefreshUI(items);
        };

    }


    IEnumerator DelayedRefresh()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshUI(items);
    }


}
