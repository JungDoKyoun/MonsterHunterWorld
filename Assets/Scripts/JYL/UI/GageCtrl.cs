using UnityEngine;
using UnityEngine.UI;

public class GageCtrl : MonoBehaviour
{
    [SerializeField]
    private Text _nameText;

    [SerializeField]
    private Slider _lifeSlider;

    [SerializeField]
    private Slider _staminaSlider;

    public void Set(string nickname)
    {
        _nameText.SetText(nickname);
    }

    public void SetLife(int current, int full)
    {
        if (full == 0)
        {

        }
        else
        {
            _lifeSlider.value = current / full;
        }

    }
}
