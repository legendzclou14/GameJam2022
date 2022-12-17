using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    private int _currentHP = 0;

    public Action<float> OnHealthChanged;

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    public void Damage(int damageAmount)
    {
        _currentHP -= damageAmount;
        Debug.Log("Damaged player!");
        CheckHP();
    }

    private void CheckHP()
    {
        _currentHP = _currentHP > _maxHP ? _maxHP : _currentHP;

        OnHealthChanged.Invoke((float)_currentHP / _maxHP);
        if (_currentHP < 0)
        {
            GameLogicManager.Instance.GameOver(false);
        }
    }
}
