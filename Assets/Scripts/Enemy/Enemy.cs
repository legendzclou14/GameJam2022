using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    [SerializeField] private GameObject _zigZagAttackPrefab;
    [SerializeField] private GameObject _pillarAttackPrefab;
    [SerializeField] private GameObject _bombAttackPrefab;
    [SerializeField] private GameObject _rayAttackPrefab;
    private int _currentHP = 0;

    public Action<float> OnHealthChanged;

    private void Awake()
    {
        _currentHP = _maxHP;

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

        if (_currentHP < 0)
        {
            GameLogicManager.Instance.GameOver(true);
        }
    }

    public void Damage(int damageAmount)
    {
        _currentHP -= damageAmount;
        Debug.Log("Damaged Enemy!");
        CheckHP();
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
