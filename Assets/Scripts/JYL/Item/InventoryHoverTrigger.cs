using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ChangeBox arrowFx;

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrowFx.OnInventoryHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        arrowFx.OnExit();
    }
}
