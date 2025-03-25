using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;


//사물함 인벤토리
public class BoxInvenTory : BaseInventory
{

    //현재 사물함 인덱스
    int boxIndex = 1;
    //사물함 최대갯수
    int boxMaxIndex = 10;
 
    [SerializeField]
    Text boxIndexText;

    //현재 선택된 사물함 태그
    ItemType selectBoxTag;


    public List<BaseItem> BoxItems
    {
        get => items;
        set => items = value;
    }

    private void Start()
    {
        invenType = InvenType.Box;

        SlotSetting(invenType);

        Debug.Log("박스인벤 시작");
    }

    public void AddItem(BaseItem item , int index)
    {
        if(index < 0 || index >= 1000)
        {
            Debug.Log("인덱스 잘못넣었습니다.");
            return;
        }

        items[index] = item;
    }


    public void NextBox(int index)
    {
        boxIndex = index;

        for (int i = 0; i < 100; i++)
        {
            if (items[(boxIndex - 1) * 100 + i] != null)
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
        boxIndex = 0;
        NextBox(boxIndex);
    }

    public void OpenBox()
    {
        
        boxIndex = 1;
        InvenOpen();
        if (BoxItems.Count > 0)
        {
            NextBox(boxIndex);
        }
    }

}
