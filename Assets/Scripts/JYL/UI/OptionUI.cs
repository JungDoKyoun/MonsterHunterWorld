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

        Debug.Log("optionUI ���� �Ϸ�");

        if (SceneManager.GetActiveScene().name == "ALLTestScene")
        {
            Debug.Log("�� ���� ���Դϴ�.");
            firstButtonName.text = "����Ʈ ����";
        }
        else
        {
            Debug.Log("�� ���� ���� �ƴմϴ�.");
            firstButtonName.text = "����";
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
