using System;
using UnityEngine;

[Serializable]
public struct PlayerStat
{
    [Header("�ִ� ü��"), SerializeField]
    private uint _fullLife;

    public uint fullLife
    {
        get
        {
            return _fullLife;
        }
        set
        {
            _fullLife = value;
            if(_fullLife < _currentLife)
            {
                _currentLife = _fullLife;
            }
        }
    }

    [Header("���� ü��"), SerializeField]
    private uint _currentLife;

    public uint currentLife
    {
        get
        {
            return _currentLife;
        }
        set
        {
            _currentLife = value;
            if(_fullLife < _currentLife)
            {
                _currentLife = _fullLife;
            }
        }
    }

    [Header("�ִ� ���¹̳�"), SerializeField]
    private float _fullStamina;

    public float fullStamina
    { 
        get
        {
            return _fullStamina;
        }
        set
        {
            _fullStamina = Mathf.Clamp(value, 0, float.MaxValue);
            if(_fullStamina < _currentStamina)
            {
                _currentStamina = _fullStamina;
            }
        }
    }

    [Header("���� ���¹̳�"), SerializeField]
    private float _currentStamina;

    public float currentStamina
    {
        get
        {
            return _currentStamina;
        }
        set
        {
            _currentStamina = Mathf.Clamp(value, 0, float.MaxValue);
            if (_fullStamina < _currentStamina)
            {
                _currentStamina = _fullStamina;
            }
        }
    }
}