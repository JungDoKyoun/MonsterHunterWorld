using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemToolTipCtrl : MonoBehaviour
{
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
    GameObject gold;
    [SerializeField]
    GameObject count;
    [SerializeField]
    GameObject maxCount;
    [SerializeField]
    GameObject allCount;

    public void SetItem(BaseItem value)
    {
        item = value;
        itemName.GetComponent<Text>().text = item.name;
        toolTip.GetComponent<Text>().text = item.tooltip;
        image.GetComponent<Image>().sprite = item.image;
        image.GetComponent<Image>().color = item.color;
        rarity.GetComponent<Text>().text = item.rarity.ToString();
        gold.GetComponent<Text>().text = item.price.ToString();
        count.GetComponent<Text>().text = item.count.ToString();
        maxCount.GetComponent<Text>().text = item.maxCount.ToString();
        allCount.GetComponent<Text>().text = item.allCount.ToString();
    }

}
