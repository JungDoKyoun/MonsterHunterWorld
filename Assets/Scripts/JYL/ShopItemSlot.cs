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
        Debug.Log($"{currentItem.name} ���� �õ�");

        // ���⿡ �κ��丮 �߰� �� ��� ���� ���� �ۼ�
        InvenToryCtrl.Instance.inventory.Add(currentItem.Clone()); // ���纻 ���� �ʿ�

        var isBuy = InvenToryCtrl.Instance.GetGold - currentItem.price >= 0
            ? true : false;

        if (isBuy)
        {
            InvenToryCtrl.Instance.SetGold(-currentItem.price); // �ش� �ݾ׸�ŭ ����
        }
    }
}
