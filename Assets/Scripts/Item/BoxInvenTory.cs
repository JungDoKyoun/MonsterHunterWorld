using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;


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

    [SerializeField]
    Text boxIndexText;

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

        for(int i = 0; i < 1000; i++)
        {
            boxItems.Add(ItemDataBase.Instance.emptyItem);
        }

        //for (int i = 0; i < 30; i++)
        //{
        //    SetItem(index: i, item: ItemDataBase.Instance.GetItem((int)ItemImageNumber.RecoveryPotion));
        //}

        Debug.Log("박스인벤 시작");
    }

    public void SetItem(BaseItem item , int index)
    {
        if(index < 0 || index >= 1000)
        {
            Debug.Log("인덱스 잘못넣었습니다.");
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
            if (boxIndex > boxSize)
            {
                boxIndex = boxSize;
            }
            NextBox(boxIndex);
            Debug.Log("E");
        }

        //Debug.Log("들어는왔나");
    }


    public void SelectTag(ItemType tag)
    {
        boxTag = tag;
        boxIndex = 0;
        NextBox(boxIndex);
    }


}
