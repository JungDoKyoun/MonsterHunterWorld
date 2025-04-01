using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : BaseInventory
{
    [SerializeField] Text useItemCountText;
    [SerializeField] Image itemImg;
    [SerializeField] Text itemNameText;
    [SerializeField] List<Image> icons;


    int minIndex = 0;
    int maxIndex = 0;
    int currentIndex = 0;

    bool isSelecting = false;
    private void Awake()
    {
        invenType = InvenType.QuickSlot;

    }

    void Start()
    {
        InvenToryCtrl.Instance.LoadQuickSlotItemsFromInventory();

        
        items = InvenToryCtrl.Instance.quickSlotItem;

        if (items.Count == 0)
        {
            Debug.LogWarning("사용 가능한 아이템이 없습니다.");
            return;
        }

        maxIndex = items.Count - 1;
        ShowCurrentItem();
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            currentIndex = (currentIndex + 1) % items.Count;
            ShowCurrentItem();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            currentIndex = (currentIndex - 1 + items.Count) % items.Count;
            ShowCurrentItem();
        }
    }
    public void UseCurrentItem()
    {
        var item = items[currentIndex];
        if (item.count > 0)
        {
            item.count--;
            useItemCountText.text = item.count.ToString();

            if (item.count == 0)
            {
                InvenToryCtrl.Instance.quickSlotItem.RemoveAt(currentIndex);
                currentIndex = Mathf.Clamp(currentIndex, 0, InvenToryCtrl.Instance.quickSlotItem.Count - 1);
            }

            ShowCurrentItem();
            InvenToryCtrl.Instance.OnInventoryChanged?.Invoke();
        }
    }


    void ShowCurrentItem()
    {
        var item = items[currentIndex];
        useItemCountText.text = item.count.ToString();
        itemNameText.text = item.name;
        itemImg.sprite = item.image;
        itemImg.color = item.color;

        // 예: 가운데 아이콘(선택 중) 강조
        icons[3].sprite = item.image;
        icons[3].color = item.color;
    }


    void NextItem(int index)
    {
        if (index < minIndex)
        {
            var minindex = maxIndex - 1;
            icons[0].sprite = items[minindex].image;
            icons[0].color = items[minindex].color;
            icons[1].sprite = items[minindex + 1].image;
            icons[1].color = items[minindex + 1].color;
        }

        currentIndex = index;

        icons[3].sprite = items[currentIndex].image;
        icons[3].color = items[currentIndex].color;

    }

    void PrevItem(int index)
    {
        if (index >= maxIndex)
        {
            return;
        }

        currentIndex = index;

    }



}
