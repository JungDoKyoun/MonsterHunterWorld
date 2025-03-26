using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedInventoryUI : MonoBehaviour
{
    [SerializeField] private List<Text> slotTexts;
    [SerializeField] private List<Image> highlights;
    [SerializeField] private EquipDetailUI detailUI;

    private List<BaseItem> equippedItems = new List<BaseItem>();
    private int selectedIndex = 0;

    public void SetEquipped(List<BaseItem> equips)
    {
        equippedItems = equips;
        for (int i = 0; i < slotTexts.Count; i++)
            slotTexts[i].text = equips[i]?.name ?? "¾øÀ½";

        selectedIndex = 0;
        UpdateUI();
    }

    public void Move(int dir)
    {
        selectedIndex = Mathf.Clamp(selectedIndex + dir, 0, slotTexts.Count - 1);
        UpdateUI();
    }

    private void UpdateUI()
    {
        for (int i = 0; i < highlights.Count; i++)
            highlights[i].enabled = (i == selectedIndex);

        var item = equippedItems[selectedIndex];
        if (item != null && item.type != ItemType.Empty)
            detailUI.ShowDetail(item);
        else
            detailUI.Clear();
    }

    public int GetSelectedIndex() => selectedIndex;
    public BaseItem GetSelectedItem() => equippedItems[selectedIndex];
}
