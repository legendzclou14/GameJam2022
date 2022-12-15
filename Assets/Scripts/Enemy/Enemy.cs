using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHP;
    private int _currentHP = 0;

    private void Awake()
    {
        _currentHP = _maxHP;
    }

    private void CheckHP()
    {
        _currentHP = _currentHP > _maxHP ? _maxHP : _currentHP;

        if (_currentHP < 0)
        {
            //GameOver
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DamageEnemy"))
        {
            DamagingObject damagingObject = other.gameObject.GetComponent<DamagingObject>();
            _currentHP -= damagingObject.DamageAmount;
            Debug.Log("Damaged enemy!");
            Destroy(other.gameObject);
            CheckHP();
        }
    }
}
