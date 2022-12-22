using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _maxHPEasy = 200;
    [SerializeField] private AudioSource _hurtSource;
    [SerializeField] private AudioSource _itemSource;
    [SerializeField] private AudioClip _atkClip;
    [SerializeField] private AudioClip _shieldClip;
    [SerializeField] private AudioClip _healClip;
    [SerializeField] private GameObject _shieldRenderer;
    [SerializeField] private SpriteRenderer _playerRenderer;
    [SerializeField] private Color _hurtColor;
    private int _currentHP = 0;
    private Vector3 _spawnPos = Vector3.zero;
    private bool _canTakeDamage = false;
    public bool AtkBoostEnabled { get; private set; } = false;

    public Action<float> OnHealthChanged;
    public Action OnPlayerDeath;
    private Coroutine _hurtCoroutine = null;

    private void Start()
    {
        _maxHP = Inventory.Instance.IsOnEasy ? _maxHPEasy : _maxHP;
        _currentHP = _maxHP;
        _spawnPos = transform.position;
    }

    public void OnStartGame()
    {
        _canTakeDamage = true;
    }

    public void Damage(int damageAmount)
    {
        if (_canTakeDamage && !GameLogicManager.Instance.IsInDialogue)
        {
            if (_hurtCoroutine == null)
            {
                _hurtCoroutine = StartCoroutine(DamageVFX());
            }
            _currentHP -= damageAmount;
            CheckHP();
        }
    }

    private IEnumerator DamageVFX()
    {
        _playerRenderer.color = _hurtColor;
        yield return new WaitForSeconds(0.07f);
        _hurtSource.Play();
        _playerRenderer.color = Color.white;
        _hurtCoroutine = null;
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
        if (context.performed && !GameLogicManager.Instance.IsInDialogue)
        {
            if (Inventory.Instance.CanUse(ItemType.ATK_BOOST) && AtkBoostEnabled == false)
            {
                _itemSource.clip = _atkClip;
                _itemSource.Play();
                StartCoroutine(StartAtkBoost(Inventory.Instance.AtkBoostTime));
            }
        }
    }

    public void UseShield(InputAction.CallbackContext context)
    {
        if (context.performed && !GameLogicManager.Instance.IsInDialogue)
        {
            if (Inventory.Instance.CanUse(ItemType.SHIELD) && _canTakeDamage == true)
            {
                _itemSource.clip = _shieldClip;
                _itemSource.Play();
                StartCoroutine(SpawnShield(Inventory.Instance.ShieldTime));
            }
        }
    }

    public void UseHeal(InputAction.CallbackContext context)
    {
        if (context.performed && !GameLogicManager.Instance.IsInDialogue)
        {
            if (_currentHP < _maxHP && Inventory.Instance.CanUse(ItemType.HEAL))
            {
                _itemSource.clip = _healClip;
                _itemSource.Play();
                _currentHP += Inventory.Instance.HealAmount;
                CheckHP();
            }
        }
    }

    private IEnumerator SpawnShield(float time)
    {
        _shieldRenderer.SetActive(true);
        _canTakeDamage = false;
        yield return new WaitForSeconds(time);
        _canTakeDamage = true;
        _shieldRenderer.SetActive(false);
    }

    private IEnumerator StartAtkBoost(float time)
    {
        AtkBoostEnabled = true;
        yield return new WaitForSeconds(time);
        AtkBoostEnabled = false;
    }

    public IEnumerator DeathCoroutine()
    {
        yield return null;
        //set animation, timer, etc
    }

    public void BackToSpawn()
    {
        StartCoroutine(BackToSpawnPos());
    }

    private IEnumerator BackToSpawnPos()
    {
        Vector3 startpos = transform.position;
        Vector3 currentPos = Vector3.zero;
        float timer = 0;
        float time = 0.5f;

        while (timer < time)
        {
            currentPos = Vector3.Lerp(startpos, _spawnPos, timer / time);
            transform.position = currentPos;
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = _spawnPos;
    }

}
