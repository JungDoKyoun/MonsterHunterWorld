using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemToolTipCtrl : MonoBehaviour
{
    //�����ؿ� ������Ʈ��
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
    
    //�����ų ����
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
        //��������Ʈ�� �ּ�ȭ �ϱ����� ��.
        itemNameText = itemName.GetComponent<Text>();
        tooltipText = toolTip.GetComponent<Text>();
        rarityText = rarity.GetComponent<Text>();

        itemImage = image.GetComponent<Image>();
        //�ʱ�ȭ - ó���� �ƹ��͵� �����ϱ� ��Ȱ��ȭ
        TooltipClear(false);
    }

    public void TooltipClear(bool set)
    {
        //�ƹ��͵� ���� �������̸� ��Ȱ��ȭ
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
