using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringEnemy : MonoBehaviour
{
    [SerializeField] private float _delayBeforeFiring = 1f;
    [SerializeField] private int _shotsPerSecond = 1;
    [SerializeField] private float _attackTime = 5;
    [SerializeField] [Range(0, 4)] private float _maxHeight = 2;
    [SerializeField] private float _verticallSpeed = 0.5f;
    [SerializeField] private GameObject _bulletPrefab = null;
    private Coroutine _firingCoroutine = null;
    private Coroutine _movementCoroutine = null;

    private void Awake()
    {
        _firingCoroutine = StartCoroutine(FireCoroutine());
        _movementCoroutine = StartCoroutine(MovementCoroutine());
    }
    private void OnDestroy()
    {
        StopCoroutine(_firingCoroutine);
        StopCoroutine(_movementCoroutine);
    }

    private IEnumerator FireCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeFiring);

        float delay = 1f / _shotsPerSecond;
        float timer = 0;
        float lastAttackTime = -999;
        while (true)
        {
            if (timer - lastAttackTime >= delay)
            {
                GameObject.Instantiate(_bulletPrefab, transform.position, transform.rotation, GameLogicManager.Instance.EnemyAttackParent);
                lastAttackTime = timer;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MovementCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeFiring);

        float timer = 0;
        Vector3 movement = new Vector3(0, _verticallSpeed, 0);

        while (timer < _attackTime)
        {
            transform.Translate(movement);
            if (Mathf.Abs(transform.position.y) >= _maxHeight)
            {
                movement *= -1;
            }

            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        Destroy(gameObject);
    }
}
