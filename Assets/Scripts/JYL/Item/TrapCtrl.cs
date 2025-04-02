using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCtrl : MonoBehaviour
{
    [SerializeField] GameObject end;   // 활성화할 오브젝트

    List<GameObject> endList = new List<GameObject>();

    void Start()
    {
        end.SetActive(false);

        foreach (Transform c in transform)
        {
            endList.Add(c.gameObject);
        }
    }

    // 이 메서드는 애니메이션 이벤트로 호출됩니다.
    public void OnStartAnimationEnd()
    {
        // end 오브젝트를 활성화
        if (end != null)
        {
            end.SetActive(true);
            Debug.Log("함정 오브젝트가 활성화되었습니다.");            
        }
        
    }

    private void OnDestroy()
    {
        end.SetActive(false);
    }

}