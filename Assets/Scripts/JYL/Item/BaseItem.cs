using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Accessory,
    Potion,
    Trap,
    Empty
}
public enum EquipSlot
{
    Weapon,
    Head,
    Chest,
    Arms,
    Waist,
    Legs,
    band,
    neck,
    end

}


public enum Attribute
{
    empty,//���Ӽ�
    Fire,
    Water,
    Lightning,
    Ice,
    Dragon
}

public enum TrapType
{
    Setup, Throw

}

public class BaseItem
{
    public Sprite image;
    public ItemName id;
    public string name;
    public ItemType type;
    public string rarity;
    public int count;//���� ����ִ� ����
    public int maxCount;//�ִ��� ��������� �ִ� ����
    public int allCount;//�������ִ� �� ����  
    public Color color;

    public string tooltip;
    public int price;

    public virtual BaseItem Clone()
    {
        return new BaseItem
        {
            image = this.image,
            id = this.id,
            name = this.name,
            type = this.type,
            rarity = this.rarity,
            count = this.count,
            maxCount = this.maxCount,
            allCount = this.allCount,
            color = this.color,
            tooltip = this.tooltip,
            price = this.price
        };
    }

    //EquipSlot? (nullable)�� �ϸ� ��� �ƴ� �������� null�� ó�� ����
    public virtual EquipSlot? GetEquipSlot()
    {
        return null; // �⺻�� ���� �Ұ�
    }

}

public class Weapon : BaseItem
{
    public int damage;
    public Attribute attribute;
    public override EquipSlot? GetEquipSlot()
    {
        return EquipSlot.Weapon;
    }

    public override BaseItem Clone()
    {
        return new Weapon
        {
            image = this.image,
            id = this.id,
            name = this.name,
            type = this.type,
            rarity = this.rarity,
            count = this.count,
            maxCount = this.maxCount,
            allCount = this.allCount,
            color = this.color,
            tooltip = this.tooltip,
            price = this.price,
            damage = this.damage,
            attribute = this.attribute
        };
    }
}
public class Armor : BaseItem
{
    public EquipSlot equipType;
    public int defense;
    public int level;//��ȭ ����
    public int fireDef;
    public int waterDef;
    public int LightningDef;
    public int IceDef;
    public int DragonDef;

    public override EquipSlot? GetEquipSlot()
    {
        return equipType; // Armor�� �̹� ���ǵ� ���� ����
    }

    public override BaseItem Clone()
    {
        return new Armor
        {
            image = this.image,
            id = this.id,
            name = this.name,
            type = this.type,
            equipType = this.equipType,
            rarity = this.rarity,
            count = this.count,
            maxCount = this.maxCount,
            allCount = this.allCount,
            color = this.color,
            tooltip = this.tooltip,
            price = this.price,
            defense = this.defense,
            level = this.level,
            fireDef = this.fireDef,
            waterDef = this.waterDef,
            LightningDef = this.LightningDef,
            IceDef = this.IceDef,
            DragonDef = this.DragonDef
        };

    }
}

public class Potion : BaseItem
{
    public int heal = 0;
    public int maxHeal;
    public int stamina = 0;
    public int maxStamina;

    public override BaseItem Clone()
    {
        return new Potion
        {
            image = this.image,
            id = this.id,
            name = this.name,
            type = this.type,
            rarity = this.rarity,
            count = this.count,
            maxCount = this.maxCount,
            allCount = this.allCount,
            color = this.color,
            tooltip = this.tooltip,
            price = this.price,
            heal = this.heal,
            maxHeal = this.maxHeal,
            stamina = this.stamina,
            maxStamina = this.maxStamina
        };
    }
}


public class Trap : BaseItem
{
    public GameObject trap;
    public TrapType trapType;


    public override BaseItem Clone()
    {
        return new Trap
        {
            trap = this.trap,
            image = this.image,
            id = this.id,
            name = this.name,
            type = this.type,
            rarity = this.rarity,
            count = this.count,
            maxCount = this.maxCount,
            allCount = this.allCount,
            color = this.color,
            tooltip = this.tooltip,
            price = this.price,
            trapType = this.trapType

        };
    }

}





