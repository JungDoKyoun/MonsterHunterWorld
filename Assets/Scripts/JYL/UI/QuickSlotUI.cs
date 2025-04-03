using Photon.Pun;
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

    Transform pl; // ���� �÷��̾��� Transform ���� ����

    private void Awake()
    {
        invenType = InvenType.QuickSlot;
    }

    void Start()
    {
        // Firebase���� �κ��丮 ������ �ε�
        //InvenToryCtrl.Instance.LoadInventoryFromFirebase(() =>
        //{
        // ������ �ε� �� ������ �ʱ�ȭ
        InvenToryCtrl.Instance.LoadQuickSlotItemsFromInventory();

        items = InvenToryCtrl.Instance.quickSlotItem;

        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("��� ������ �������� �����ϴ�.");
            return;
        }

        ShowCurrentItem();
        //});

        // ���� �÷��̾ ������ ������ ��ٸ�
        StartCoroutine(WaitForLocalPlayer());
    }

    void Update()
    {
        // ���� ��Ʈ���� ������ �ִ� ���� Ȯ�� ���� ����
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // Ȯ�� ���� Ȱ��ȭ
            if (!ExpansionSlot.activeSelf)
                ExpansionSlot.SetActive(true);

            // ���콺 �ٷ� ������ ����
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
            // Ȯ�� ���� ��Ȱ��ȭ
            if (ExpansionSlot.activeSelf)
                ExpansionSlot.SetActive(false);

            // Ȯ�� ������ ���� ���� ���� ������ ��� ����
            if (Input.GetKeyDown(KeyCode.E))
            {
                UseCurrentItem();
            }
        }

        // Ű �Է��� �߻��� �� UI ����
        if (Input.anyKeyDown)
        {
            ShowCurrentItem();
        }
    }

    // ���� �÷��̾ ������ ������ ����ϴ� �ڷ�ƾ
    private IEnumerator WaitForLocalPlayer()
    {
        while (pl == null)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                PhotonView pv = player.GetComponent<PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    pl = pv.GetComponent<Transform>();
                    //Debug.Log("Local player found: " + pl.name);
                    yield break; // ���� �÷��̾ ã���� �ڷ�ƾ ����
                }
            }

            yield return null; // ���� �����ӱ��� ���
        }
    }

    private bool isTrapPlaced = false; // ���� ��ȯ �ߺ� ���� �÷���
    [SerializeField] private float trapCooldown = 0.5f; // 0.5�� ��Ÿ��

    // ������ ���
    public void UseCurrentItem()
    {
        if (pl == null)
        {
            Debug.LogError("�÷��̾� Transform(pl)�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        if (isTrapPlaced || items.Count == 0) return;

        BaseItem item = items[currentIndex];
        if (item.count <= 0) return;

        // �ߺ� ���� �� ī��Ʈ ����
        isTrapPlaced = true;

        //Debug.Log($"Before Use: {item.name} - Count: {item.count}");
        item.count--;
        //Debug.Log($"After Use: {item.name} - Count: {item.count}");
        useItemCountText.text = item.count.ToString();

        if (item.id == ItemName.PitfallTrap)
        {
            PhotonNetwork.Instantiate("trap", pl.position, Quaternion.identity);

            Debug.Log("Ʈ���� ��ȯ�Ǿ����ϴ�.");
        }

        // ������ 0�� �Ǹ� ����


        if (item.count == 0)
        {
            //Debug.Log($"[REMOVE] {item.name} count is now 0");


            items.RemoveAt(currentIndex);

            if (items.Count == 0)
            {
                ClearUI(); // �������� ����� �� UI �ʱ�ȭ
                isTrapPlaced = false;
                return;
            }

            currentIndex = Mathf.Clamp(currentIndex, 0, items.Count - 1);
        }

        InvenToryCtrl.Instance.OnInventoryChanged?.Invoke();
        ShowCurrentItem();

        StartCoroutine(ResetTrapPlacedAfterDelay());
    }

    IEnumerator ResetTrapPlacedAfterDelay()
    {
        yield return new WaitForSeconds(trapCooldown);
        isTrapPlaced = false;
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

        // ���� ������ �ε��� ����
        currentIndex = Mathf.Clamp(currentIndex, 0, items.Count - 1);

        // �߾� ������ ���� ����
        var centerItem = items[currentIndex];
        useItemCountText.text = centerItem.count.ToString();
        itemNameText.text = centerItem.name;

        for (int i = 0; i < icons.Count; i++)
        {
            int offset = i - 2; // icons[2]�� �߾�
            int targetIndex = GetWrappedIndex(currentIndex + offset);

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
        if (items == null || items.Count == 0)
            yield break;

        isSliding = true;

        int nextIndex = (currentIndex - 1 + items.Count) % items.Count;
        yield return AnimateSlide(-1); // �������� �����̵�

        currentIndex = nextIndex;
        ShowCurrentItem();

        isSliding = false;
    }

    IEnumerator SlideToPrevious()
    {
        if (items == null || items.Count == 0)
            yield break;

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