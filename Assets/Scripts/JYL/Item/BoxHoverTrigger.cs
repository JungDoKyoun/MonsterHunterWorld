using UnityEngine;
using UnityEngine.EventSystems;

public class BoxHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ChangeBox arrowFx;

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrowFx.OnBoxHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        arrowFx.OnExit();
    }
}
