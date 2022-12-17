using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAttack : MonoBehaviour
{
    [SerializeField] private GameObject _rayPrefab = null;
    [SerializeField] private float _attackTime = 2f;
    [SerializeField] private float _endAngle = 90;

    private void Awake()
    {
        StartCoroutine(StartRay());
    }

    private IEnumerator StartRay()
    {
        float timer = 0;
        float currentAngle = 0;
        Transform _rayTransform = GameObject.Instantiate(_rayPrefab, GameLogicManager.Instance.EnemyBoss.transform.position, Quaternion.identity, GameLogicManager.Instance.EnemyAttackParent).transform;
        while (timer < _attackTime)
        {
            currentAngle = Mathf.Lerp(0, -_endAngle, timer / _attackTime);
            _rayTransform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(_rayTransform.gameObject);
    }
}
