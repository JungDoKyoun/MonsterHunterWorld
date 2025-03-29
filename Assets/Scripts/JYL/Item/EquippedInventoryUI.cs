using UnityEngine;

//������ �κ� UI
public class EquippedInventoryUI : BaseInventory
{
    public GameObject[] equipSlot;
    public bool IsOpen => gameObject.activeSelf;

    private void Start()
    {
        invenType = InvenType.Equipped;



        items = InvenToryCtrl.Instance.equippedInventory;

        // ����Ʈ ũ�� ���� (���� ���� ����)
        EnsureEquipListSize();

        // ��ȭ ���� �̺�Ʈ ����
        InvenToryCtrl.Instance.OnEquippedChanged += RefreshUI;

        // �ʱ� ����
        RefreshUI();

    }

    // ����Ʈ ũ�Ⱑ �����ϸ� �� �������� ä��
    private void EnsureEquipListSize()
    {
        int targetCount = equipSlot.Length;

        while (items.Count < targetCount)
        {
            items.Add(ItemDataBase.Instance.emptyItem);
        }
    }
    // UI ����
    public void RefreshUI()
    {
        for (int i = 0; i < equipSlot.Length; i++)
        {
            var slotCtrl = equipSlot[i].GetComponent<EquipslotCtrl>();
            if (slotCtrl == null)
            {
                Debug.LogWarning($"[EquippedInventoryUI] {i}�� ���Կ� EquipslotCtrl ����");
                continue;
            }

            var item = (i < items.Count && items[i] != null)
                ? items[i]
                : ItemDataBase.Instance.emptyItem;

            slotCtrl.SlotListSetting(item);
        }
    }



    public void EquipItem(BaseItem value)
    {
        var index = (int)value.GetEquipSlot();

        if (index < 0 || index >= equipSlot.Length || value.name == "")
        {
            Debug.Log("���� �߸�����ϴ�.");
            return;
        }

        equipSlot[index].GetComponent<EquipslotCtrl>().SlotListSetting(value);
        //items[index] = value;
    }


    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
