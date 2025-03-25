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
public enum Attribute
{
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

}

public class Weapon : BaseItem
{
    public int damage;
    public Attribute attribute;
}

public class Armor : BaseItem
{
    public int defense;
    public Attribute attribute;
}

public class Potion : BaseItem
{
    public int heal = 0;
    public int maxHeal;
    public int stamina = 0;
    public int maxStamina;
}

public class Trap : BaseItem
{
    public GameObject trap;
    public TrapType trapType;
}




