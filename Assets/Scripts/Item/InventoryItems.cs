using System.Collections;
using UnityEngine;

//���� ���������� ������ �κ��丮
public class InventoryItems : BaseInventory
{
    public GameObject upSlot;
    public GameObject downSlot;

    void Start()
    {
        invenType = InvenType.Inven;

        //���� ����
        SlotSetting(upSlot, invenType);
        //�κ� ����
        InvenInit();

        //������ ŉ��
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.RecoveryPotion));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.RecoveryPotion));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.WellCookedMeat));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.VineTrap));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.WellCookedMeat));

        //�������ִ� �������� �ִ°��
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
                Debug.Log(items[i].name + "�̰��ֵ��ƾƾƾƾƾƾƾƾӾƾƾƾƾƾƾƾ�");
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
        RefreshUI();
    }

}
