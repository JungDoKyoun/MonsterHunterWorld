using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<BaseItem> onClick;

    InvenType invenType;

    //선택중 이미지 1번 
    //선택중 아닌 이미지 0번
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
        //    Debug.LogError("▶ SlotSetItem: value가 null입니다.");
        //    return;
        //}

        item = value;

        //if (itemImage == null)
        //{
        //    Debug.LogError("▶ itemImage가 null입니다. 초기화 전에 호출됐을 수 있음.");
        //    return;
        //}


        var img = itemImage.GetComponent<Image>();

        //if (img == null)
        //{
        //    Debug.LogError($"▶ itemImage '{itemImage.name}' 에 Image 컴포넌트가 없습니다.");
        //    return;
        //}

        //if (item.image == null)
        //{
        //    Debug.LogWarning("▶ item.image가 null입니다.");
        //    return;
        //}



        //Debug.Log($" SlotSetItem 성공: {item.name}");

        img.sprite = item.image;
        img.color = item.color;


        if (value.type > ItemType.Accessory)
        {
            countText.text = (item.count > 0) ? item.count.ToString() : "";
        }

        //위 코드로 수정했음
        //if (item.count > 0)
        //{
        //    countText.text = item.count.ToString();
        //}
        //else
        //{
        //    countText.text = "";
        //}
    }

    //아이템 클릭시 해당 아이템 반대편 인벤토리로 넘기기
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
                Debug.LogError("잘못된 타입입니다.");
                break;
        }
        if (item.count <= 0)
        {
            item = ItemDataBase.Instance.emptyItem;
            SlotSetItem(item);
        }

        InvenToryCtrl.Instance.OnInventoryChanged?.Invoke();
    }

    //아이템 슬롯에 마우스 갔다 댔을때 툴팁 띄우기
    public void OnPointerEnter(PointerEventData eventData)
    {
        //나중에 반짝반짝 할예정
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
                Debug.LogError("잘못된 타입입니다.");
                break;

        }

        Debug.Log(item.name);

        fadeCoroutine = StartCoroutine(FadeAlphaLoop(image));

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

    //아이템 슬롯에 마우스 빠져나갔을때 툴팁 끄고 원래대로
    public void OnPointerExit(PointerEventData eventData)
    {

        if (invenType == InvenType.Inven || invenType == InvenType.Box)
        {
            //현재 선택된 정보가 아이템이 아니면 초기화
            InvenToryCtrl.Instance.ItemToolTipCtrl.TooltipClear(false);
        }
        else
        {
            InvenToryCtrl.Instance.EquipItemToolTipCtrl.TooltipClear(false, item.type);
        }
        //실행되던 코루틴 정지
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
