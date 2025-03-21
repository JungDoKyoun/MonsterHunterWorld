using System.Collections.Generic;
using UnityEngine;


//사물함 인벤토리
public class BoxInvenTory : MonoBehaviour
{
    InvenType invenType = InvenType.Box;

    //현재 사물함 인덱스
    int boxIndex = 1;
    //사물함 최대갯수
    int boxSize = 10;
    //사물함 인벤토리
    List<BaseItem> boxItems = new List<BaseItem>();

    //사물함 슬롯
    List<GameObject> slot = new List<GameObject>();

    //현재 선택된 사물함 태그
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
        //자식 오브젝트 연결
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
        //사물함 왼쪽
        if (Input.GetKeyDown(KeyCode.Q))
        {
            boxIndex--;
            if (boxIndex <= 0)
            {
                boxIndex = 1;
            }
            NextBox(boxIndex);
        }
        //사물함 오른쪽
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
