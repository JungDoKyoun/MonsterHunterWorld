using System.Collections;
using UnityEngine;

//���� ���������� ������ �κ��丮
public class InventoryItems : BaseInventory, IClosableUI
{
    public GameObject upSlot;
    public GameObject downSlot;
    
    public bool IsOpen => gameObject.activeSelf;

    void Start()
    {
        invenType = InvenType.Inven;

        //���� ����
        SlotSetting(upSlot, invenType);
        //�κ� ����
        InvenInit();

        //������ ŉ��
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemName.Potion));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemName.PitfallTrap));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemName.WellDoneSteak));

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
        RefreshUI();
        UIManager.Instance.RegisterUI(this);
    }

    private void OnDisable()
    {
        UIManager.Instance.UnregisterUI(this);
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
