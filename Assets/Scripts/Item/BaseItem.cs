using UnityEngine;
using UnityEngine.UI;

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

public enum TrapType
{
    Setup,Throw
     
}

public class BaseItem
{
    public Sprite image;

    public string name;
    public ItemType type;
    public int rarity;
    public int count;
    public int maxCount;

    public Color color;

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


