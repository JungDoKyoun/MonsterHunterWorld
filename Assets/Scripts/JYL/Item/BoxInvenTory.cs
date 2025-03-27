using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//사물함 인벤토리
public class BoxInvenTory : BaseInventory, IClosableUI
{

    //현재 사물함 인덱스
    int boxIndex = 1;
    //사물함 최대갯수
    int boxMaxIndex = 10;

    [SerializeField]
    Text boxIndexText;

    //현재 선택된 사물함 태그
    ItemType selectBoxTag;

    public bool IsOpen => gameObject.activeSelf;

    private void Start()
    {
        invenType = InvenType.Box;

        SlotSetting(gameObject, invenType);

        InvenInit();

        //Debug.Log("박스인벤 시작");
    }

    public void AddItem(BaseItem item, int index)
    {
        if (index < 0 || index >= 1000)
        {
            Debug.Log("인덱스 잘못넣었습니다.");
            return;
        }

        items[index] = item;
    }

    override public void RefreshUI()
    {
        for (int i = 0; i < 100; i++)
        {
            var slotComp = slot[i].GetComponent<ItemSlot>();
            if (slotComp == null)
            {
                Debug.LogWarning($"슬롯 {i}번에 ItemSlot 컴포넌트가 없습니다.");
                continue;
            }

            int targetIndex = (boxIndex - 1) * 100 + i;

            if (targetIndex < items.Count && items[targetIndex] != null)
            {
                slotComp.SlotSetItem(items[targetIndex]);
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
            //Debug.Log("박스인벤 업데이트");
        }
    }

    public void BoxInput()
    {
        //사물함 왼쪽
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
        //사물함 오른쪽
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

        //Debug.Log("들어는왔나");
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

    private void OnEnable()
    {
       // RefreshUI();
        UIManager.Instance.RegisterUI(this);
    }

    private void OnDisable()
    {
        UIManager.Instance.UnregisterUI(this);
    }
}
