using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCtrl : MonoBehaviour
{
    [SerializeField] GameObject end;   // Ȱ��ȭ�� ������Ʈ

    List<GameObject> endList = new List<GameObject>();

    void Start()
    {
        end.SetActive(false);

        foreach (Transform c in transform)
        {
            endList.Add(c.gameObject);
        }
    }

    // �� �޼���� �ִϸ��̼� �̺�Ʈ�� ȣ��˴ϴ�.
    public void OnStartAnimationEnd()
    {
        // end ������Ʈ�� Ȱ��ȭ
        if (end != null)
        {
            end.SetActive(true);
            Debug.Log("���� ������Ʈ�� Ȱ��ȭ�Ǿ����ϴ�.");            
        }
        
    }

    private void OnDestroy()
    {
        end.SetActive(false);
    }

}