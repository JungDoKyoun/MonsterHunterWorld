using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;


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

    [SerializeField]
    Text boxIndexText;

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

        for(int i = 0; i < 1000; i++)
        {
            boxItems.Add(ItemDataBase.Instance.emptyItem);
        }

        //for (int i = 0; i < 30; i++)
        //{
        //    SetItem(index: i, item: ItemDataBase.Instance.GetItem((int)ItemImageNumber.RecoveryPotion));
        //}

        Debug.Log("�ڽ��κ� ����");
    }

    public void SetItem(BaseItem item , int index)
    {
        if(index < 0 || index >= 1000)
        {
            Debug.Log("�ε��� �߸��־����ϴ�.");
            return;
        }

        boxItems[index] = item;
    }


    public void NextBox(int index)
    {
        boxIndex = index;

        for (int i = 0; i < 100; i++)
        {
            slot[i].GetComponent<ItemSlot>().SlotSetItem(boxItems[(boxIndex - 1) * 100 + i]);
            //Debug.Log((boxIndex - 1) * 100 + i);
        }

        boxIndexText.text = index.ToString() + " / " + boxSize.ToString();

    }

    private void Update()
    {
        if (isBoxOpen)
        {
            BoxInput();
            //Debug.Log("�ڽ��κ� ������Ʈ");
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
            Debug.Log("Q");
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
            Debug.Log("E");
        }

        //Debug.Log("���¿Գ�");
    }


    public void SelectTag(ItemType tag)
    {
        boxTag = tag;
        boxIndex = 0;
        NextBox(boxIndex);
    }


}
