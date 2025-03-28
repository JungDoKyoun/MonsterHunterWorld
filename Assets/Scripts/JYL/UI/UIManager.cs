using System.Collections.Generic;
using UnityEngine;
public interface IClosableUI
{
    void CloseUI();
    bool IsOpen { get; }
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Stack<IClosableUI> openUIs = new();

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTopUI();
        }
    }

    public void RegisterUI(IClosableUI ui)
    {
        if (!openUIs.Contains(ui))
        {
            openUIs.Push(ui);
        }
    }

    public void UnregisterUI(IClosableUI ui)
    {
        if (openUIs.Contains(ui))
        {
            var temp = new Stack<IClosableUI>();
            while (openUIs.Count > 0)
            {
                var top = openUIs.Pop();
                if (top != ui) temp.Push(top);
            }
            while (temp.Count > 0) openUIs.Push(temp.Pop());
        }
    }

    public void CloseTopUI()
    {
        if (openUIs.Count > 0)
        {
            var topUI = openUIs.Pop();
            topUI.CloseUI();
        }
    }

    public void CloseAll()
    {
        while (openUIs.Count > 0)
        {
            openUIs.Pop().CloseUI();
        }
    }
}
