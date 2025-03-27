using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipslotCtrl : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text itemName;

 
    public void SlotListSetting(BaseItem item)
    {
        icon.sprite = item.image;
        icon.color = item.color;
        itemName.text = item.name;
    }

}
