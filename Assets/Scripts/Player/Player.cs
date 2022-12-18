using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    [SerializeField] private AudioSource _hurtSource;
    private int _currentHP = 0;
    private bool _canTakeDamage = false;
    public bool AtkBoostEnabled { get; private set; } = false;

    public Action<float> OnHealthChanged;
    public Action OnPlayerDeath;

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    public void OnStartGame()
    {
        _canTakeDamage = true;
    }

    public void Damage(int damageAmount)
    {
        if (_canTakeDamage && !GameLogicManager.Instance.IsInDialogue)
        {
            _hurtSource.Play();
            _currentHP -= damageAmount;
            CheckHP();
        }
    }

    private void CheckHP()
    {
        _currentHP = _currentHP > _maxHP ? _maxHP : _currentHP;
        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke((float)_currentHP / _maxHP);
        }

        if (_currentHP < 0)
        {
            OnPlayerDeath.Invoke();
        }
    }

    public void UseAtkBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Inventory.Instance.CanUse(ItemType.ATK_BOOST) && AtkBoostEnabled == false)
            {
                Debug.Log("atkboost");
                StartCoroutine(StartAtkBoost(Inventory.Instance.AtkBoostTime));
            }
        }
    }

    public void UseShield(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Inventory.Instance.CanUse(ItemType.SHIELD) && _canTakeDamage == true)
            {
                Debug.Log("shield");
                StartCoroutine(SpawnShield(Inventory.Instance.ShieldTime));
            }
        }
    }

    public void UseHeal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_currentHP < _maxHP && Inventory.Instance.CanUse(ItemType.HEAL))
            {
                Debug.Log("heal");
                _currentHP += Inventory.Instance.HealAmount;
                CheckHP();
            }
        }
    }

    private IEnumerator SpawnShield(float time)
    {
        //spawn shield or smth idk
        _canTakeDamage = false;
        yield return new WaitForSeconds(time);
        _canTakeDamage = true;
        //unspawn sheild
    }

    private IEnumerator StartAtkBoost(float time)
    {
        //vfx?
        AtkBoostEnabled = true;
        yield return new WaitForSeconds(time);
        AtkBoostEnabled = false;
    }

    public IEnumerator DeathCoroutine()
    {
        yield return null;
        //set animation, timer, etc
    }
}
