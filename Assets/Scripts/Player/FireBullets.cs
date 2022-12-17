using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class FireBullets : MonoBehaviour
{
    [SerializeField] private int _shotsPerSecond = 1;
    [SerializeField] private GameObject _bulletPrefab = null;
    private Coroutine _firingCoroutine = null;
    private bool _firing = false;
    public bool Firing { get { return _firing; } }

    public Action IsFiring;

    private void Awake() 
    {
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
            if (_firing)
            {
                GameObject.Instantiate(_bulletPrefab, transform.position, transform.rotation, GameLogicManager.Instance.BulletParent);
                yield return delay;
            }
            else
            {
                yield return null;
            }
        }
    }
}
