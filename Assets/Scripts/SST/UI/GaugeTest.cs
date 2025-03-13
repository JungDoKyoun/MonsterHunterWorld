using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeTest : MonoBehaviour
{
    public Slider hpSlider;
    public Slider staminaSlider;

    float maxHp = 100;
    float currentHp;

    void Start()
    {
        currentHp = maxHp;
    }

    void Update()
    {
        currentHp -= 0.01f * Time.deltaTime;
        hpSlider.value = currentHp / maxHp;
    }
}
