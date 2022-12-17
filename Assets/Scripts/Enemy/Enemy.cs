using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHPFirstPhase = 200;
    [SerializeField] private int _maxHPSecondPhase = 200;
    [SerializeField] private GameObject _zigZagAttackPrefab;
    [SerializeField] private GameObject _pillarAttackPrefab;
    [SerializeField] private GameObject _bombAttackPrefab;
    [SerializeField] private GameObject _rayAttackPrefab;
    
    private int _maxHP = 0;
    private int _currentHP = 0;
    private int _phase = 1;
    private bool _canTakeDamage = false;

    public Action<float> OnHealthChanged;

    private void Awake()
    {
        _maxHP = _maxHPFirstPhase;
        _currentHP = _maxHP;
    }

    public void OnStartGame()
    {
        _canTakeDamage = true;

        StartCoroutine(PhaseOneAttacks());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void CheckHP()
    {
        _currentHP = _currentHP > _maxHP ? _maxHP : _currentHP;
        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke((float)_currentHP / _maxHP);
        }

        if (_currentHP <= 0)
        {
            if (_phase == 1)
            {
                StartCoroutine(StartSecondPhase());
            }
            else
            {
                GameLogicManager.Instance.GameOver(true);
            }
        }
    }

    public void Damage(int damageAmount)
    {
        if (_canTakeDamage)
        {
            _currentHP -= damageAmount;
            Debug.Log($"Damaged Enemy for {damageAmount}!");
            CheckHP();
        }
    }

    public void StartZigZagAttack()
    {
        GameObject.Instantiate(_zigZagAttackPrefab, GameLogicManager.Instance.EnemyAttackParent);
    }
    
    public void StartPillarAttack()
    {
        GameObject.Instantiate(_pillarAttackPrefab, GameLogicManager.Instance.EnemyAttackParent);
    }

    public void StartBombAttack()
    {
        GameObject.Instantiate(_bombAttackPrefab, GameLogicManager.Instance.EnemyAttackParent);
    }

    public void StartRayAttack()
    {
        GameObject.Instantiate(_rayAttackPrefab, GameLogicManager.Instance.EnemyAttackParent);
    }

    private IEnumerator StartSecondPhase()
    {
        _canTakeDamage = false;
        _maxHP = _maxHPSecondPhase;
        _currentHP = _maxHP;
        yield return GameLogicManager.Instance.UI.FillEnemyBar();
        _phase = 2;
        _canTakeDamage = true;
    }

    private IEnumerator PhaseOneAttacks()
    {
        yield return new WaitForSeconds(5);
        StartZigZagAttack();
        yield return new WaitForSeconds(15);
        StartPillarAttack();
        yield return new WaitForSeconds(15);
        StartBombAttack();
        yield return new WaitForSeconds(15);
        StartRayAttack();
    }
}
