using UnityEngine;
using UnityEngine.UI;

public class ItemToolTipCtrl : MonoBehaviour
{
    //�����ؿ� ������Ʈ��
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

    //�����ų ����
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
        //��������Ʈ�� �ּ�ȭ �ϱ����� ��.
        itemNameText = itemName.GetComponent<Text>();
        tooltipText = toolTip.GetComponent<Text>();
        rarityText = rarity.GetComponent<Text>();
        goldText = gold.GetComponent<Text>();
        countText = count.GetComponent<Text>();
        maxCountText = maxCount.GetComponent<Text>();
        allCountText = allCount.GetComponent<Text>();
        itemImage = image.GetComponent<Image>();

        //�ʱ�ȭ - ó���� �ƹ��͵� �����ϱ� ��Ȱ��ȭ
        TooltipClear(false);

    }

    public void TooltipClear(bool set)
    {
        //�ƹ��͵� ���� �������̸� ��Ȱ��ȭ
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

    //���� �Ҹ������̶� ������Ʈ�� �̸� ����������
    public void ToolTipSetItem(BaseItem value)
    {
        //�ƹ��͵� ���� �������̸� ��Ȱ��ȭ
        if (value.name == "")
        {
            TooltipClear(false);
            return;
        }
        //�������� ������ Ȱ��ȭ
        else
        {
            TooltipClear(true);
        }

        //�� ����
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
