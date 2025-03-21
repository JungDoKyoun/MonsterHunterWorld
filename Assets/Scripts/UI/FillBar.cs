using UnityEngine;
using UnityEngine.UI;

public enum FillBarType
{
    HP,
    SP
}

public class FillBar : MonoBehaviour
{
    public FillBarType type;
    
    public Slider slider;

    float current;
    float max;

    public void SetPlayer(PlayerController ctrl)
    {

        if (type == FillBarType.HP)
        {
            slider.maxValue = max = (float)ctrl.fullLife;
            slider.value = current = (float)ctrl.currentLife;

        }

        else if (type == FillBarType.SP)
        {
            slider.maxValue = max = ctrl.fullStamina;
            slider.value = current = ctrl.currentStamina;
        }
    }


    public void UpdateHP(float newHP)
    {
        current = Mathf.Clamp(newHP, 0, max);
        slider.value = current;

        //Debug.Log(slider.maxValue);
        //Debug.Log(slider.value);
        //Debug.LogError("HP: " + current);
    }
}
