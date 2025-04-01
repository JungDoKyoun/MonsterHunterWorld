using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    [SerializeField] Text useItemCountText;
    [SerializeField] Image itemImg;
    [SerializeField] Text itemNameText;

    BaseItem item;

    int minIndex = 0;
    int maxIndex = 0;
    int currentIndex = 0;
    
    bool isSelecting = false;
    
    void Start()
    {
        item = InvenToryCtrl.Instance.inventory[0];

        useItemCountText.text = item.count.ToString();
        itemNameText.text = item.name;
        itemImg.sprite = item.image;
        itemImg.color = item.color;

        maxIndex = InvenToryCtrl.Instance.inventory.Count;
    }


    void NextItem(int index)
    {
        if (index < minIndex)
        {
            return;
        }

        currentIndex = index;

        item = InvenToryCtrl.Instance.inventory[currentIndex];
    }

    void PrevItem(int index)
    {
        if (index >= maxIndex)
        {
            return;
        }

        currentIndex = index;

        item = InvenToryCtrl.Instance.inventory[currentIndex];
    }

}
