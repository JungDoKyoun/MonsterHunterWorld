using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<BaseItem> onClick;

    InvenType invenType;

    //������ �̹��� 1�� 
    //������ �ƴ� �̹��� 0��
    [SerializeField]
    Sprite[] sprites = new Sprite[2];

    Image image;

    BaseItem item;

    public GameObject itemImage;
    public Text countText;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        item = ItemDataBase.Instance.emptyItem;

        image = gameObject.GetComponent<Image>();
    }

    public void SetInvenType(InvenType type)
    {
        invenType = type;
    }

    public void SlotSetItem(BaseItem value)
    {
        //if (value == null)
        //{
        //    Debug.LogError("�� SlotSetItem: value�� null�Դϴ�.");
        //    return;
        //}

        item = value;

        //if (itemImage == null)
        //{
        //    Debug.LogError("�� itemImage�� null�Դϴ�. �ʱ�ȭ ���� ȣ����� �� ����.");
        //    return;
        //}


        var img = itemImage.GetComponent<Image>();

        //if (img == null)
        //{
        //    Debug.LogError($"�� itemImage '{itemImage.name}' �� Image ������Ʈ�� �����ϴ�.");
        //    return;
        //}

        //if (item.image == null)
        //{
        //    Debug.LogWarning("�� item.image�� null�Դϴ�.");
        //    return;
        //}



        //Debug.Log($" SlotSetItem ����: {item.name}");

        img.sprite = item.image;
        img.color = item.color;


        if (value.type > ItemType.Accessory)
        {
            countText.text = (item.count > 0) ? item.count.ToString() : "";
        }

        //�� �ڵ�� ��������
        //if (item.count > 0)
        //{
        //    countText.text = item.count.ToString();
        //}
        //else
        //{
        //    countText.text = "";
        //}
    }

    //������ Ŭ���� �ش� ������ �ݴ��� �κ��丮�� �ѱ��
    public void OnPointerClick(PointerEventData eventData)
    {
        var ctrl = InvenToryCtrl.Instance;

        switch (invenType)
        {
            case InvenType.Inven:                
                ctrl.ChangeItemByKey(ctrl.inventory, ctrl.boxInven, item.id);
                break;
            case InvenType.Box:                
                ctrl.ChangeItemByKey(ctrl.boxInven, ctrl.inventory, item.id);
                break;

            case InvenType.EquipBox:
                ctrl.ChangeItemByKey(ctrl.equipInventory, ctrl.equippedInventory, item.id);
                break;
            default:
                Debug.LogError("�߸��� Ÿ���Դϴ�.");
                break;
        }
        if (item.count <= 0)
        {
            item = ItemDataBase.Instance.emptyItem;
            SlotSetItem(item);
        }

        InvenToryCtrl.Instance.OnInventoryChanged?.Invoke();
    }

    //������ ���Կ� ���콺 ���� ������ ���� ����
    public void OnPointerEnter(PointerEventData eventData)
    {
        //���߿� ��¦��¦ �ҿ���
        var image = gameObject.GetComponent<Image>();
        var ctrl = InvenToryCtrl.Instance;

        image.sprite = sprites[1];
        switch (invenType)
        {
            case InvenType.Inven:
            case InvenType.Box:
                ctrl.ItemToolTipCtrl.ToolTipSetItem(item);
                break;

            case InvenType.Equipped:
            case InvenType.EquipBox:

                if (item.type == ItemType.Weapon)
                {
                    ctrl.EquipItemToolTipCtrl.SetWeapon(item as Weapon);
                }
                else if (item.type == ItemType.Armor)
                {
                    ctrl.EquipItemToolTipCtrl.SetArmor(item as Armor);
                }
                else
                {
                    ctrl.EquipItemToolTipCtrl.TooltipClear(false, ItemType.Empty);
                }
                    break;
            default:
                Debug.LogError("�߸��� Ÿ���Դϴ�.");
                break;

        }

        Debug.Log(item.name);

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

        if (invenType == InvenType.Inven || invenType == InvenType.Box)
        {
            //���� ���õ� ������ �������� �ƴϸ� �ʱ�ȭ
            InvenToryCtrl.Instance.ItemToolTipCtrl.TooltipClear(false);
        }
        else
        {
            InvenToryCtrl.Instance.EquipItemToolTipCtrl.TooltipClear(false, item.type);
        }
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
