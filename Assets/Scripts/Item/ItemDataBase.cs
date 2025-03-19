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
    public List<Sprite> itemImages = new List<Sprite>(); // 스프라이트 리스트
    public List<Image> uiItemImages = new List<Image>(); // UI 이미지 리스트
    public List<GameObject> trapItemObj = new List<GameObject>();
    public List<BaseItem> items = new List<BaseItem>();

    //임시로 이미지 저장해줄 변수
    public Image temp;

    public static ItemDataBase Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }


        // UI 이미지 설정
        foreach (var image in itemImages)
        {
            
            uiItemImages.Add(temp); // UI Image에 Sprite 적용
            uiItemImages[uiItemImages.Count - 1].sprite = image;
        }



        items.Add(new Weapon { image = uiItemImages[(int)ItemImageNumber.HunterKnife], name = "헌터 나이프", type = ItemType.Weapon, rarity = 1, count = 1, maxCount = 1, damage = 10, attribute = Attribute.Fire, color = Color.white });
        items.Add(new Armor { image = uiItemImages[(int)ItemImageNumber.HunterArmor], name = "헌터 아머", type = ItemType.Armor, rarity = 1, count = 1, maxCount = 1, defense = 10, attribute = Attribute.Fire, color = Color.white });
        items.Add(new Potion { image = uiItemImages[(int)ItemImageNumber.RecoveryPotion], name = "회복약", type = ItemType.Potion, rarity = 1, count = 1, maxCount = 10, heal = 10, color = new Color(36, 225, 148) });
        items.Add(new Potion { image = uiItemImages[(int)ItemImageNumber.WellCookedMeat], name = "잘 구운고기", type = ItemType.Potion, rarity = 1, count = 1, maxCount = 3, stamina = 10, color = new Color(254, 115, 28) });
        items.Add(new Trap { image = uiItemImages[(int)ItemImageNumber.VineTrap], name = "덩굴 함정", type = ItemType.Trap, rarity = 1, count = 1, maxCount = 1, trap = null, color = new Color(36, 225, 148), trapType = TrapType.Setup });

        Debug.Log("아이템 데이터 세팅 완료");
    }

}
