using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//장비 인벤토리 UI
public class EquipInventoryUI : BaseInventory
{
    private void Awake()
    {
        invenType = InvenType.EquipBox;

        SlotSetting(parrentObj, invenType);
    }

    private void Start()
    {
        InvenToryCtrl.Instance.OnInventoryChanged += () =>
        {
            RefreshUI(items);
        };       
    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.equipInventory;

        StartCoroutine(DelayedRefresh());

        UIManager.Instance.StackUIOpen(UIType.EquipInfoUI);
    }
    private void OnDisable()
    {
        UIManager.Instance.CloseAll();

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
