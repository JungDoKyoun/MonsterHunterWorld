using System.Linq;

//���� ���������� ������ �κ��丮
public class InventoryItems : BaseInventory
{

    void Start()
    {
        invenType = InvenType.Inven;

        //�׽�Ʈ�� ������ �߰�
        foreach (var item in ItemDataBase.Instance.items.ToList())
        {
            items.Add(item);
            //Debug.Log(item.name);
        }
        
        //InvenToryCtrl.Instance.AddItemByName(items, "ȸ����");
        AddItem(items, ItemDataBase.Instance.GetItem(ItemImageNumber.RecoveryPotion));


        SlotSetting(gameObject,invenType);

        //�������ִ� �������� �ִ°��
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
            }
        }
        //������ ���� �ϸ��

    }

}
