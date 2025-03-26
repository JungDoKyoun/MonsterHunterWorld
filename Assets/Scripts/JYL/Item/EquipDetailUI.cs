using UnityEngine;
using UnityEngine.UI;

public class EquipDetailUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text nameText;
    [SerializeField] private Text rarityText;
    [SerializeField] private Text statText;
    [SerializeField] private Text attributeText;
    [SerializeField] private Text tooltipText;
    [SerializeField] private Text priceText;

    public void ShowDetail(BaseItem item)
    {
        icon.sprite = item.image;
        icon.color = item.color;
        nameText.text = item.name;
        rarityText.text = item.rarity;
        tooltipText.text = item.tooltip;
        priceText.text = $"{item.price} G";

        switch (item)
        {
            case Weapon weapon:
                statText.text = $"傍拜仿: {weapon.damage}";
                attributeText.text = $"加己: {weapon.attribute}";
                break;

            case Armor armor:
                statText.text = $"规绢仿: {armor.defense}";
                attributeText.text = $"加己: {armor.attribute}";
                break;

            default:
                statText.text = "-";
                attributeText.text = "-";
                break;
        }
    }

    public void Clear()
    {
        icon.sprite = null;
        icon.color = Color.clear;
        nameText.text = "";
        rarityText.text = "";
        statText.text = "";
        attributeText.text = "";
        tooltipText.text = "";
        priceText.text = "";
    }
}
