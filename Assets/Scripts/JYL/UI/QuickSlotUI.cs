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

    [SerializeField] float slideDuration = 0.2f; // �ִϸ��̼� �ӵ�
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
            //������ �ε� �Ϸ� �� ������ �ʱ�ȭ
            InvenToryCtrl.Instance.LoadQuickSlotItemsFromInventory();

            items = InvenToryCtrl.Instance.quickSlotItem;

            if (items.Count == 0)
            {
                Debug.LogWarning("��� ������ �������� �����ϴ�.");
                return;
            }

            ShowCurrentItem();
        });

        InvenToryCtrl.Instance.LoadQuickSlotItemsFromInventory();


        items = InvenToryCtrl.Instance.quickSlotItem;

        if (items.Count == 0)
        {
            Debug.LogWarning("��� ������ �������� �����ϴ�.");
            return;
        }

        ShowCurrentItem();
    }


    void Update()
    {
        // ���� ��Ʈ���� ������ �ִ� ���ȸ� Ȯ�� ���� ����
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //Ȯ�彽�� Ŵ
            if (!ExpansionSlot.activeSelf)
                ExpansionSlot.SetActive(true);

            // �� ���۵� �� �ȿ����� ó��
            // �ִϸ��̼� ���� �ƴ� ����
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
            //Ȯ�彽�� ��
            if (ExpansionSlot.activeSelf)
                ExpansionSlot.SetActive(false);

            // ������ ��� Ȯ�彽�� �ȿ��������� ��밡��
            if (Input.GetKeyDown(KeyCode.E))
            {
                UseCurrentItem();
            }
        } 

        // UI ���� (�⺻������ Ű �Է��� ������ ����)
        if (Input.anyKeyDown)
        {
            ShowCurrentItem();
        }
    }


    //�����ۻ��
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
                    ClearUI(); // �������� ������� �� ó��
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

        // �߾� �ε��� ����
        currentIndex = Mathf.Clamp(currentIndex, 0, items.Count - 1);

        // �߽� ������ ���� �ؽ�Ʈ
        var centerItem = items[currentIndex];
        useItemCountText.text = centerItem.count.ToString();
        itemNameText.text = centerItem.name;

        //for (int i = -2; i < 3; i++)
        //{
        //    SetIcon(i + 2, items[currentIndex + i]);
        //}

        // ���� ������ ������Ʈ (���� 5ĭ)
        for (int i = 0; i < icons.Count; i++)
        {
            int offset = i - 2; // icons[2]�� �߾�
            int targetIndex = GetWrappedIndex(currentIndex + offset);

            //indexoutofrange ���� list�� null�϶� ȣ���ϸ� ������..
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
        yield return AnimateSlide(-1); // �������� �����̵�

        currentIndex = nextIndex;
        ShowCurrentItem();

        isSliding = false;
    }

    IEnumerator SlideToPrevious()
    {
        isSliding = true;

        int prevIndex = (currentIndex + 1 + items.Count) % items.Count;
        yield return AnimateSlide(1); // ���������� �����̵�

        currentIndex = prevIndex;
        ShowCurrentItem();

        isSliding = false;
    }

    IEnumerator AnimateSlide(int direction)
    {
        float elapsed = 0f;
        float endX = direction * -100f; // 100�ȼ���ŭ �̵�

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

        // �����̵� �� ��ġ ���� �� ������ ���� ����
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
