using System.Collections.Generic;
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


    //����Ʈ���� ������ ���� �߰�
    public void AddItemByName(List<BaseItem> itemList, string targetName)
    {
        foreach (BaseItem item in itemList)
        {
            if (item.name == targetName)
            {
                item.count = Mathf.Min(item.count + 1, item.maxCount); // �ִ�ġ �ʰ� ����
                Debug.Log($"������ '{item.name}' ���� ����: {item.count}/{item.maxCount}");
                return;
            }
        }

        Debug.LogWarning($"'{targetName}' �̸��� �������� ����Ʈ���� ã�� �� �����ϴ�.");
    }

    //������ �̵�
    public void ChangeItem(InvenType type, BaseItem item)
    {
        //�κ��丮���� �������� ����������
        if (type == InvenType.Inven)
        {
            if(boxInvenTory.BoxItems.Contains(item))
            {
                AddItemByName(boxInvenTory.BoxItems, item.name);
            }
            else
            {
                boxInvenTory.BoxItems.Add(item);
            }

            inventoryItems.Items.Remove(item);

        }
        //�繰�Կ��� �������� ����������
        else
        {
            //�������� ������
            //�������� �κ��丮�� �̵�
            if(inventoryItems.Items.Contains(item))
            {
                AddItemByName(inventoryItems.Items, item.name);
            }
            else
            {
                inventoryItems.Items.Add(item);
            }

            boxInvenTory.BoxItems.Remove(item);
            
        }
                                                                                                              

        Debug.Log("�繰�� ������ ���� : " + boxInvenTory.BoxItems.Count );
    }

}
