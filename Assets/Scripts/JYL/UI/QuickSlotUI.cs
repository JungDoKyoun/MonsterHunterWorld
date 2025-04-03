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

    [SerializeField] float slideDuration = 0.2f; // 애니메이션 속도
    private bool isSliding = false;

    int currentIndex = 0;

    Transform pl; // 로컬 플레이어의 Transform 저장 변수

    private void Awake()
    {
        invenType = InvenType.QuickSlot;
    }

    void Start()
    {
        // Firebase에서 인벤토리 데이터 로드
        //InvenToryCtrl.Instance.LoadInventoryFromFirebase(() =>
        //{
        // 데이터 로드 후 퀵슬롯 초기화
        InvenToryCtrl.Instance.LoadQuickSlotItemsFromInventory();

        items = InvenToryCtrl.Instance.quickSlotItem;

        if (items == null || items.Count == 0)
        {
            Debug.LogWarning("사용 가능한 아이템이 없습니다.");
            return;
        }

        ShowCurrentItem();
        //});

        // 로컬 플레이어가 생성될 때까지 기다림
        StartCoroutine(WaitForLocalPlayer());
    }

    void Update()
    {
        // 왼쪽 컨트롤을 누르고 있는 동안 확장 슬롯 열기
        if (Input.GetKey(KeyCode.LeftControl))
        {
            // 확장 슬롯 활성화
            if (!ExpansionSlot.activeSelf)
                ExpansionSlot.SetActive(true);

            // 마우스 휠로 아이템 변경
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
            // 확장 슬롯 비활성화
            if (ExpansionSlot.activeSelf)
                ExpansionSlot.SetActive(false);

            // 확장 슬롯이 닫혀 있을 때만 아이템 사용 가능
            if (Input.GetKeyDown(KeyCode.E))
            {
                UseCurrentItem();
            }
        }

        // 키 입력이 발생할 때 UI 갱신
        if (Input.anyKeyDown)
        {
            ShowCurrentItem();
        }
    }

    // 로컬 플레이어가 생성될 때까지 대기하는 코루틴
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
                    yield break; // 로컬 플레이어를 찾으면 코루틴 종료
                }
            }

            yield return null; // 다음 프레임까지 대기
        }
    }

    private bool isTrapPlaced = false; // 함정 소환 중복 방지 플래그
    [SerializeField] private float trapCooldown = 0.5f; // 0.5초 쿨타임

    // 아이템 사용
    public void UseCurrentItem()
    {
        if (pl == null)
        {
            Debug.LogError("플레이어 Transform(pl)이 초기화되지 않았습니다.");
            return;
        }

        if (isTrapPlaced || items.Count == 0) return;

        BaseItem item = items[currentIndex];
        if (item.count <= 0) return;

        // 중복 방지 및 카운트 감소
        isTrapPlaced = true;

        //Debug.Log($"Before Use: {item.name} - Count: {item.count}");
        item.count--;
        //Debug.Log($"After Use: {item.name} - Count: {item.count}");
        useItemCountText.text = item.count.ToString();

        if (item.id == ItemName.PitfallTrap)
        {
            PhotonNetwork.Instantiate("trap", pl.position, Quaternion.identity);

            Debug.Log("트랩이 소환되었습니다.");
        }

        // 아이템 0개 되면 제거


        if (item.count == 0)
        {
            //Debug.Log($"[REMOVE] {item.name} count is now 0");


            items.RemoveAt(currentIndex);

            if (items.Count == 0)
            {
                ClearUI(); // 퀵슬롯이 비었을 때 UI 초기화
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

        // 현재 아이템 인덱스 조정
        currentIndex = Mathf.Clamp(currentIndex, 0, items.Count - 1);

        // 중앙 아이템 정보 갱신
        var centerItem = items[currentIndex];
        useItemCountText.text = centerItem.count.ToString();
        itemNameText.text = centerItem.name;

        for (int i = 0; i < icons.Count; i++)
        {
            int offset = i - 2; // icons[2]가 중앙
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
        yield return AnimateSlide(-1); // 왼쪽으로 슬라이드

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