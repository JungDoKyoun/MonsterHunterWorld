using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipItemToolTipCtrl : MonoBehaviour
{

    //�����ؿ� ������Ʈ��
    [Header("���� ����")]
    [SerializeField]
    Image image;
    [SerializeField]
    Text itemName;
    [SerializeField]
    Text toolTip;
    [SerializeField]
    Text sellGold;

    [Header("���� ����")]
    [SerializeField]
    GameObject weaponObj;
    [SerializeField]
    Text weaponRarity;
    [SerializeField]
    Text damage;
    [SerializeField]
    Text attribute;
    
    [Header("�� ����")]
    [SerializeField]
    GameObject ArmorObj;
    [SerializeField]
    Text armorRarity;
    [SerializeField]
    Text level;
    [SerializeField]
    Text defense;
    [SerializeField]
    Text fireDef;
    [SerializeField]
    Text waterDef;
    [SerializeField]
    Text LightningDef;
    [SerializeField]
    Text IceDef;
    [SerializeField]
    Text DragonDef;


    private void Awake()
    {
        //�ʱ�ȭ - ó���� �ƹ��͵� �����ϱ� ��Ȱ��ȭ
        TooltipClear(false,ItemType.Empty);
    }

    public void TooltipClear(bool set ,ItemType type)
    {
        image.gameObject.SetActive(set);
        itemName.gameObject.SetActive(set);
        toolTip.gameObject.SetActive(set);
        sellGold.gameObject.SetActive(set);

        //����
        if(type == ItemType.Weapon)
        {
            weaponObj.SetActive(set);
            damage.gameObject.SetActive(set);
            weaponRarity.gameObject.SetActive(set);
            attribute.gameObject.SetActive(set);
        }        

        //��
        else if (type == ItemType.Armor)
        {
            ArmorObj.SetActive(set);
            armorRarity.gameObject.SetActive(set);
            level.gameObject.SetActive(set);
            defense.gameObject.SetActive(set);
            fireDef.gameObject.SetActive(set);
            waterDef.gameObject.SetActive(set);
            LightningDef.gameObject.SetActive(set);
            IceDef.gameObject.SetActive(set);
            DragonDef.gameObject.SetActive(set);
        }
        
    }

    public void SetWeapon(Weapon item)
    {
        if (item == null)
        {
            TooltipClear(false, ItemType.Weapon);
            return;
        }
        image.sprite = item.image;
        itemName.text = item.name;
        toolTip.text = item.tooltip;
        weaponRarity.text = item.rarity.ToString();
        sellGold.text = item.price.ToString();

        damage.text = item.damage.ToString();
        attribute.text = item.attribute.ToString();



        TooltipClear(true,ItemType.Weapon);
    }

    public void SetArmor(Armor item)
    {
        if (item == null)
        {
            TooltipClear(false,ItemType.Armor);
            return;
        }

        image.sprite = item.image;
        itemName.text = item.name;
        armorRarity.text = item.rarity.ToString();
        toolTip.text = item.tooltip;
        
        defense.text = item.defense.ToString();
        level.text = item.level.ToString();
        fireDef.text = item.fireDef.ToString();
        waterDef.text = item.waterDef.ToString();
        LightningDef.text = item.LightningDef.ToString();
        IceDef.text = item.IceDef.ToString();
        DragonDef.text = item.DragonDef.ToString();




        sellGold.text = item.price.ToString();

        TooltipClear(true, ItemType.Armor);

    }
}
