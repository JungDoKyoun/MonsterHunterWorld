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
    List<Sprite> itemImages = new List<Sprite>(); // ��������Ʈ ����Ʈ
    
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


        
        items.Add(new Weapon { image = itemImages[(int)ItemImageNumber.HunterKnife], name = "���� ������", type = ItemType.Weapon, rarity = 1, count = 1, maxCount = 1, damage = 10, attribute = Attribute.Fire, color = Color.white });
        items.Add(new Armor { image = itemImages[(int)ItemImageNumber.HunterArmor], name = "���� �Ƹ�", type = ItemType.Armor, rarity = 1, count = 1, maxCount = 1, defense = 10, attribute = Attribute.Fire, color = Color.white });
        items.Add(new Potion { image = itemImages[(int)ItemImageNumber.RecoveryPotion], name = "ȸ����", type = ItemType.Potion, rarity = 1, count = 1, maxCount = 10, heal = 10, color = new Color(36, 225, 148) });
        items.Add(new Potion { image = itemImages[(int)ItemImageNumber.WellCookedMeat], name = "�� ������", type = ItemType.Potion, rarity = 1, count = 1, maxCount = 3, stamina = 10, color = new Color(254, 115, 28) });
        items.Add(new Trap { image = itemImages[(int)ItemImageNumber.VineTrap], name = "���� ����", type = ItemType.Trap, rarity = 1, count = 1, maxCount = 1, trap = null, color = new Color(36, 225, 148), trapType = TrapType.Setup });

        Debug.Log("������ ������ ���� �Ϸ�");
    }

}
