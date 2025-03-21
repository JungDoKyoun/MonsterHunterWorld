using System.Collections.Generic;
using UnityEngine;


//�繰�� �κ��丮
public class BoxInvenTory : MonoBehaviour
{
    InvenType invenType = InvenType.Box;

    //���� �繰�� �ε���
    int boxIndex = 1;
    //�繰�� �ִ밹��
    int boxSize = 10;
    //�繰�� �κ��丮
    List<BaseItem> boxItems = new List<BaseItem>();

    //�繰�� ����
    List<GameObject> slot = new List<GameObject>();

    //���� ���õ� �繰�� �±�
    ItemType boxTag;

    bool isBoxOpen = false;

    public bool IsBoxOpen { get => isBoxOpen; set => isBoxOpen = value; }
    public List<BaseItem> BoxItems
    {
        get => boxItems;
        set => boxItems = value;
    }

    private void Start()
    {
        //�ڽ� ������Ʈ ����
        var objs = GetComponentsInChildren<ItemSlot>();
        foreach (var item in objs)
        {
            item.SetInvenType(invenType);
            slot.Add(item.gameObject);
        }


    }



    public void NextBox(int index)
    {
        boxIndex = index;

        for (int i = 0; i < 100; i++)
        {
            slot[i].GetComponent<ItemSlot>().SlotSetItem(boxItems[(boxIndex - 1) * 100 + i]);
            Debug.Log((boxIndex - 1) * 100 + i);
        }

        
    }

    private void FixedUpdate()
    {
        if (isBoxOpen)
        {
            BoxInput();
        }
    }

    public void BoxInput()
    {
        //�繰�� ����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            boxIndex--;
            if (boxIndex <= 0)
            {
                boxIndex = 1;
            }
            NextBox(boxIndex);
        }
        //�繰�� ������
        else if (Input.GetKeyDown(KeyCode.E))
        {
            boxIndex++;
            if (boxIndex > boxSize)
            {
                boxIndex = boxSize;
            }
            NextBox(boxIndex);

        }
    }

}
