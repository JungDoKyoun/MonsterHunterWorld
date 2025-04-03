using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    [SerializeField] Text firstButtonName;

    void Start()
    {
        UIManager.Instance.AddUI(UIType.OptionUI,gameObject);

        Debug.Log("optionUI 연결 완료");

        if (SceneManager.GetActiveScene().name == "ALLTestScene")
        {
            Debug.Log("인 게임 씬입니다.");
            firstButtonName.text = "퀘스트 포기";
        }
        else
        {
            Debug.Log("인 게임 씬이 아닙니다.");
            firstButtonName.text = "저장";
        }

    }
    public async void QuitGame()
    {
        await InvenToryCtrl.Instance.SaveInventoryToFirebase();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public async void SaveButton()
    {
        await InvenToryCtrl.Instance.SaveInventoryToFirebase();
    }

    public void OptionButton()
    {
        UIManager.Instance.StackUIOpen(UIType.SoundOptionUI);
    }
}
