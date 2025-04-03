using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private CSVItemLoader itemLoader; // CSV에서 로드할 컴포넌트
    [SerializeField] private GameObject itemSlotPrefab; // 상점용 아이템 슬롯 프리팹
    [SerializeField] private Transform contentPanel; // 슬롯을 붙일 부모 오브젝트

    public List<BaseItem> shopItems = new List<BaseItem>();

    void Start()
    {
        LoadShopItems();
        ShopUiSetItem();
    }

    private void OnEnable()
    {        
        InvenToryCtrl.Instance.IsShop = true;
    }

    private void OnDisable()
    {
        InvenToryCtrl.Instance.IsShop = false;
    }

    void LoadShopItems()
    {
        shopItems = itemLoader.LoadItemsFromCSV();
    }

    void ShopUiSetItem()
    {
        foreach (var item in shopItems)
        {
            GameObject slot = Instantiate(itemSlotPrefab, contentPanel);
            ShopItemSlot slotUI = slot.GetComponent<ShopItemSlot>();
            slotUI.SetItem(item);
        }
    }


}
