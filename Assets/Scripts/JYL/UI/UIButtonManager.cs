using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIButtonManager : MonoBehaviour
{
    [System.Serializable]
    public class UIButtonData
    {
        public Button button;
        public UIType type;
    }

    [SerializeField] private List<UIButtonData> buttonMappings = new();

    private void Start()
    {
        foreach (var data in buttonMappings)
        {
            if (data.button == null || data.type == UIType.None)
            {
                Debug.LogWarning("버튼 또는 UIType이 설정되지 않았습니다.");
                continue;
            }

            data.button.onClick.AddListener(() =>
            {
                if(data.type < UIType.ItemChangeUI)
                {
                    UIManager.Instance.StackUIOpen(data.type);
                }
                else
                {
                    UIManager.Instance.EndOpenUI(data.type);
                }
            });
        }
    }
}
