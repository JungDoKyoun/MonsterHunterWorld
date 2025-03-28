using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.Progress;

//�̹��� ��������Ʈ ����Ʈ�� �̰ɷ� ������ ����.


public enum ItemName
{
    HuntersKnife_I ,

    LeatherHead,
    LeatherVest,
    LeatherGloves,
    LeatherBelt,
    LeatherPants,

    HuntersHelmS,
    HuntersMailS,
    HuntersArmS,
    HuntersCoilS,
    HuntersGreavesS,

    AlloyHelmS,
    AlloyMailS,
    AlloyArmS,
    AlloyCoilS,
    AlloyGreavesS,

    BoneHelmS,
    BoneMailS,
    BoneArmS,
    BoneCoilS,
    BoneGreavesS,

    KuluHelmS,
    KuluMailS,
    KuluBraceS,
    KuluCoilS,
    KuluGreavesS,

    AnjaHelmS,
    AnjaMailS,
    AnjaArmS,
    AnjaCoilS,
    AnjaGreavesS,

    Potion,

    WellDoneSteak,

    PitfallTrap,

    Empty

}


public class ItemDataBase : MonoBehaviour
{
    //[SerializeField]
    //List<Sprite> itemImages = new List<Sprite>(); // ��������Ʈ ����Ʈ
    //public List<Sprite> ItemImages { get => itemImages; set => itemImages = value; }

    [SerializeField]
    List<GameObject> trapItemObj = new List<GameObject>();

    public List<BaseItem> items = new List<BaseItem>();
    public Dictionary<ItemName, BaseItem> itemDB = new Dictionary<ItemName, BaseItem>();


    public BaseItem emptyItem;

    public static ItemDataBase Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //if (itemImages.Count <= (int)ItemImageNumber.Empty ||
        //    itemImages[(int)ItemImageNumber.Empty] == null)
        //{
        //    Debug.LogError("Empty ���Կ� �̹����� ����ֽ��ϴ�.");
        //}

        //��������Ʈ none �λ����ΰŷ� �ʱ�ȭ


        //Debug.Log(emptyItem.name);
        //Debug.Log(emptyItem.image.name);
        //Debug.Log(emptyItem.color);



        items = GetComponent<CSVItemLoader>().LoadItemsFromCSV();

        emptyItem = new BaseItem
        {
            image = items[(int)ItemName.Empty].image,
            id = ItemName.Empty,
            name = "",
            type = ItemType.Empty,
            rarity = "  ",
            count = 0,
            maxCount = 0,
            color = new Color32(255, 255, 255, 0),
            tooltip = "",
            price = 0
        };

        //���� �⺻���̻��ؼ� �ٲ�
        items[(int)ItemName.WellDoneSteak].color = new Color32(255 ,178, 86,255);
        Debug.Log(items.Count); 
        //������ ���̽� �ʱ�ȭ
        foreach(var item in items)
        {
            var key = item.id;
            itemDB.Add(key, item);
        }

