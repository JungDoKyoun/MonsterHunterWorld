using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;


public class CSVItemLoader : MonoBehaviour
{
    [SerializeField] private TextAsset csvFile;
    [SerializeField] private List<Sprite> itemImages;



    public List<BaseItem> LoadItemsFromCSV()
    {
        var items = new List<BaseItem>();
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일이 지정되지 않았습니다.");
            return items;
        }

        using StringReader reader = new StringReader(csvFile.text);
        bool isFirstLine = true;

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            if (isFirstLine) { isFirstLine = false; continue; }

            string[] values = line.Split(',');

            int id = int.Parse(values[0]);
            string name = values[1];
            string type = values[2];
            string equipSlot = values[3];
            string rarity = values[4];
            int.TryParse(values[5], out int damage);
            int.TryParse(values[6], out int defense);
            int.TryParse(values[7], out int level);
            int.TryParse(values[8], out int fire);
            int.TryParse(values[9], out int water);
            int.TryParse(values[10], out int lightning);
            int.TryParse(values[11], out int ice);
            int.TryParse(values[12], out int dragon);
            int.TryParse(values[13], out int maxCount);
            string tooltip = values[14];
            int.TryParse(values[15], out int price);
            int.TryParse(values[16], out int heal);
            int.TryParse(values[17], out int maxHeal);
            int.TryParse(values[18], out int stamina);
            int.TryParse(values[19], out int maxStamina);
            int.TryParse(values[20], out int imageIndex);

            Sprite image = (imageIndex >= 0 && imageIndex < itemImages.Count) ? itemImages[imageIndex] : null;

            BaseItem item;
            if (type == "Weapon")
            {
                item = new Weapon
                {
                    id = (ItemName)id,
                    name = name,
                    type = ItemType.Weapon,
                    image = image,
                    rarity = rarity,
                    damage = damage,
                    attribute = ElementAttribute.Fire, // TODO: 확장 가능
                    maxCount = 1,
                    count = 1,
                    tooltip = tooltip,
                    price = price,
                    color = Color.white
                };
            }
            else if (type == "Armor")
            {
                item = new Armor
                {
                    id = (ItemName)id,
                    name = name,
                    type = ItemType.Armor,
                    image = image,
                    rarity = rarity,
                    equipType = (EquipSlot)System.Enum.Parse(typeof(EquipSlot), equipSlot),
                    defense = defense,
                    level = level,
                    fireDef = fire,
                    waterDef = water,
                    LightningDef = lightning,
                    IceDef = ice,
                    DragonDef = dragon,
                    maxCount = 1,
                    count = 1,
                    tooltip = tooltip,
                    price = price,
                    color = Color.white
                };
            }
            else if (type == "Potion")
            {
                item = new Potion
                {
                    id = (ItemName)id,
                    name = name,
                    type = ItemType.Potion,
                    image = image,
                    rarity = rarity,
                    maxCount = maxCount,
                    count = 1,
                    tooltip = tooltip,
                    price = price,
                    color = new Color32(36, 225, 148, 255),
                    heal = heal,
                    maxHeal = maxHeal,
                    stamina = stamina,
                    maxStamina = maxStamina,

                };
            }
            else if (type == "Trap")
            {
                
                item = new Trap
                {                    
                    id = (ItemName)id,
                    name = name,
                    type = ItemType.Trap,
                    image = image,
                    rarity = rarity,
                    maxCount = maxCount,
                    count = 1,
                    tooltip = tooltip,
                    price = price,
                    color = new Color32(36, 225, 148, 255)
                };
            }
            else
            {
                item = new BaseItem
                {
                    id = (ItemName)id,
                    name = "",
                    type = ItemType.Empty,
                    image = image,
                    rarity = "",
                    maxCount = 0,
                    count = 0,
                    tooltip = "",
                    price = 0,
                    color = new Color32(255, 255, 255, 0)
                };
            }

            items.Add(item);
        }

        return items;
    }
}
