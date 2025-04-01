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
    }

    private void Start()
    {


        for (int i = 0; i < equipSlot.Length; i++)
        {
            equipSlot[i].SetType((EquipSlot)i);
        }

        //InvenToryCtrl.Instance.SlotSetting(equipSlot);

        InvenToryCtrl.Instance.equippedUiSlot = equipSlot;


        //// �ʱ� ����
        RefreshUI();

        InvenToryCtrl.Instance.DebugTest();
    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.equippedInventory;

        UIManager.Instance.StackUIOpen(UIType.EquipInfoUI);

        // ��ȭ ���� �̺�Ʈ ����
        InvenToryCtrl.Instance.OnInventoryChanged += RefreshUI;

        RefreshUI();
    }

    private void OnDisable()
    {
        UIManager.Instance.CloseAll();
        InvenToryCtrl.Instance.OnInventoryChanged -= RefreshUI;

        //foreach (var item in items)
        //{
        //    Debug.Log(item.name);
        //}

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

            var item = (i < items.Count && items[i] != null) ? items[i] : ItemDataBase.Instance.EmptyItem;


            equipSlot[i].SlotListSetting(item);
        }
    }



    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
