using System.Collections.Generic;
using UnityEngine;
public enum UIType
{
    None,
    AllVillageUI,
    SelectButtonUI,
    ItemButtonSelectUI,
    EquipButtonSelectUI,
    ItemChangeUI,
    ItemSellUI,
    EquipSelectUI,
    EquipInvenUI,
    BoxSelectUI,
    InGameUI

}

[System.Serializable]
public class UIData
{
    public UIType type;
    public GameObject uiObject;
}


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    [SerializeField] private List<UIData> uiList = new();

    private Dictionary<UIType, GameObject> uiMap = new();
    private Stack<UIType> openStack = new();

    public bool isBox = false;
    public Collider player;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;

        foreach (var ui in uiList)
        {
            if (!uiMap.ContainsKey(ui.type))
            {
                uiMap.Add(ui.type, ui.uiObject);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            player.gameObject.GetComponent<PlayerController>().enabled = true;
            CloseTopUI();
        }
        if (isBox && Input.GetKeyDown(KeyCode.F))
        {
            if (uiMap[UIType.AllVillageUI].activeSelf == false)
            {
                StackUIOpen(UIType.AllVillageUI);
                //플레이어 이동 제한
                player.gameObject.GetComponent<PlayerController>().enabled = false;
                player.gameObject.GetComponent<PlayerController>().Move(Vector2.zero);
                CloseUI(UIType.BoxSelectUI);
            }
            else
            {
                player.gameObject.GetComponent<PlayerController>().enabled = true;

                CloseAll();
                OpenUI(UIType.BoxSelectUI);
            }
        }
    }

    //일반 UI오픈
    public void OpenUI(UIType type)
    {
        if (!uiMap.ContainsKey(type)) return;

        var go = uiMap[type];
        if (!go.activeSelf)
        {
            go.SetActive(true);
        }
    }

    //스택에 쌓인 UI 다 지우고 UI 새로여는 메서드
    public void EndOpenUI(UIType key)
    {
        while (openStack.Count > 0)
        {
            var type = openStack.Pop();
            if (uiMap.ContainsKey(type))
            {
                uiMap[type].SetActive(false);
            }
        }

        GameObject go = uiMap[key];
        if (!go.activeSelf)
        {
            go.SetActive(true);
            openStack.Push(key);
        }
    }

    //스택UI 오픈
    public void StackUIOpen(UIType type)
    {

        if (!uiMap.ContainsKey(type)) return;

        GameObject go = uiMap[type];
        if (!go.activeSelf)
        {
            go.SetActive(true);
            openStack.Push(type);
        }
    }



    //일반 UI 닫기
    public void CloseUI(UIType type)
    {
        var go = uiMap[type];
        if (go.activeSelf)
        {
            go.SetActive(false);
        }
    }

    //제일 마지막 UI 제거
    public void CloseTopUI()
    {
        if (openStack.Count > 0)
        {
            var type = openStack.Pop();
            if (uiMap.ContainsKey(type))
            {
                uiMap[type].SetActive(false);
            }
        }
    }

    //UI 모두 제거
    public void CloseAll()
    {
        while (openStack.Count > 0)
        {
            var type = openStack.Pop();
            if (uiMap.ContainsKey(type))
            {
                uiMap[type].SetActive(false);
            }
        }
    }
}
