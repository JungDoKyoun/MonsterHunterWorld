using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemToolTipCtrl : MonoBehaviour
{
    //참조해올 오브젝트들
    BaseItem item;
    [SerializeField]
    GameObject image;
    [SerializeField]
    GameObject itemName;
    [SerializeField]
    GameObject toolTip;
    [SerializeField]
    GameObject rarity;    
    [SerializeField]
    GameObject sellGold;
    
    //적용시킬 값들
    Text itemNameText;
    Text tooltipText;
    Text rarityText;
    Text goldText;
    Text countText;
    Text maxCountText;
    Text allCountText;
    Image itemImage;


    private void Awake()
    {
        //겟컴포넌트를 최소화 하기위한 일.
        itemNameText = itemName.GetComponent<Text>();
        tooltipText = toolTip.GetComponent<Text>();
        rarityText = rarity.GetComponent<Text>();

        itemImage = image.GetComponent<Image>();
        //초기화 - 처음엔 아무것도 없으니까 비활성화
        TooltipClear(false);
    }

    public void TooltipClear(bool set)
    {
        //아무것도 없는 아이템이면 비활성화
        image.SetActive(set);
        itemName.SetActive(set);
        toolTip.SetActive(set);
        rarity.SetActive(set);
        sellGold.SetActive(set);

    }

    public void SetItem(BaseItem item)
    {
        this.item = item;
        if (item == null)
        {
            TooltipClear(false);
            return;
        }
        itemNameText.text = item.name;
        tooltipText.text = item.tooltip;
        rarityText.text = item.rarity.ToString();
        goldText.text = item.price.ToString();
        countText.text = item.count.ToString();
        maxCountText.text = item.maxCount.ToString();
        allCountText.text = item.allCount.ToString();
        itemImage.sprite = item.image;
        TooltipClear(true);
    }
}
