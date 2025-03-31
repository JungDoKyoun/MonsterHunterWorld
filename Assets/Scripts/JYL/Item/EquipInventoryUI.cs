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
