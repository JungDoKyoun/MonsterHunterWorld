using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private CSVItemLoader itemLoader; // CSV���� �ε��� ������Ʈ
    [SerializeField] private GameObject itemSlotPrefab; // ������ ������ ���� ������
    [SerializeField] private Transform contentPanel; // ������ ���� �θ� ������Ʈ

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
