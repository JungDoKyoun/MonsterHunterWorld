using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class ChangeBox : MonoBehaviour
{
    public enum Direction { ToBox, ToInventory }

    // 화살표 이미지들 (좌 → 우 순서)
    [SerializeField] private GameObject[] arrowObjects;
    private Image[] arrows;
    private RectTransform[] arrowTransforms;

    [SerializeField] private float interval = 0.1f;

    [SerializeField] private Image[] leftIcon = new Image[2];
    [SerializeField] private Image[] rightIcon = new Image[2];
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color highlightColor = Color.yellow;

    private Coroutine animCoroutine;

    private void Awake()
    {
        arrows = new Image[arrowObjects.Length];

        arrowTransforms = new RectTransform[arrowObjects.Length];

        for (int i = 0; i < arrowObjects.Length; i++)
        {
            arrows[i] = arrowObjects[i].GetComponent<Image>();
            arrowTransforms[i] = arrowObjects[i].GetComponent<RectTransform>();
        }
    }
    // 화살표 애니메이션 시작
    public void StartArrowAnimation(Direction dir)
    {
        StopArrowAnimation();
        animCoroutine = StartCoroutine(AnimateArrow(dir));
    }
    // 화살표 애니메이션 멈춤
    public void StopArrowAnimation()
    {
        if (animCoroutine != null)
        {
            StopCoroutine(animCoroutine);
            animCoroutine = null;
        }
        ResetArrows();
        ResetIcons();
    }

    // 화살표 애니메이션
    private IEnumerator AnimateArrow(Direction dir)
    {
        // 방향 설정
        float rotationZ = dir == Direction.ToBox ? 90f : -90f; //box로 가는 방향이면 90도, inventory로 가는 방향이면 -90도
        foreach (var rect in arrowTransforms)//화살표 이미지들을 회전
        {
            rect.rotation = Quaternion.Euler(0, 0, rotationZ);
        }

        // 아이콘 강조
        if (dir == Direction.ToBox)//box로 가는 방향이면 왼쪽 아이콘 강조
        {
            for (int i = 0; i < leftIcon.Length; i++)
            {
                leftIcon[i].color = highlightColor;
                rightIcon[i].color = normalColor;
            }

        }
        else
        {
            for (int i = 0; i < leftIcon.Length; i++)
            {
                leftIcon[i].color = normalColor;
                rightIcon[i].color = highlightColor;
            }
        }

        int index = 0;
        while (true)
        {
            ResetArrows();
            if (dir == Direction.ToInventory)
            {
                if (index >= 0 && index < arrows.Length)
                {
                    arrows[arrows.Length - 1 - index].color = highlightColor;
                }

            }
            else
            {
                if (index >= 0 && index < arrows.Length)
                {
                    arrows[index].color = highlightColor;
                }

            }

            index++;
            if (index >= arrows.Length) index = 0;
            yield return new WaitForSeconds(interval);
        }
    }

    private void ResetArrows()
    {
        foreach (var arrow in arrows)
        {
            arrow.color = normalColor;
        }
    }

    private void ResetIcons()
    {
        for (int i = 0; i < leftIcon.Length; i++)
        {
            leftIcon[i].color = normalColor;
            rightIcon[i].color = normalColor;
        }
    }

    // 마우스 올렸을 때 호출
    public void OnInventoryHover()
    {
        StartArrowAnimation(Direction.ToBox);
    }

    public void OnBoxHover()
    {
        StartArrowAnimation(Direction.ToInventory);
    }

    public void OnExit()
    {
        StopArrowAnimation();
    }
}
