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
        image.SetActive(false);
        itemName.SetActive(false);
        toolTip.SetActive(false);
        rarity.SetActive(false);
        gold.SetActive(false);
        sellGold.SetActive(false);
        count.SetActive(false);
        slash.SetActive(false);
        maxCount.SetActive(false);
        allCount.SetActive(false);

    }

    public void TooltipClear()
    {
        //�ƹ��͵� ���� �������̸� ��Ȱ��ȭ
        image.SetActive(false);
        itemName.SetActive(false);
        toolTip.SetActive(false);
        rarity.SetActive(false);
        gold.SetActive(false);
        sellGold.SetActive(false);
        count.SetActive(false);
        slash.SetActive(false);
        maxCount.SetActive(false);
        allCount.SetActive(false);
    }

    //���� �Ҹ������̶� ������Ʈ�� �̸� ����������
    public void ToolTipSetItem(BaseItem value)
    {
        //�ƹ��͵� ���� �������̸� ��Ȱ��ȭ
        if (value.name == "")
        {
            TooltipClear();
            return;
        }
        //�������� ������ Ȱ��ȭ
        else
        {
            image.SetActive(true);
            itemName.SetActive(true);
            toolTip.SetActive(true);
            rarity.SetActive(true);
            gold.SetActive(true);
            sellGold.SetActive(true);
            count.SetActive(true);
            slash.SetActive(true);
            maxCount.SetActive(true);
            allCount.SetActive(true);

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
    }

}
