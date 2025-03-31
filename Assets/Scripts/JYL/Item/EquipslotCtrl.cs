using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipslotCtrl : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image icon;
    [SerializeField] Text itemName;
    BaseItem item;

    EquipSlot type;

    private Coroutine fadeCoroutine;


    public void OnPointerClick(PointerEventData eventData)
    {
        var ctrl = InvenToryCtrl.Instance;

        if(type != EquipSlot.end)
        {
            ctrl.ChangeItemByKey(ctrl.equippedInventory, ctrl.equipInventory, item.id);
        }
        else
        {
            UIManager.Instance.StackUIOpen(UIType.EquipInvenUI);
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var ctrl = InvenToryCtrl.Instance;

        if (item.type == ItemType.Weapon)
        {
            ctrl.EquipItemToolTipCtrl.SetWeapon(item as Weapon);
        }
        else if (item.type == ItemType.Armor)
        {
            ctrl.EquipItemToolTipCtrl.SetArmor(item as Armor);
        }

        fadeCoroutine = StartCoroutine(FadeAlphaLoop(icon));
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


    public void OnPointerExit(PointerEventData eventData)
    {
        InvenToryCtrl.Instance.EquipItemToolTipCtrl.TooltipClear(false, item.type);
        //����Ǵ� �ڷ�ƾ ����
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        ClearSlot();
    }

    public void SetType(EquipSlot slottype)
    {
        type = slottype;
    }

    public void SlotListSetting(BaseItem slotitem)
    {
        item = slotitem;
        icon.sprite = slotitem.image;
        icon.color = slotitem.color;
        itemName.text = slotitem.name;
    }
    public void ClearSlot()
    {
        icon.color = new Color(1, 1, 1, 1);
      //  icon.sprite = sprites[0];
    }
}
