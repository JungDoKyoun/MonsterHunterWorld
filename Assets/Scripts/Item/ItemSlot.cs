using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    //������ �̹��� 1�� 
    //������ �ƴ� �̹��� 0��
    [SerializeField]
    Sprite[] sprites = new Sprite[2];

    BaseItem item;
    public BaseItem Item => item;

    public GameObject itemImage;
    public Text countText;



    public void SetItem(BaseItem value)
    {
        item = value;
        var img = itemImage.GetComponent<Image>();

        //Debug.Log(img.name);
        //Debug.Log(1 +". " + item.name);
        //Debug.Log(img.color);
        //Debug.Log(value.color);

        //Debug.Log(img.material.color);
        //img.material.color = value.color; // �⺻ ��Ƽ���� ����
        img.sprite = item.image;
        img.color = item.color;
        
        //Debug.Log(2 + ". " + item.name);
        //Debug.Log(img.color);
        //Debug.Log(value.color);

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

    public void OnPointerClick(PointerEventData eventData)
    {
        
        var tooltipBox = GameObject.Find("ItemTooltipBox").GetComponent<ItemToolTipCtrl>();

        tooltipBox.SetItem(item);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
