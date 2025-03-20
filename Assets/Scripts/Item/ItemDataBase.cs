using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemImageNumber
{
    HunterKnife,
    HunterArmor,
    RecoveryPotion,
    WellCookedMeat,
    VineTrap
}

public class ItemDataBase : MonoBehaviour
{
    [SerializeField]
    List<Sprite> itemImages = new List<Sprite>(); // 스프라이트 리스트
    
    [SerializeField]
    List<GameObject> trapItemObj = new List<GameObject>();

    public List<BaseItem> items = new List<BaseItem>();


    public static ItemDataBase Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        
        items.Add(new Weapon { image = itemImages[(int)ItemImageNumber.HunterKnife], name = "헌터 나이프", type = ItemType.Weapon, rarity = 1, count = 1, maxCount = 1, damage = 10, attribute = Attribute.Fire, color = new Color32(255, 225, 255, 255) });
        items.Add(new Armor { image = itemImages[(int)ItemImageNumber.HunterArmor], name = "헌터 아머", type = ItemType.Armor, rarity = 1, count = 1, maxCount = 1, defense = 10, attribute = Attribute.Fire, color = new Color32(255, 225, 255, 255) });
        items.Add(new Potion { image = itemImages[(int)ItemImageNumber.RecoveryPotion], name = "회복약", type = ItemType.Potion, rarity = 1, count = 1, maxCount = 10, heal = 10, color = new Color32(36, 225, 148,255) });
        items.Add(new Potion { image = itemImages[(int)ItemImageNumber.WellCookedMeat], name = "잘 구운고기", type = ItemType.Potion, rarity = 1, count = 1, maxCount = 3, stamina = 10, color = new Color32(254, 115, 28, 255) });
        items.Add(new Trap { image = itemImages[(int)ItemImageNumber.VineTrap], name = "덩굴 함정", type = ItemType.Trap, rarity = 1, count = 1, maxCount = 1, trap = null, color = new Color32(36, 225, 148, 255), trapType = TrapType.Setup });

        Debug.Log("아이템 데이터 세팅 완료");

        var item = GetItem(ItemImageNumber.HunterKnife);
        Debug.Log(item.name);
    }

    public BaseItem GetItem(int index)
    {
        return items[index];
    }

    public BaseItem GetItem(ItemImageNumber itemImageNumber)
    {
        return items[(int)itemImageNumber];
    }
}
