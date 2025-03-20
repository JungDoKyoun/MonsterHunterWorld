using UnityEngine;

public enum InvenType
{
    Inven,
    Box
}

//�÷��̾��� ��� �κ��丮
public class InvenToryCtrl : MonoBehaviour
{
    //���� ���������� �κ��丮
    public InventoryItems inventoryItems;
    //�繰�� �κ��丮
    public BoxInvenTory boxInvenTory;

    public static InvenToryCtrl Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ChangeItem(InvenType type, BaseItem item)
    {
        //�κ��丮���� �������� ����������
        if (type == InvenType.Inven)
        {
            inventoryItems.Items.Remove(item);
            boxInvenTory.BoxItems.Add(item);

        }
        //�繰�Կ��� �������� ����������
        else
        {
            //�������� ������
            //�������� �κ��丮�� �̵�
            boxInvenTory.BoxItems.Remove(item);
            inventoryItems.Items.Add(item);
        }

    }

}
