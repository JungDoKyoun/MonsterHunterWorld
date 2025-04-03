using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Button buyButton;

    private BaseItem currentItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void SetItem(BaseItem item)
    {
        currentItem = item;
        itemImage.sprite = item.image;
        itemImage.color = item.color;
        itemNameText.text = item.name;
        priceText.text = $"{item.price} G";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuyItem());
    }

    void BuyItem()
    {
        Debug.Log($"{currentItem.name} 구매 시도");

        // 여기에 인벤토리 추가 및 골드 차감 로직 작성
        InvenToryCtrl.Instance.inventory.Add(currentItem.Clone()); // 복사본 생성 필요

        var isBuy = InvenToryCtrl.Instance.GetGold - currentItem.price >= 0
            ? true : false;

        if (isBuy)
        {
            InvenToryCtrl.Instance.SetGold(-currentItem.price); // 해당 금액만큼 차감
        }
    }
}
