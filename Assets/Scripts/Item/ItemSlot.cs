using System.Collections;
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

    Image image;

    BaseItem item;
    public BaseItem Item => item;

    public GameObject itemImage;
    public Text countText;


    ItemToolTipCtrl tooltipBox;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        item = ItemDataBase.Instance.emptyItem;
        tooltipBox = GameObject.Find("ItemTooltipBox").GetComponent<ItemToolTipCtrl>();

        //SlotSetItem(item);

        image = gameObject.GetComponent<Image>();
    }

    public void SetInvenType(InvenType type)
    {
        invenType = type;
    }

    public void SlotSetItem(BaseItem value)
    {
        if (value == null)
        {
            Debug.LogError("�� SlotSetItem: value�� null�Դϴ�.");
            return;
        }

        item = value;

        if (itemImage == null)
        {
            Debug.LogError("�� itemImage�� null�Դϴ�. �ʱ�ȭ ���� ȣ����� �� ����.");
            return;
        }


        var img = itemImage.GetComponent<Image>();

        if (img == null)
        {
            Debug.LogError($"�� itemImage '{itemImage.name}' �� Image ������Ʈ�� �����ϴ�.");
            return;
        }

        if (item.image == null)
        {
            Debug.LogWarning("�� item.image�� null�Դϴ�.");
            return;
        }



        Debug.Log($" SlotSetItem ����: {item.name}");

        img.sprite = item.image;
        img.color = item.color;
        
        if (item.count > 0)
        {
            countText.text = item.count.ToString();
        }
        else
        {
            countText.text = "";
        }
    }

    //������ Ŭ���� �ش� ������ â��� �ѱ��
    public void OnPointerClick(PointerEventData eventData)
    {
        //������ġ ����
        //item.count--;
        
        InvenToryCtrl.Instance.ChangeItem(invenType, item);
        

        if (item.count <= 0)
        {
            item = ItemDataBase.Instance.emptyItem;
            SlotSetItem(item);
        }


        countText.text = item.count.ToString();
        tooltipBox.ToolTipSetItem(item);

    }

    //������ ���Կ� ���콺 ���� ������ ���� ����
    public void OnPointerEnter(PointerEventData eventData)
    {


        //���߿� ��¦��¦ �ҿ���
        var image = gameObject.GetComponent<Image>();

        image.sprite = sprites[1];

        tooltipBox.ToolTipSetItem(item);

        fadeCoroutine = StartCoroutine(FadeAlphaLoop(image));

    }

    IEnumerator FadeAlphaLoop(Image targetImage)
    {
        bool fadingOut = true; // ó������ ���İ��� ���ߴ� ����
        float alpha = 1f; // �ʱ� ���İ�

        float alphaSpeed = 2f; // ���� ��ȭ �ӵ�
        float minAlpha = 0.3f; // �ּ� ���İ� (0~1)

        while (true) // ���� ����
        {
            alpha += (fadingOut ? -1 : 1) * alphaSpeed * Time.deltaTime;

            if (alpha <= minAlpha)
            {
                alpha = minAlpha;
                fadingOut = false; // ���� ����
            }
            else if (alpha >= 1f)
            {
                alpha = 1f;
                fadingOut = true; // ���� ����
            }

            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, alpha);
            yield return null;
        }
    }

    //������ ���Կ� ���콺 ������������ ���� ���� �������
    public void OnPointerExit(PointerEventData eventData)
    {


        //���� ���õ� ������ �������� �ƴϸ� �ʱ�ȭ
        tooltipBox.TooltipClear(false);

        //����Ǵ� �ڷ�ƾ ����
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
        ClearSlot();
    }

    public void ClearSlot()
    {
        image.color = new Color(1, 1, 1, 1);
        image.sprite = sprites[0];
    }
}
