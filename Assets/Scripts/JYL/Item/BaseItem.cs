using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Potion,
    Trap,
    Empty,
    All
}
public enum EquipSlot
{
    Weapon,
    Head,
    Chest,
    Arms,
    Waist,
    Legs
}


public enum Attribute
{
    empty,//무속성
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
    public ItemImageNumber key;
    public string name;
    public ItemType type;
    public string rarity;
    public int count;//현재 들고있는 갯수
    public int maxCount;//최대한 들고있을수 있는 갯수
    public int allCount;//가지고있는 총 갯수  
    public Color color;

    public string tooltip;
    public int price;

    public virtual BaseItem Clone()
    {
        return new BaseItem
        {
            image = this.image,
            key = this.key,
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

    //EquipSlot? (nullable)로 하면 장비가 아닌 아이템은 null로 처리 가능
    public virtual EquipSlot? GetEquipSlot()
    {
        return null; // 기본은 장착 불가
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
            key = this.key,
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
    public int defense;
    public Attribute attribute;
    public EquipSlot equipType;
    public override EquipSlot? GetEquipSlot()
    {
        return equipType; // Armor에 이미 정의된 장착 부위
    }

    public override BaseItem Clone()
    {
        return new Armor
        {
            image = this.image,
            key = this.key,
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
            attribute = this.attribute
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
            key = this.key,
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
            key = this.key,
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





