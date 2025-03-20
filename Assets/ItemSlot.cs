using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    BaseItem item;


    public GameObject itemImage;
    public Text countText;

    public BaseItem Item => item;

    //void Start()
    //{
    //    var img = GetComponentInChildren<Image>();
    //    img.sprite = item.image.sprite;
    //}

    public void SetItem(BaseItem value)
    {
        item = value;
        var img = itemImage.GetComponent<Image>();
        //Debug.Log("1" + value.image.name);
        //Debug.Log("2" + img.name);

        Debug.Log(1);
        Debug.Log(img.color);
        Debug.Log(value.color);
        
        img.sprite = value.image;
        img.color = value.color;
        
        Debug.Log(2);
        Debug.Log(img.color);
        Debug.Log(value.color);

        if (value.count > 0)
        {
            countText.gameObject.SetActive(true);
            countText.text = value.count.ToString();
        }
        else
        {
            countText.gameObject.SetActive(false);
        }
    }
}
