using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipInventoryUI : MonoBehaviour
{
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Text pageText;
    [SerializeField] private EquipDetailUI detailUI;

    private List<BaseItem> allEquipItems;
    private List<ItemSlot> slots = new();
    private int itemsPerPage = 20;
    private int currentPage = 0;

    public void Open(List<BaseItem> allItems)
    {
        allEquipItems = allItems.FindAll(i => i.type == ItemType.Weapon || i.type == ItemType.Armor);
        currentPage = 0;
        RefreshPage();
    }

    public void RefreshPage()
    {
        foreach (var slot in slots)
            Destroy(slot.gameObject);
        slots.Clear();

        int start = currentPage * itemsPerPage;
        int end = Mathf.Min(start + itemsPerPage, allEquipItems.Count);

        for (int i = start; i < end; i++)
        {
            var go = Instantiate(slotPrefab, gridParent);
            var slot = go.GetComponent<ItemSlot>();
            slot.SlotSetItem(allEquipItems[i]);
            slot.onClick = OnClickItem;
            slots.Add(slot);
        }

        pageText.text = $"{currentPage + 1} / {Mathf.CeilToInt((float)allEquipItems.Count / itemsPerPage)}";
        detailUI.Clear();
    }

    public void OnClickItem(BaseItem item)
    {
        detailUI.ShowDetail(item);
        // 추후 "장착" 키 누르면 장비 전환 처리
    }

    public void NextPage() => SetPage(currentPage + 1);
    public void PrevPage() => SetPage(currentPage - 1);

    private void SetPage(int page)
    {
        currentPage = Mathf.Clamp(page, 0, (allEquipItems.Count - 1) / itemsPerPage);
        RefreshPage();
    }
}
