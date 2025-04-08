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
        
        ////���� ������ ���� üũ
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

        if (type == EquipSlot.box)
        {
            item = ItemDataBase.Instance.EmptyItem;
        }
    }

    public void SlotListSetting(BaseItem slotitem)
    {
        if (slotitem == null)
        {
            Debug.LogWarning("[EquipslotCtrl] slotitem�� null�Դϴ�.");
            return;
        }

        // ���� ���Կ� �� ������ �ڵ����� �´� ���� ã��
        if (!IsCorrectType(slotitem))
        {
            Debug.LogWarning($"[���� ����] {slotitem.name}�� {type} ���Կ� ������ �� �����ϴ�. �ڵ����� �ùٸ� ���� Ž�� �õ�.");

            // Ÿ���� Armor�� ��� �ش� Ÿ�Կ� �´� ���� ã��
            if (slotitem is Armor armor)
            {
                int correctIndex = (int)armor.equipType;
                if (correctIndex < InvenToryCtrl.Instance.equippedUiSlot.Length)
                {
                    InvenToryCtrl.Instance.equippedUiSlot[correctIndex].SlotListSetting(slotitem);

                    return;
                }
            }
            // ������ ��� Weapon ��������
            else if (slotitem is Weapon)
            {
                InvenToryCtrl.Instance.equippedUiSlot[(int)EquipSlot.Weapon].SlotListSetting(slotitem);
                return;
            }

            return;
        }

        // �������� ��� ���� ó��
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
            return true; // �� �������� ���ܷ� ���

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
