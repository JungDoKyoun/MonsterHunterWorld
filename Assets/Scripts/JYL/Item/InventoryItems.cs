using System.Collections;
using UnityEngine;

//���� ���������� ������ �κ��丮
public class InventoryItems : BaseInventory
{
    public GameObject upSlot;
    public GameObject downSlot;

    public bool IsOpen => gameObject.activeSelf;

    void Start()
    {
        invenType = InvenType.Inven;

        //���� ����
        SlotSetting(upSlot, invenType);

        items = InvenToryCtrl.Instance.inventory;
        //�κ� ����
        //InvenInit(items);

        //������ ŉ��
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.PitfallTrap));
        GetItemToInventory(items, ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        
        InvenToryCtrl.Instance.OnInventoryChanged += () =>
        {
            RefreshUI(items);
        };

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

    private void OnEnable()
    {
        StartCoroutine(DelayedRefresh());
    }

    IEnumerator DelayedRefresh()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshUI(items);
    }


}
