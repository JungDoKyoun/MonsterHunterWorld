using System.Collections;
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
    //���� ���õ� �繰�� �±�
    ItemType boxTag;

    bool isBoxOpen = false;

    public bool IsBoxOpen { get => isBoxOpen; set => isBoxOpen = value; }
    public List<BaseItem> BoxItems
    {
        get => boxItems;
        set => boxItems = value;
    }
        

    private void FixedUpdate()
    {
        if(isBoxOpen)
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
        }
        //�繰�� ������
        else if (Input.GetKeyDown(KeyCode.E))
        {
            boxIndex++;
            if (boxIndex > boxSize)
            {
                boxIndex = boxSize;
            }
        }
    }

}
