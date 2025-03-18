using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Potion,
    Trap
}
public enum Attribute
{
    Fire,
    Water,
    Lightning,
    Ice,
    Dragon
}

public class BaseItem
{
    public string name;
    public ItemType type;
    public int rarity;
    public int count;
    public int maxCount;
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
    public int heal;
    public int maxHeal;
}

public class Trap : BaseItem
{
    public GameObject trap;
}


