using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayAttack : MonoBehaviour
{
    [SerializeField] private GameObject _rayPrefab = null;
    [SerializeField] private float _attackTime = 2f;
    [SerializeField] private float _startAngle = 0;
    [SerializeField] private float _endAngle = 180;
    [SerializeField] private float _waitBetweenRounds = 1f;

    private void Awake()
    {
        StartCoroutine(StartRay());
    }

    private IEnumerator StartRay()
    {
        float timer = 0;
        Transform rayTransform = GameObject.Instantiate(_rayPrefab, GameLogicManager.Instance.EnemyBoss.RaySpawnPoint.position, Quaternion.identity, GameLogicManager.Instance.EnemyAttackParent).transform;
        float currentAngle = -_startAngle;
        while (timer < _attackTime)
        {
            currentAngle = Mathf.Lerp(-_startAngle, -_endAngle, timer / _attackTime);
            rayTransform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(rayTransform.gameObject);

        yield return new WaitForSeconds(_waitBetweenRounds);

        timer = 0;
        currentAngle = -_endAngle;
        rayTransform = GameObject.Instantiate(_rayPrefab, GameLogicManager.Instance.EnemyBoss.RaySpawnPoint.position, Quaternion.identity, GameLogicManager.Instance.EnemyAttackParent).transform;
        while (timer < _attackTime)
        {
            currentAngle = Mathf.Lerp(-_endAngle, -_startAngle, timer / _attackTime);
            rayTransform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(rayTransform.gameObject);
        Destroy(gameObject);
    }
}
