using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangeInputField : MonoBehaviour
{
    EventSystem system;

    [SerializeField] Selectable emailInput;

    private void Start()
    {
        system = EventSystem.current;

        emailInput.Select();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {
                next.Select();
            }
        }
    }
}
