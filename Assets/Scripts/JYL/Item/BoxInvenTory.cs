using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//�繰�� �κ��丮
public class BoxInvenTory : BaseInventory
{

    //���� �繰�� �ε���
    int boxIndex = 1;
    //�繰�� �ִ밹��
    int boxMaxIndex = 10;

    [SerializeField]
    Text boxIndexText;

    //���� ���õ� �繰�� �±�
    ItemType selectBoxTag;

    public bool IsOpen => gameObject.activeSelf;


    private void Start()
    {
        invenType = InvenType.Box;

        SlotSetting(gameObject, invenType);
        items = InvenToryCtrl.Instance.boxInven;

        //InvenInit(items);

        InvenToryCtrl.Instance.OnInventoryChanged += () =>
        {
            RefreshUI(items);
        };
    }

    override public void RefreshUI(List<BaseItem> list)
    {
        for (int i = 0; i < 100; i++)
        {
            var slotComp = slot[i].GetComponent<ItemSlot>();
            if (slotComp == null)
            {
                Debug.LogWarning($"���� {i}���� ItemSlot ������Ʈ�� �����ϴ�.");
                continue;
            }

            int targetIndex = (boxIndex - 1) * 100 + i;

            if (targetIndex < list.Count && list[targetIndex] != null)
            {
                slotComp.SlotSetItem(list[targetIndex]);
            }
            else
            {
                slotComp.SlotSetItem(ItemDataBase.Instance.emptyItem);
            }
        }

        boxIndexText.text = $"{boxIndex} / {boxMaxIndex}";
    }

    public void NextBox(int index)
    {
        boxIndex = index;

        for (int i = 0; i < 100; i++)
        {
            if ((boxIndex - 1) * 100 + i < items.Count)
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(items[(boxIndex - 1) * 100 + i]);
            }
            else
            {
                slot[i].GetComponent<ItemSlot>().SlotSetItem(ItemDataBase.Instance.emptyItem);
            }
            //Debug.Log((boxIndex - 1) * 100 + i);
        }

        boxIndexText.text = index.ToString() + " / " + boxMaxIndex.ToString();

    }

    private void Update()
    {
        if (isInvenOpen)
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
            if (boxIndex > boxMaxIndex)
            {
                boxIndex = boxMaxIndex;
            }
            NextBox(boxIndex);
            Debug.Log("E");
        }

        //Debug.Log("���¿Գ�");
    }


    public void SelectTag(ItemType tag)
    {
        selectBoxTag = tag;
        boxIndex = 1;
        NextBox(boxIndex);
    }

    public void OpenBox()
    {

        boxIndex = 1;
        InvenOpen();
        if (items.Count > 0)
        {
            NextBox(boxIndex);
        }
    }
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

}
