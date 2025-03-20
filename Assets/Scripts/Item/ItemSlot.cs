using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    InvenType invenType;

    //������ �̹��� 1�� 
    //������ �ƴ� �̹��� 0��
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
        //img.material.color = value.color; // �⺻ ��Ƽ���� ����
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

    //������ Ŭ���� �ش� ������ â��� �ѱ��
    public void OnPointerClick(PointerEventData eventData)
    {
        item.count--;
        if (item.count <= 0)
        {
            item = ItemDataBase.Instance.emptyItem;
        }

        InvenToryCtrl.Instance.ChangeItem(invenType, item);


    }

    //������ ���Կ� ���콺 ���� ������ ���� ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        //���߿� ��¦��¦ �ҿ���
        gameObject.GetComponent<Image>().sprite = sprites[1];

        tooltipBox = GameObject.Find("ItemTooltipBox").GetComponent<ItemToolTipCtrl>();

        tooltipBox.ToolTipSetItem(item);
    }

    //������ ���Կ� ���콺 ������������ ���� ���� �������
    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject.GetComponent<Image>().sprite = sprites[0];
        //���� ���õ� ������ �������� �ƴϸ� �ʱ�ȭ
        tooltipBox.TooltipClear();
    }
}
