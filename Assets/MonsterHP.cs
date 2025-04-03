using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MonsterHP : MonoBehaviour
{
    Text hp;
    MonsterController target;

    void Start()
    {
        hp = GetComponent<Text>();  
        target = FindObjectOfType<MonsterController>();
        hp.text = target.CurrentHP.ToString();
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
