using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ItemDataBase : MonoBehaviour
{
    List<BaseItem> items = new List<BaseItem>();    

    void Start()
    {
        items.Add(new Weapon { name = "HunterSword", type = ItemType.Weapon, rarity = 1, count = 1, maxCount = 1, damage = 10, attribute = Attribute.Fire });
        items.Add(new Armor { name = "HunterArmor", type = ItemType.Armor, rarity = 1, count = 1, maxCount = 1, defense = 10, attribute = Attribute.Fire });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
