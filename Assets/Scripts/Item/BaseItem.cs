using UnityEngine;
using UnityEngine.UI;

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
    Setup,Throw
     
}

public class BaseItem
{
    public Sprite image;

    public string name;
    public ItemType type;
    public string rarity;
    public int count;//현재 들고있는 갯수
    public int maxCount;//최대한 들고있을수 있는 갯수
    public int allCount;//가지고있는 총 갯수  
    public Color color;
    
    public string tooltip;
    public int price;

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




