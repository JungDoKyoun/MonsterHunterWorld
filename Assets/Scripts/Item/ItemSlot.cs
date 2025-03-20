using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    InvenType invenType;

    //선택중 이미지 1번 
    //선택중 아닌 이미지 0번
    [SerializeField]
    Sprite[] sprites = new Sprite[2];

    BaseItem item;
    public BaseItem Item => item;

    public GameObject itemImage;
    public Text countText;


    ItemToolTipCtrl tooltipBox;

    private void Start()
    {
        item = ItemDataBase.Instance.emptyItem;
        //SlotSetItem(item);
    }

    public void SetInvenType(InvenType type)
    {
        invenType = type;
    }

    public void SlotSetItem(BaseItem value)
    {
        item = value;
        var img = itemImage.GetComponent<Image>();

        //Debug.Log(img.name);
        //Debug.Log(1 +". " + item.name);
        //Debug.Log(img.color);
        //Debug.Log(value.color);

        //Debug.Log(img.material.color);
        //img.material.color = value.color; // 기본 머티리얼 제거
        img.sprite = item.image;
        img.color = item.color;

        //Debug.Log(2 + ". " + item.name);
        //Debug.Log(img.color);
        //Debug.Log(value.color);

        if (item.count > 0)
        {
            countText.gameObject.SetActive(true);
            countText.text = item.count.ToString();
        }
        else
        {
            countText.gameObject.SetActive(false);
        }


    }

    //아이템 클릭시 해당 아이템 창고로 넘기기
    public void OnPointerClick(PointerEventData eventData)
    {
        item.count--;
        if (item.count <= 0)
        {
            item = ItemDataBase.Instance.emptyItem;
        }

        InvenToryCtrl.Instance.ChangeItem(invenType, item);


    }

    //아이템 슬롯에 마우스 갔다 댔을때 툴팁 띄우기
    public void OnPointerEnter(PointerEventData eventData)
    {
        //나중에 반짝반짝 할예정
        gameObject.GetComponent<Image>().sprite = sprites[1];

        tooltipBox = GameObject.Find("ItemTooltipBox").GetComponent<ItemToolTipCtrl>();

        tooltipBox.ToolTipSetItem(item);
    }

    //아이템 슬롯에 마우스 빠져나갔을때 툴팁 끄고 원래대로
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().sprite = sprites[0];
        //현재 선택된 정보가 아이템이 아니면 초기화
        tooltipBox.TooltipClear();
    }
}
