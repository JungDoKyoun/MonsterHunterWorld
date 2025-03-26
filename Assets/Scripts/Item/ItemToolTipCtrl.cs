using UnityEngine;
using UnityEngine.UI;

public class ItemToolTipCtrl : MonoBehaviour
{
    //참조해올 오브젝트들
    BaseItem item;
    [SerializeField]
    GameObject image;
    [SerializeField]
    GameObject itemName;
    [SerializeField]
    GameObject toolTip;
    [SerializeField]
    GameObject rarity;
    [SerializeField]
    GameObject gold;
    [SerializeField]
    GameObject sellGold;
    [SerializeField]
    GameObject count;
    [SerializeField]
    GameObject slash;
    [SerializeField]
    GameObject maxCount;
    [SerializeField]
    GameObject allCount;

    //적용시킬 값들
    Text itemNameText;
    Text tooltipText;
    Text rarityText;
    Text goldText;
    Text countText;
    Text maxCountText;
    Text allCountText;
    Image itemImage;

    private void Awake()
    {
        //겟컴포넌트를 최소화 하기위한 일.
        itemNameText = itemName.GetComponent<Text>();
        tooltipText = toolTip.GetComponent<Text>();
        rarityText = rarity.GetComponent<Text>();
        goldText = gold.GetComponent<Text>();
        countText = count.GetComponent<Text>();
        maxCountText = maxCount.GetComponent<Text>();
        allCountText = allCount.GetComponent<Text>();
        itemImage = image.GetComponent<Image>();

        //초기화 - 처음엔 아무것도 없으니까 비활성화
        TooltipClear(false);

    }

    public void TooltipClear(bool set)
    {
        //아무것도 없는 아이템이면 비활성화
        image.SetActive(set);
        itemName.SetActive(set);
        toolTip.SetActive(set);
        rarity.SetActive(set);
        gold.SetActive(set);
        sellGold.SetActive(set);
        count.SetActive(set);
        slash.SetActive(set);
        maxCount.SetActive(set);
        allCount.SetActive(set);
    }

    //자주 불릴예정이라 컴포넌트를 미리 정의해줬음
    public void ToolTipSetItem(BaseItem value)
    {
        //아무것도 없는 아이템이면 비활성화
        if (value.name == "")
        {
            TooltipClear(false);
            return;
        }
        //아이템이 있으면 활성화
        else
        {
            TooltipClear(true);
        }

        //값 적용
        item = value;
        itemNameText.text = item.name;
        tooltipText.text = item.tooltip;
        itemImage.sprite = item.image;
        itemImage.color = item.color;
        rarityText.text = item.rarity.ToString();
        goldText.text = item.price.ToString();
        countText.text = item.count.ToString();

        maxCountText.text = item.maxCount.ToString();
        allCountText.text = item.allCount.ToString();

        Debug.Log("? : " + value.GetType());
    }

}
