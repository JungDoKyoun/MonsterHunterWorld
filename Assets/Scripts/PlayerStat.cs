using System;
using UnityEngine;

[Serializable]
public struct PlayerStat
{
    [Header("최대 체력"), SerializeField]
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

    [Header("현재 체력"), SerializeField]
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

    [Header("최대 스태미너"), SerializeField]
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

    [Header("현재 스태미너"), SerializeField]
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