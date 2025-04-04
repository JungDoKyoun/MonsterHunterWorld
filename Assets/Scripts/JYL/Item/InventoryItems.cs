using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

//���� ���������� ������ �κ��丮
public class InventoryItems : BaseInventory
{
    public GameObject upSlot;
    public GameObject downSlot;

    public bool IsOpen => gameObject.activeSelf;

    private void Awake()
    {
        invenType = InvenType.Inven;

        //���� ����
        SlotSetting(upSlot, invenType);
    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.inventory;

        StartCoroutine(DelayedRefresh());


        //�κ� ����� UI �ʱ�ȭ
        InvenToryCtrl.Instance.OnInventoryChanged += () =>
        {
            RefreshUI(items);
        };

    }

    void OnDisable()
    {
        //�κ� ����� UI �ʱ�ȭ
        InvenToryCtrl.Instance.OnInventoryChanged -= () =>
        {
            RefreshUI(items);
        };

    }


    IEnumerator DelayedRefresh()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshUI(items);
    }


}
