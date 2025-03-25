using System.Linq;

//현재 가지고있을 아이템 인벤토리
public class InventoryItems : BaseInventory
{

    void Start()
    {
        invenType = InvenType.Inven;

        //테스트용 아이템 추가
        foreach (var item in ItemDataBase.Instance.items.ToList())
        {
            items.Add(item);
            //Debug.Log(item.name);
        }
        
        //InvenToryCtrl.Instance.AddItemByName(items, "회복약");
        AddItem(items, ItemDataBase.Instance.GetItem(ItemImageNumber.RecoveryPotion));


        SlotSetting(gameObject,invenType);

        //가지고있는 아이템이 있는경우
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
            }
        }
        //없으면 고대로 하면됨

    }

}
