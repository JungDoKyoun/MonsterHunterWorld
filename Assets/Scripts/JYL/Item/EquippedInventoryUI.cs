using UnityEngine;

//������ �κ� UI
public class EquippedInventoryUI : BaseInventory
{
    public GameObject[] equipSlot;

    private void Awake()
    {
        invenType = InvenType.Equipped;
        //// ����Ʈ ũ�� ���� (���� ���� ����)
        //EnsureEquipListSize();
    }
    private void Start()
    {

        // ��ȭ ���� �̺�Ʈ ����
        InvenToryCtrl.Instance.OnInventoryChanged += RefreshUI;

        for (int i = 0; i < equipSlot.Length; i++)
        {
            var slotCtrl = equipSlot[i].GetComponent<EquipslotCtrl>();
            slotCtrl.SetType((EquipSlot)i);
        }

        // �ʱ� ����
        RefreshUI();

    }

    private void OnEnable()
    {
        items = InvenToryCtrl.Instance.equippedInventory;
        RefreshUI();

        UIManager.Instance.StackUIOpen(UIType.EquipInfoUI);  
    }

    private void OnDisable()
    {
        UIManager.Instance.CloseAll();

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



    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
