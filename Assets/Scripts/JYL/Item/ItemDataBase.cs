using System.Collections.Generic;
using UnityEngine;

public enum ItemImageNumber
{
    HunterKnife,
    HunterArmor,
    RecoveryPotion,
    WellCookedMeat,
    VineTrap,
    Empty
}

public class ItemDataBase : MonoBehaviour
{
    [SerializeField]
    List<Sprite> itemImages = new List<Sprite>(); // ��������Ʈ ����Ʈ

    [SerializeField]
    List<GameObject> trapItemObj = new List<GameObject>();

    public List<BaseItem> items = new List<BaseItem>();
    public Dictionary<ItemImageNumber, BaseItem> itemDB = new Dictionary<ItemImageNumber, BaseItem>();


    public BaseItem emptyItem;

    public static ItemDataBase Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (itemImages.Count <= (int)ItemImageNumber.Empty ||
            itemImages[(int)ItemImageNumber.Empty] == null)
        {
            Debug.LogError("Empty ���Կ� �̹����� ����ֽ��ϴ�.");
        }

        //��������Ʈ none �λ����ΰŷ� �ʱ�ȭ
        emptyItem = new BaseItem
        {
            image = itemImages[(int)ItemImageNumber.Empty],
            key = ItemImageNumber.Empty,
            name = "",
            type = ItemType.Empty,
            rarity = "  ",
            count = 0,
            maxCount = 0,
            color = new Color32(255, 255, 255, 0),
            tooltip = "",
            price = 0
        };

        //Debug.Log(emptyItem.name);
        //Debug.Log(emptyItem.image.name);
        //Debug.Log(emptyItem.color);


        itemDB.Add(ItemImageNumber.Empty, emptyItem);


        //������ ����Ʈ ������ ����

        items.Add(new Weapon
        {
            image = itemImages[(int)ItemImageNumber.HunterKnife],
            key = ItemImageNumber.HunterKnife,
            name = "���� ������ I",
            type = ItemType.Weapon,
            rarity = "��͵� 1",
            count = 1,
            maxCount = 1,
            damage = 80,
            attribute = Attribute.Fire,
            color = new Color32(255, 255, 255, 255),
            tooltip = "���� ���Ͱ� �ֿ��ϴ� ������ �Ѽհ�. �ܼ��� ������ ������ �ٷ�� ����.",
            price = 150
        });

        items.Add(new Armor
        {
            image = itemImages[(int)ItemImageNumber.HunterArmor],
            key = ItemImageNumber.HunterArmor,
            name = "���� X ����",
            type = ItemType.Armor,
            equipType = EquipSlot.Chest,
            rarity = "��͵� 8",
            count = 1,
            maxCount = 1,
            defense = 82,
            attribute = Attribute.Fire,
            color = new Color32(170, 239, 255, 255),
            tooltip = "�ǿ������� ������� �αⰡ ���� ���Ϳ� ���� ��. ���� �ֻ�� ���� �ٸ���.",
            price = 150
        });

        items.Add(new Potion
        {
            image = itemImages[(int)ItemImageNumber.RecoveryPotion],
            key = ItemImageNumber.RecoveryPotion,
            name = "ȸ����",
            type = ItemType.Potion,
            rarity = "��͵� 1",
            count = 1,
            maxCount = 10,
            heal = 30,
            color = new Color32(36, 225, 148, 255),
            tooltip = "ü���� �ణ ȸ���ϴ� ��.",
            price = 150
        });

        items.Add(new Potion
        {
            image = itemImages[(int)ItemImageNumber.WellCookedMeat],
            key = ItemImageNumber.WellCookedMeat,
            name = "�� ���� ���",
            type = ItemType.Potion,
            rarity = "��͵� 1",
            count = 1,
            maxCount = 3,
            stamina = 50,
            color = new Color32(254, 115, 28, 255),
            tooltip = "����⸦ ������ ����� ���� �� �ִ�. ",
            price = 150
        });

        items.Add(new Trap
        {
            image = itemImages[(int)ItemImageNumber.VineTrap],
            key = ItemImageNumber.VineTrap,
            name = "���� ����",
            type = ItemType.Trap,
            rarity = "��͵� 3",
            count = 1,
            maxCount = 1,
            trap = null,
            color = new Color32(36, 225, 148, 255),
            trapType = TrapType.Setup,
            tooltip = "���͸� ����߸��� ���� ����. ���߷� ���ϰ� �ɸ��� �ߵ��ϴ� ����.",
            price = 150
        });

        itemDB.Add(ItemImageNumber.HunterKnife, items[(int)ItemImageNumber.HunterKnife]);
        itemDB.Add(ItemImageNumber.HunterArmor, items[(int)ItemImageNumber.HunterArmor]);
        itemDB.Add(ItemImageNumber.RecoveryPotion, items[(int)ItemImageNumber.RecoveryPotion]);
        itemDB.Add(ItemImageNumber.WellCookedMeat, items[(int)ItemImageNumber.WellCookedMeat]);
        itemDB.Add(ItemImageNumber.VineTrap, items[(int)ItemImageNumber.VineTrap]);


        if (items.Count > 0)
        {
            Debug.Log("������ ������ ���� �Ϸ�");
        }

        //var item = GetItem(ItemImageNumber.HunterKnife);
        //Debug.Log(item.name);
    }

    public BaseItem GetItem(int index)
    {
        return items[index].Clone();
    }

    public BaseItem GetItem(string name)
    {
        foreach (BaseItem item in items)
        {
            if (item.name == name)
            {
                return item.Clone();
            }
        }
        return null;
    }

    public BaseItem GetItem(ItemImageNumber itemImageNumber)
    {
        return items[(int)itemImageNumber].Clone();
    }
}
