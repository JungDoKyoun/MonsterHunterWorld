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
        var image = gameObject.GetComponent<Image>();

        image.sprite = sprites[1];

        tooltipBox.ToolTipSetItem(item);

        //exit���� ���� �ص� �Ǵµ� �ؼ� �ϴ� �ּ���
        //if (fadeCoroutine != null)
        //{
        //    StopCoroutine(fadeCoroutine);
        //}

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
        var img = gameObject.GetComponent<Image>();

        //���� ���õ� ������ �������� �ƴϸ� �ʱ�ȭ
        tooltipBox.TooltipClear(false);
        img.color = new Color(1, 1, 1, 1);
        img.sprite = sprites[0];
        //����Ǵ� �ڷ�ƾ ����
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }
}
