using System.Collections.Generic;
using UnityEngine;

//이미지 스프라이트 리스트를 이걸로 관리할 예정.


public enum ItemName
{
    HuntersKnife_I,

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
    public List<BaseItem> items = new List<BaseItem>();
    public Dictionary<ItemName, BaseItem> itemDB = new Dictionary<ItemName, BaseItem>();


    BaseItem emptyItem;
    public BaseItem EmptyItem => emptyItem.Clone();

    public static ItemDataBase Instance;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

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
        items[(int)ItemName.WellDoneSteak].color = new Color32(255, 178, 86, 255);

        //데이터 베이스 초기화
        foreach (var item in items)
        {
            var key = item.id;
            itemDB.Add(key, item);
        }

        if (items.Count > 0)
        {
            Debug.Log("아이템 데이터 세팅 완료");
        }
    }


    public BaseItem GetItem(int index)
    {
        return items[index].Clone();
    }

    public BaseItem GetItem(ItemName id)
    {
        return itemDB[id].Clone();
    }
}
