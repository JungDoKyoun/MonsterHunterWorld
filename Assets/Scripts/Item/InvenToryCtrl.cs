using System.Collections.Generic;
using UnityEngine;

public enum InvenType
{
    Inven,
    Box
}

//플레이어의 모든 인벤토리
public class InvenToryCtrl : MonoBehaviour
{
    //현재 가지고있을 인벤토리
    public InventoryItems inventoryItems;
    //사물함 인벤토리
    public BoxInvenTory boxInvenTory;
    



    public static InvenToryCtrl Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    //리스트내의 아이템 갯수 추가
    public void AddItemByName(List<BaseItem> itemList, string targetName)
    {
        foreach (BaseItem item in itemList)
        {
            if (item.name == targetName)
            {
                item.count = Mathf.Min(item.count + 1, item.maxCount); // 최대치 초과 방지
                Debug.Log($"아이템 '{item.name}' 수량 증가: {item.count}/{item.maxCount}");
                return;
            }
        }

        Debug.LogWarning($"'{targetName}' 이름의 아이템을 리스트에서 찾을 수 없습니다.");
    }

    //아이템 이동
    public void ChangeItem(InvenType type, BaseItem item)
    {
        //인벤토리에서 아이템을 선택했을때
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
        //사물함에서 아이템을 선택했을때
        else
        {
            //아이템이 있으면
            //아이템을 인벤토리로 이동
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
                                                                                                              

        Debug.Log("사물함 아이템 종류 : " + boxInvenTory.BoxItems.Count );
    }

}
