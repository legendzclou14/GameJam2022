using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAttack : MonoBehaviour
{
    [SerializeField] private float _delayBetweenBombs = 2;
    [SerializeField] private float _numberOfBombs = 5;
    [SerializeField] private GameObject _bombPrefab = null;

    private void Awake()
    {
        StartCoroutine(StartBombs());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartBombs()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayBetweenBombs);
        int currentBomb = 0;

        while (currentBomb < _numberOfBombs)
        {
            GameObject.Instantiate(_bombPrefab, GameLogicManager.Instance.Player.transform.position, Quaternion.identity, GameLogicManager.Instance.EnemyAttackParent);

            currentBomb++;
            yield return delay;
        }

        Destroy(gameObject);
    }
}
