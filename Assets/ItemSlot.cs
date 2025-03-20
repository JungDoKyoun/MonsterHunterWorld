using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public BaseItem item;
    public GameObject itemImage;
    public Text countText;

    //void Start()
    //{
    //    var img = GetComponentInChildren<Image>();
    //    img.sprite = item.image.sprite;
    //}

    public void SetItem(BaseItem item)
    {
        this.item = item;
        var img = itemImage.GetComponent<Image>();
        Debug.Log("1" + item.image.name);
        Debug.Log("2" + img.name);
        //img = item.image;
        img.sprite = item.image;
        img.color = item.color;

        if (item.count > 0)
        {
            countText.gameObject.SetActive(true);
            countText.text = item.count.ToString();
        }
        else
        {
            countText.gameObject.SetActive(false);
        }
    }
}
