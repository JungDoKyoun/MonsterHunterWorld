using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvenTory : MonoBehaviour
{
    List<BaseItem> items = new List<BaseItem>();
    List<GameObject> slot = new List<GameObject>();
    void Start()
    {
        
        //테스트용 아이템 추가
        foreach (var item in ItemDataBase.Instance.items.ToList())
        {
            items.Add(item);
            //Debug.Log(item.name);
        }     
        
        //자식 오브젝트 연결
        var objs = GetComponentsInChildren<ItemSlot>();
        foreach (var item in objs)
        {            
            slot.Add(item.gameObject);            
        }
      

        //가지고있는 아이템이 있는경우
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SetItem(items[i]);
            }
        }
        //없으면 고대로 하면됨

    }



}
