using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonsterHP : MonoBehaviour
{
    [SerializeField] Text hp;
    MonsterController target;

    public void SetTarget(MonsterController monster)
    {
        target = monster;
        hp.text = target.CurrentHP.ToString();
    }

    void Start()
    {
        Debug.Log(hp);

    }


    private void FixedUpdate()
    {
        if (target != null)
        {
            hp.text = target.CurrentHP.ToString();
        }
        else
        {
            Debug.Log("¿¬°á¾ÈµÊ");
            target = FindObjectOfType<MonsterController>();
        }
    }
    
}
