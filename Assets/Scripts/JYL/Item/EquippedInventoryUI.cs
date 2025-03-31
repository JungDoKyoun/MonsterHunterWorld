using UnityEngine;

//������ �κ� UI
public class EquippedInventoryUI : BaseInventory
{
    public EquipslotCtrl[] equipSlot;

    private void Awake()
    {
        invenType = InvenType.Equipped;
        //// ����Ʈ ũ�� ���� (���� ���� ����)
        //EnsureEquipListSize();
        InvenToryCtrl.Instance.equippedUIslot = equipSlot;
    }

    private void Start()
    {
        // ��ȭ ���� �̺�Ʈ ����
        InvenToryCtrl.Instance.OnInventoryChanged += RefreshUI;

        for (int i = 0; i < equipSlot.Length; i++)
        {
            equipSlot[i].SetType((EquipSlot)i);
        }

        Debug.Log(equipSlot.Length);

        // �ʱ� ����
        RefreshUI();

    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.equippedInventory;
        RefreshUI();

        UIManager.Instance.StackUIOpen(UIType.EquipInfoUI);

        Debug.Log(" equip" + equipSlot.Length);
        Debug.Log("ctrl"  +InvenToryCtrl.Instance.equippedUIslot.Length);
    }

    private void OnDisable()
    {
        UIManager.Instance.CloseAll();

    }

    // UI ����
    public void RefreshUI()
    {
        for (int i = 0; i < equipSlot.Length - 1; i++)
        {

            if (equipSlot[i] == null)
            {
                Debug.LogWarning($"[EquippedInventoryUI] {i}�� ���Կ� EquipslotCtrl ����");
                continue;
            }

            var item = (i < items.Count && items[i] != null) ? items[i] : ItemDataBase.Instance.emptyItem;


            equipSlot[i].SlotListSetting(item);
        }
    }



    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
