using UnityEngine;
using UnityEngine.EventSystems;

public class BoxHoverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ChangeBox changeBox;

    public void OnPointerEnter(PointerEventData eventData)
    {
        changeBox.OnBoxHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        changeBox.OnExit();
    }
}
