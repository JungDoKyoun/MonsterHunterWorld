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
        if(type == FillBarType.HP)
        {
            current = Mathf.Clamp(newHP, 0, max);
            slider.value = current;
        }
        else
        {
            Debug.Log("HP바에 SP값을 넣었습니다.");
        }
        

    }

    public void UpdateSP(float newSP)
    {
        if(type == FillBarType.SP)
        {
            current = Mathf.Clamp(newSP, 0, max);
            slider.value = current;
        }
        else
        {
            Debug.Log("SP바에 HP값을 넣었습니다.");
        }
    }
}
