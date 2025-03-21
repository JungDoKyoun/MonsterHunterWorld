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

    public BaseItem emptyItem;

    public static ItemDataBase Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        emptyItem = new BaseItem { image = itemImages[(int)ItemImageNumber.HunterKnife], name = "", type = ItemType.Weapon, rarity = "  ", count = 0, maxCount = 0, color = new Color32(255, 255, 255, 0), tooltip = "", price = 0 };

        //아이템 디폴트 데이터 생성

        items.Add(new Weapon { image = itemImages[(int)ItemImageNumber.HunterKnife], name = "헌터 나이프 I", type = ItemType.Weapon, rarity = "희귀도 1", count = 1, maxCount = 1, damage = 80, attribute = Attribute.Fire, color = new Color32(255, 255, 255, 255), tooltip = "많은 헌터가 애용하는 전통의 한손검. 단순한 구조로 굉장히 다루기 쉽다.", price = 150 });
        items.Add(new Armor { image = itemImages[(int)ItemImageNumber.HunterArmor], name = "헌터 X 메일", type = ItemType.Armor, rarity = "희귀도 8", count = 1, maxCount = 1, defense = 82, attribute = Attribute.Fire, color = new Color32(170, 239, 255, 255) ,tooltip = "실용적으로 만들어져 인기가 많은 헌터용 몸통 방어구. 역시 최상급 모델은 다르다.", price = 150 });
        items.Add(new Potion { image = itemImages[(int)ItemImageNumber.RecoveryPotion], name = "회복약", type = ItemType.Potion, rarity = "희귀도 1", count = 1, maxCount = 10, heal = 30, color = new Color32(36, 225, 148,255),tooltip = "체력을 약간 회복하는 약.", price = 150 });
        items.Add(new Potion { image = itemImages[(int)ItemImageNumber.WellCookedMeat], name = "잘 익은 고기", type = ItemType.Potion, rarity = "희귀도 1", count = 1, maxCount = 3, stamina = 50, color = new Color32(254, 115, 28, 255),tooltip = "날고기를 적당히 구우면 얻을 수 있다. ", price = 150 });
        items.Add(new Trap { image = itemImages[(int)ItemImageNumber.VineTrap], name = "구멍 함정", type = ItemType.Trap, rarity = "희귀도 3", count = 1, maxCount = 1, trap = null, color = new Color32(36, 225, 148, 255), trapType = TrapType.Setup , tooltip = "몬스터를 떨어뜨리기 위한 함정. 초중량 부하가 걸리면 발동하는 구조.", price = 150 });

        if (items.Count > 0)
        {
            Debug.Log("아이템 데이터 세팅 완료");
        }

        //var item = GetItem(ItemImageNumber.HunterKnife);
        //Debug.Log(item.name);
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
