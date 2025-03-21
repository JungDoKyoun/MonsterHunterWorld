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
    
    PlayerController player;

    public Slider slider;

    float current;
    float max;

    public void SetPlayer(PlayerController ctrl)
    {
        player = ctrl;
    }

    void Start()
    {
        

        if (type == FillBarType.HP)
        {
            slider.maxValue = max = (float)player.fullLife;
            slider.value = current = (float)player.currentLife;

        }

        else if (type == FillBarType.SP)
        {
            slider.maxValue = max = player.fullStamina;
            slider.value = current = player.currentStamina;
        }

    }

    public void UpdateHP(float newHP)
    {
        current = Mathf.Clamp(newHP, 0, max);
        slider.value = current;
    }
}
