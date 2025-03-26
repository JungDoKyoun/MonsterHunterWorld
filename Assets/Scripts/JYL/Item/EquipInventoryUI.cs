using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��� �κ��丮 UI
public class EquipInventoryUI : BaseInventory
{
    [SerializeField] GameObject slotParent;

    private void Start()
    {
        invenType = InvenType.EquipBox;

        SlotSetting(slotParent, invenType);

        InvenInit();

        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.HunterKnife));
        GetItemToInventory(ItemDataBase.Instance.GetItem(ItemImageNumber.HunterArmor));

        //�������ִ� �������� �ִ°��
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[i]);
            }
        }
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
