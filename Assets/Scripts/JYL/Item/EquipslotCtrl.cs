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
    public EquipSlot Type => type;

    private Coroutine fadeCoroutine;


    public void OnPointerClick(PointerEventData eventData)
    {
        var ctrl = InvenToryCtrl.Instance;

        if (type != EquipSlot.box)
        {
            ctrl.ChangeItemByKey(ctrl.equippedInventory, ctrl.equipInventory, item.id);
        }
        else
        {
            UIManager.Instance.StackUIOpen(UIType.EquipInvenUI);
        }

        InvenToryCtrl.Instance.OnInventoryChanged?.Invoke();

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
        else
        {
            ctrl.EquipItemToolTipCtrl.TooltipClear(false, ItemType.Empty);
        }

        fadeCoroutine = StartCoroutine(FadeAlphaLoop(icon));

        Debug.Log(type);
    }


    IEnumerator FadeAlphaLoop(Image targetImage)
    {
        bool fadingOut = true; // 처음에는 알파값을 낮추는 방향
        float alpha = 1f; // 초기 알파값

        float alphaSpeed = 2f; // 알파 변화 속도
        float minAlpha = 0.3f; // 최소 알파값 (0~1)

        while (true) // 무한 루프
        {
            alpha += (fadingOut ? -1 : 1) * alphaSpeed * Time.deltaTime;

            if (alpha <= minAlpha)
            {
                alpha = minAlpha;
                fadingOut = false; // 증가 방향
            }
            else if (alpha >= 1f)
            {
                alpha = 1f;
                fadingOut = true; // 감소 방향
            }

            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, alpha);
            yield return null;
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        InvenToryCtrl.Instance.EquipItemToolTipCtrl.TooltipClear(false, item.type);
        //실행되던 코루틴 정지
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

        if (type == EquipSlot.box)
        {
            item = ItemDataBase.Instance.emptyItem;
        }
    }

    public void SlotListSetting(BaseItem slotitem)
    {
        if (!IsCorrectType(slotitem))
        {
            Debug.LogWarning($"[장착 실패] {item.name}은 {type} 슬롯에 장착할 수 없습니다.");
            return;
        }

        item = slotitem;
        icon.sprite = slotitem.image;
        icon.color = slotitem.color;
        itemName.text = slotitem.name;
    }

    public bool IsCorrectType(BaseItem item)
    {
        if (item == null || item.type == ItemType.Empty)
            return true; // 빈 아이템은 예외로 허용

        if (item is Armor armor)
            return armor.equipType == type;

        if (item is Weapon)
            return type == EquipSlot.Weapon;

        return false;
    }


    public void ClearSlot()
    {
        icon.color = new Color(1, 1, 1, 1);
        //  icon.sprite = sprites[0];
    }
}
