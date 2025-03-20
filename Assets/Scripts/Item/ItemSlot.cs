using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    BaseItem item;
    public BaseItem Item => item;

    public GameObject itemImage;
    public Text countText;

    public void SetItem(BaseItem value)
    {
        item = value;
        var img = itemImage.GetComponent<Image>();

        Debug.Log(img.name);
        Debug.Log(1 +". " + item.name);
        Debug.Log(img.color);
        Debug.Log(value.color);

        //Debug.Log(img.material.color);
        //img.material.color = value.color; // 기본 머티리얼 제거
        img.sprite = value.image;
        img.color = value.color;
        
        Debug.Log(2 + ". " + item.name);
        Debug.Log(img.color);
        Debug.Log(value.color);

        if (item.count > 0)
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