        //������ ����Ʈ ������ ����
        #region CSV ������ �ӽ÷� ����
        //items.Add(new Weapon
        //{
        //    image = itemImages[(int)ItemImageNumber.OneHandSword],
        //    id = ItemName.HuntersKnife_I,
        //    name = "���� ������ I",
        //    type = ItemType.Weapon,
        //    rarity = "��͵� 1",
        //    count = 1,
        //    maxCount = 1,
        //    damage = 80,
        //    attribute = Attribute.Fire,
        //    color = new Color32(255, 255, 255, 255),
        //    tooltip = "���� ���Ͱ� �ֿ��ϴ� ������ �Ѽհ�. �ܼ��� ������ ������ �ٷ�� ����.",
        //    price = 150
        //});

        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Head],
        //    id = ItemName.LeatherHeadgear,
        //    name = "���� ���",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Head,
        //    level = 1,
        //    rarity = "��͵� 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),            
        //    tooltip = "�ǿ������� ������� �αⰡ ���� ���Ϳ� �Ӹ� ��. ���� �ֻ�� ���� �ٸ���.",
        //    price = 150
        //});
        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Chest],
        //    id = ItemName.LeatherVest,
        //    name = "���� ����Ʈ",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Chest,
        //    level = 1,
        //    rarity = "��͵� 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),

        //    tooltip = "�ǿ������� ������� �αⰡ ���� ���Ϳ� ���� ��. ���� �ֻ�� ���� �ٸ���.",
        //    price = 150
        //});
        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Arms],
        //    id = ItemName.LeatherGloves,
        //    name = "���� �۷���",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Arms,
        //    level = 1,
        //    rarity = "��͵� 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),

        //    tooltip = "�ǿ������� ������� �αⰡ ���� ���Ϳ� �� ��. ���� �ֻ�� ���� �ٸ���.",
        //    price = 150
        //});
        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Waist],
        //    id = ItemName.LeatherBelt,
        //    name = "���� ��Ʈ",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Waist,
        //    level = 1,
        //    rarity = "��͵� 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),

        //    tooltip = "�ǿ������� ������� �αⰡ ���� ���Ϳ� �㸮 ��. ���� �ֻ�� ���� �ٸ���.",
        //    price = 150
        //});
        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Legs],
        //    id = ItemName.LeatherPants,
        //    name = "���� ��Ʈ",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Legs,
        //    level = 1,
        //    rarity = "��͵� 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),

        //    tooltip = "�ǿ������� ������� �αⰡ ���� ���Ϳ� �ٸ� ��. ���� �ֻ�� ���� �ٸ���.",
        //    price = 150
        //});
        //items.Add(new Potion
        //{
        //    image = itemImages[(int)ItemImageNumber.Potion],
        //    id = ItemName.Potion,
        //    name = "ȸ����",
        //    type = ItemType.Potion,
        //    rarity = "��͵� 1",
        //    count = 1,
        //    maxCount = 10,
        //    heal = 30,
        //    color = new Color32(36, 225, 148, 255),
        //    tooltip = "ü���� �ణ ȸ���ϴ� ��.",
        //    price = 150
        //});

        //items.Add(new Potion
        //{
        //    image = itemImages[(int)ItemImageNumber.Meat],
        //    id = ItemName.Well_done_Steak,
        //    name = "�� ���� ���",
        //    type = ItemType.Potion,
        //    rarity = "��͵� 1",
        //    count = 1,
        //    maxCount = 3,
        //    stamina = 50,
        //    color = new Color32(254, 115, 28, 255),
        //    tooltip = "����⸦ ������ ����� ���� �� �ִ�. ",
        //    price = 150
        //});

        //items.Add(new Trap
        //{
        //    image = itemImages[(int)ItemImageNumber.Trap],
        //    id = ItemName.Pitfall_Trap,
        //    name = "���� ����",
        //    type = ItemType.Trap,
        //    rarity = "��͵� 3",
        //    count = 1,
        //    maxCount = 1,
        //    trap = null,
        //    color = new Color32(36, 225, 148, 255),
        //    trapType = TrapType.Setup,
        //    tooltip = "���͸� ����߸��� ���� ����. ���߷� ���ϰ� �ɸ��� �ߵ��ϴ� ����.",
        //    price = 150
        //});

        //itemDB.Add(ItemName.HuntersKnife_I, items[(int)ItemName.HuntersKnife_I]);

        //itemDB.Add(ItemName.LeatherHead, items[(int)ItemName.LeatherHead]);
        //itemDB.Add(ItemName.LeatherVest, items[(int)ItemName.LeatherVest]);
        //itemDB.Add(ItemName.LeatherGloves, items[(int)ItemName.LeatherGloves]);
        //itemDB.Add(ItemName.LeatherBelt, items[(int)ItemName.LeatherBelt]);
        //itemDB.Add(ItemName.LeatherPants, items[(int)ItemName.LeatherPants]);

        //itemDB.Add(ItemName.Potion, items[(int)ItemName.Potion]);
        //itemDB.Add(ItemName.WellDoneSteak, items[(int)ItemName.WellDoneSteak]);
        //itemDB.Add(ItemName.PitfallTrap, items[(int)ItemName.PitfallTrap]);
        #endregion


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

    public BaseItem GetItem(ItemName id)
    {
        //foreach (BaseItem item in items)
        //{
        //    if (item.id == id)
        //    {
        //        Debug.Log(id.ToString());
        //        Debug.Log(item.name);
        //        return item.Clone();
        //    }
        //}
        //return null;

        return itemDB[id];
    }

    //public BaseItem GetItem(ItemImageNumber itemImageNumber)
    //{
    //    return items[(int)itemImageNumber].Clone();
    //}
}
