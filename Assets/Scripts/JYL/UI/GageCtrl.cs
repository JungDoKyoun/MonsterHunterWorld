using System;
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

    [Serializable]
    private struct Set
    {
        [SerializeField]
        private GameObject root;
        [SerializeField]
        private Text text;
        [SerializeField]
        private Slider slider;

        public void SetActive(bool value)
        {
            root.Set(value);
        }

        public void SetName(string nickname)
        {
            text.SetText(nickname);
        }

        public void SetValue(int current, int full)
        {
            if (full == 0)
            {
                slider.value = 1;
            }
            else
            {
                slider.value = (float)current / (float)full;
            }
        }
    }

    [SerializeField]
    private Set[] sets = new Set[3];

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

    public void SetName(string nickname, int index)
    {
        if(index < sets.Length)
        {
            sets[index].SetActive(true);
            sets[index].SetName(nickname);
        }
    }

    public void SetLife(int current, int full, int index)
    {
        if (index < sets.Length)
        {
            sets[index].SetValue(current, full);
        }
    }

    public void HidePlayer(int index)
    {
        if (index < sets.Length)
        {
            sets[index].SetActive(false);
        }
    }
}