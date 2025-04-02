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


    public void SetName(string nickname)
    {
        _nameText.SetText(nickname);
    }

    public void SetLife(int current, int full)
    {
        if (full == 0)
        {
            _lifeSlider.value = 1;
        }
        else
        {
            _lifeSlider.value = (float)current / (float)full;
        }
    }

    public void SetStamina(float current, float full)
    {
        if(full <= 0)
        {
            _staminaSlider.value = 1;
        }
        else
        {
            _staminaSlider.value = current / full;
        }
    }
}