using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class FireBullets : MonoBehaviour
{
    [SerializeField] private int _shotsPerSecond = 1;
    [SerializeField] protected GameObject _bulletPrefab = null;
    [SerializeField] private AudioSource _audioSource;
    private Coroutine _firingCoroutine = null;
    private bool _firing = false;
    public bool Firing { get { return _firing; } }
    private Player _linkedPlayer = null;

    public Action IsFiring; 

    private void Awake()
    {
        _linkedPlayer = GetComponent<Player>();
        _firingCoroutine = StartCoroutine(FireCoroutine());
    }

    private void OnDestroy() 
    {
        StopCoroutine(_firingCoroutine);
    }

    public void FireInput(InputAction.CallbackContext context)
    {
        _firing = context.performed;
        IsFiring.Invoke();
    }

    private IEnumerator FireCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(1f / _shotsPerSecond);

        while (true)
        {
            if (_firing && !GameLogicManager.Instance.IsInDialogue)
            {
                InstantiateBullet();
                yield return delay;
            }
            else
            {
                yield return null;
            }
        }
    }

    private void InstantiateBullet()
    {
        GameObject bullet = GameObject.Instantiate(_bulletPrefab, transform.position, transform.rotation, GameLogicManager.Instance.BulletParent);
        _audioSource.Play();
        if (_linkedPlayer.AtkBoostEnabled)
        {
            bullet.GetComponent<DamagingObject>().DamageAmount *= Inventory.Instance.AtkBoostMultiplier;
        }
    }
}
