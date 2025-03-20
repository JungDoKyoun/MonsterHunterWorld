using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvenTory : MonoBehaviour
{
    List<BaseItem> items = new List<BaseItem>();
    List<GameObject> slot = new List<GameObject>();
    void Start()
    {
        
        //�׽�Ʈ�� ������ �߰�
        foreach (var item in ItemDataBase.Instance.items.ToList())
        {
            items.Add(item);
            //Debug.Log(item.name);
        }     
        
        //�ڽ� ������Ʈ ����
        var objs = GetComponentsInChildren<ItemSlot>();
        foreach (var item in objs)
        {            
            slot.Add(item.gameObject);            
        }
      

        //�������ִ� �������� �ִ°��
        if (items.Count > 0)
        {
            for (int i = 0; i < items.Count; i++)
            {
                slot[i].GetComponent<ItemSlot>().SetItem(items[i]);
            }
        }
        //������ ���� �ϸ��

    }



}
