using UnityEngine;

public interface IClosableUI
{
    void CloseUI();
    bool IsOpen { get; }
}
