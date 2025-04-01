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
        InvenToryCtrl.Instance.equippedUiSlot = equipSlot;

        Debug.Log(equipSlot.Length);

        // �ʱ� ����
        RefreshUI();

    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.equippedInventory;
        RefreshUI();

        UIManager.Instance.StackUIOpen(UIType.EquipInfoUI);

        // ��ȭ ���� �̺�Ʈ ����
        InvenToryCtrl.Instance.OnInventoryChanged += RefreshUI;

    }

    private void OnDisable()
    {
        UIManager.Instance.CloseAll();
        InvenToryCtrl.Instance.OnInventoryChanged -= RefreshUI;
        InvenToryCtrl.Instance.SaveInventoryToFirebase();

        foreach (var item in items)
        {
            Debug.Log(item.name);
        }

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
