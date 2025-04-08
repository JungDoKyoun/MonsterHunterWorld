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
            Debug.Log(ctrl.equipInventory.Count);
            ctrl.ChangeItemByKey(ctrl.equippedInventory, ctrl.equipInventory, item.id,InvenType.Equipped);
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
        
        ////현재 장착된 장비들 체크
        //for (int i = 0; i < ctrl.equippedInventory.Count-1; i++)
        //{
        //    Debug.Log(ctrl.equippedInventory[i].name);
        //}


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
            item = ItemDataBase.Instance.EmptyItem;
        }
    }

    public void SlotListSetting(BaseItem slotitem)
    {
        if (slotitem == null)
        {
            Debug.LogWarning("[EquipslotCtrl] slotitem이 null입니다.");
            return;
        }

        // 현재 슬롯에 안 맞으면 자동으로 맞는 슬롯 찾기
        if (!IsCorrectType(slotitem))
        {
            Debug.LogWarning($"[장착 실패] {slotitem.name}은 {type} 슬롯에 장착할 수 없습니다. 자동으로 올바른 슬롯 탐색 시도.");

            // 타입이 Armor일 경우 해당 타입에 맞는 슬롯 찾기
            if (slotitem is Armor armor)
            {
                int correctIndex = (int)armor.equipType;
                if (correctIndex < InvenToryCtrl.Instance.equippedUiSlot.Length)
                {
                    InvenToryCtrl.Instance.equippedUiSlot[correctIndex].SlotListSetting(slotitem);

                    return;
                }
            }
            // 무기일 경우 Weapon 슬롯으로
            else if (slotitem is Weapon)
            {
                InvenToryCtrl.Instance.equippedUiSlot[(int)EquipSlot.Weapon].SlotListSetting(slotitem);
                return;
            }

            return;
        }

        // 정상적인 경우 장착 처리
        item = slotitem;

        if (icon != null)
        {
            icon.sprite = slotitem.image;
            icon.color = slotitem.color;
        }

        if (itemName != null)
        {
            itemName.text = slotitem.name;
        }
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
