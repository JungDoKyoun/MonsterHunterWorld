using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotUI : BaseInventory
{
    [SerializeField] Text useItemCountText;
    [SerializeField] Text itemNameText;
    [SerializeField] List<Image> icons;

    [SerializeField] GameObject ExpansionSlot;

    [SerializeField] float slideDuration = 0.2f; // 애니메이션 속도
    private bool isSliding = false;

    int currentIndex = 0;

    private void Awake()
    {
        invenType = InvenType.QuickSlot;

    }

    void Start()
    {
        InvenToryCtrl.Instance.LoadInventoryFromFirebase(() =>
        {
            //데이터 로드 완료 후 퀵슬롯 초기화
            InvenToryCtrl.Instance.LoadQuickSlotItemsFromInventory();

            items = InvenToryCtrl.Instance.quickSlotItem;

            if (items.Count == 0)
            {
                Debug.LogWarning("사용 가능한 아이템이 없습니다.");
                return;
            }

            ShowCurrentItem();
        });

        InvenToryCtrl.Instance.LoadQuickSlotItemsFromInventory();


        items = InvenToryCtrl.Instance.quickSlotItem;

        if (items.Count == 0)
        {
            Debug.LogWarning("사용 가능한 아이템이 없습니다.");
            return;
        }

        ShowCurrentItem();
    }


    void Update()
    {
        // 왼쪽 컨트롤을 누르고 있는 동안만 확장 슬롯 열기
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //확장슬롯 킴
            if (!ExpansionSlot.activeSelf)
                ExpansionSlot.SetActive(true);

            // 휠 조작도 이 안에서만 처리
            // 애니메이션 중이 아닐 때만
            if (!isSliding) 
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
                {
                    StartCoroutine(SlideToNext());
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    StartCoroutine(SlideToPrevious());
                }
            }
        }
        else
        {
            //확장슬롯 끔
            if (ExpansionSlot.activeSelf)
                ExpansionSlot.SetActive(false);

            // 아이템 사용 확장슬롯 안열었을때만 사용가능
            if (Input.GetKeyDown(KeyCode.E))
            {
                UseCurrentItem();
            }
        } 

        // UI 갱신 (기본적으로 키 입력이 있으면 갱신)
        if (Input.anyKeyDown)
        {
            ShowCurrentItem();
        }
    }


    //아이템사용
    public void UseCurrentItem()
    {
        var item = items[currentIndex];
        if (item.count > 0)
        {
            item.count--;
            useItemCountText.text = item.count.ToString();

            if (item.count == 0)
            {
                items.RemoveAt(currentIndex);
                if (items.Count == 0)
                {
                    ClearUI(); // 퀵슬롯이 비어있을 때 처리
                    return;
                }

                currentIndex = Mathf.Clamp(currentIndex, 0, items.Count - 1);
            }

            ShowCurrentItem();

            InvenToryCtrl.Instance.OnInventoryChanged?.Invoke();
        }
    }


    void ClearUI()
    {
        useItemCountText.text = "0";
        itemNameText.text = "-";

        foreach (var icon in icons)
        {
            icon.sprite = null;
            icon.color = Color.clear;
        }
    }

    void ShowCurrentItem()
    {
        if (items == null || items.Count == 0)
        {
            ClearUI();
            return;
        }

        // 중앙 인덱스 고정
        currentIndex = Mathf.Clamp(currentIndex, 0, items.Count - 1);

        // 중심 아이템 정보 텍스트
        var centerItem = items[currentIndex];
        useItemCountText.text = centerItem.count.ToString();
        itemNameText.text = centerItem.name;

        //for (int i = -2; i < 3; i++)
        //{
        //    SetIcon(i + 2, items[currentIndex + i]);
        //}

        // 슬롯 아이콘 업데이트 (고정 5칸)
        for (int i = 0; i < icons.Count; i++)
        {
            int offset = i - 2; // icons[2]가 중앙
            int targetIndex = GetWrappedIndex(currentIndex + offset);

            //indexoutofrange 대비용 list가 null일때 호출하면 에러뜸..
            BaseItem targetItem = items.Count > 0 ? items[targetIndex] : ItemDataBase.Instance.EmptyItem;
            SetIcon(i, targetItem);
        }
    }




    void SetIcon(int iconIndex, BaseItem item)
    {
        if (iconIndex < 0 || iconIndex >= icons.Count) return;

        if (item != null && item.id != ItemName.Empty)
        {
            icons[iconIndex].sprite = item.image;
            icons[iconIndex].color = item.color;
        }
        else
        {
            icons[iconIndex].sprite = null;
            icons[iconIndex].color = Color.clear;
        }
    }


    int GetWrappedIndex(int index)
    {
        if (items.Count == 0) return 0;
        return (index + items.Count) % items.Count;
    }


    IEnumerator SlideToNext()
    {
        isSliding = true;

  
        int nextIndex = (currentIndex - 1+ items.Count) % items.Count;
        yield return AnimateSlide(-1); // 왼쪽으로 슬라이드

        currentIndex = nextIndex;
        ShowCurrentItem();

        isSliding = false;
    }

    IEnumerator SlideToPrevious()
    {
        isSliding = true;

        int prevIndex = (currentIndex + 1 + items.Count) % items.Count;
        yield return AnimateSlide(1); // 오른쪽으로 슬라이드

        currentIndex = prevIndex;
        ShowCurrentItem();

        isSliding = false;
    }

    IEnumerator AnimateSlide(int direction)
    {
        float elapsed = 0f;
        float endX = direction * -100f; // 100픽셀만큼 이동

        Vector3[] startPositions = new Vector3[icons.Count];
        for (int i = 0; i < icons.Count; i++)
        {
            startPositions[i] = icons[i].rectTransform.localPosition;
        }

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / slideDuration;
            for (int i = 0; i < icons.Count; i++)
            {
                var pos = startPositions[i];
                icons[i].rectTransform.localPosition = 
                    Vector3.Lerp(pos, pos + Vector3.right * endX, t);
            }
            yield return null;
        }

        // 슬라이드 후 위치 원복 및 아이콘 새로 세팅
        ResetIconPositions();
        UpdateIcons();
    }

    void ResetIconPositions()
    {
        float baseX = 0f;
        float spacing = 100f;

        for (int i = 0; i < icons.Count; i++)
        {
            float offset = (i - icons.Count / 2) * spacing;
            icons[i].rectTransform.localPosition = new Vector3(baseX + offset, 0f, 0f);
        }
    }

    void UpdateIcons()
    {
        int count = icons.Count;
        int half = count / 2;

        for (int i = 0; i < count; i++)
        {
            int index = (currentIndex + i - half + items.Count) % items.Count;
            icons[i].sprite = items[index].image;
            icons[i].color = items[index].color;
        }
    }

}
