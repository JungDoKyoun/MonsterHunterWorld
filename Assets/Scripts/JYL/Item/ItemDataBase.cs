using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static UnityEditor.Progress;

//이미지 스프라이트 리스트를 이걸로 관리할 예정.


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
    //List<Sprite> itemImages = new List<Sprite>(); // 스프라이트 리스트
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
        //    Debug.LogError("Empty 슬롯용 이미지가 비어있습니다.");
        //}

        //스프라이트 none 인상태인거로 초기화


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

        //고기색 기본색이상해서 바꿈
        items[(int)ItemName.WellDoneSteak].color = new Color32(255 ,178, 86,255);
        Debug.Log(items.Count); 
        //데이터 베이스 초기화
        foreach(var item in items)
        {
            var key = item.id;
            itemDB.Add(key, item);
        }

        //아이템 디폴트 데이터 생성
        #region CSV 생성전 임시로 쓴것
        //items.Add(new Weapon
        //{
        //    image = itemImages[(int)ItemImageNumber.OneHandSword],
        //    id = ItemName.HuntersKnife_I,
        //    name = "헌터 나이프 I",
        //    type = ItemType.Weapon,
        //    rarity = "희귀도 1",
        //    count = 1,
        //    maxCount = 1,
        //    damage = 80,
        //    attribute = Attribute.Fire,
        //    color = new Color32(255, 255, 255, 255),
        //    tooltip = "많은 헌터가 애용하는 전통의 한손검. 단순한 구조로 굉장히 다루기 쉽다.",
        //    price = 150
        //});

        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Head],
        //    id = ItemName.LeatherHeadgear,
        //    name = "레더 헤드",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Head,
        //    level = 1,
        //    rarity = "희귀도 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),            
        //    tooltip = "실용적으로 만들어져 인기가 많은 헌터용 머리 방어구. 역시 최상급 모델은 다르다.",
        //    price = 150
        //});
        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Chest],
        //    id = ItemName.LeatherVest,
        //    name = "레더 베스트",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Chest,
        //    level = 1,
        //    rarity = "희귀도 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),

        //    tooltip = "실용적으로 만들어져 인기가 많은 헌터용 몸통 방어구. 역시 최상급 모델은 다르다.",
        //    price = 150
        //});
        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Arms],
        //    id = ItemName.LeatherGloves,
        //    name = "레더 글러브",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Arms,
        //    level = 1,
        //    rarity = "희귀도 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),

        //    tooltip = "실용적으로 만들어져 인기가 많은 헌터용 손 방어구. 역시 최상급 모델은 다르다.",
        //    price = 150
        //});
        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Waist],
        //    id = ItemName.LeatherBelt,
        //    name = "레더 벨트",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Waist,
        //    level = 1,
        //    rarity = "희귀도 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),

        //    tooltip = "실용적으로 만들어져 인기가 많은 헌터용 허리 방어구. 역시 최상급 모델은 다르다.",
        //    price = 150
        //});
        //items.Add(new Armor
        //{
        //    image = itemImages[(int)ItemImageNumber.Legs],
        //    id = ItemName.LeatherPants,
        //    name = "레더 벨트",
        //    type = ItemType.Armor,
        //    equipType = EquipSlot.Legs,
        //    level = 1,
        //    rarity = "희귀도 1",
        //    count = 1,
        //    maxCount = 1,
        //    defense = 82,
        //    fireDef = 1,
        //    waterDef = 1,
        //    LightningDef = 1,
        //    IceDef = 1,
        //    DragonDef = 1,
        //    color = new Color32(255, 255, 255, 255),

        //    tooltip = "실용적으로 만들어져 인기가 많은 헌터용 다리 방어구. 역시 최상급 모델은 다르다.",
        //    price = 150
        //});
        //items.Add(new Potion
        //{
        //    image = itemImages[(int)ItemImageNumber.Potion],
        //    id = ItemName.Potion,
        //    name = "회복약",
        //    type = ItemType.Potion,
        //    rarity = "희귀도 1",
        //    count = 1,
        //    maxCount = 10,
        //    heal = 30,
        //    color = new Color32(36, 225, 148, 255),
        //    tooltip = "체력을 약간 회복하는 약.",
        //    price = 150
        //});

        //items.Add(new Potion
        //{
        //    image = itemImages[(int)ItemImageNumber.Meat],
        //    id = ItemName.Well_done_Steak,
        //    name = "잘 익은 고기",
        //    type = ItemType.Potion,
        //    rarity = "희귀도 1",
        //    count = 1,
        //    maxCount = 3,
        //    stamina = 50,
        //    color = new Color32(254, 115, 28, 255),
        //    tooltip = "날고기를 적당히 구우면 얻을 수 있다. ",
        //    price = 150
        //});

        //items.Add(new Trap
        //{
        //    image = itemImages[(int)ItemImageNumber.Trap],
        //    id = ItemName.Pitfall_Trap,
        //    name = "구멍 함정",
        //    type = ItemType.Trap,
        //    rarity = "희귀도 3",
        //    count = 1,
        //    maxCount = 1,
        //    trap = null,
        //    color = new Color32(36, 225, 148, 255),
        //    trapType = TrapType.Setup,
        //    tooltip = "몬스터를 떨어뜨리기 위한 함정. 초중량 부하가 걸리면 발동하는 구조.",
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
            Debug.Log("아이템 데이터 세팅 완료");
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
