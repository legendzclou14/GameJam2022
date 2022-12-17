using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarAttack : MonoBehaviour
{
    [SerializeField] private GameObject _pillarPrefab = null;
    [SerializeField] private float _simultaneousPillars = 2;
    [SerializeField] private float _numberOfRounds = 3;
    [SerializeField] private float _timeBetweenRounds = 3;

    private void Awake()
    {
        StartCoroutine(StartPillars());
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator StartPillars()
    {
        int currentRound = 0;
        List<int> xPosUsed = new List<int>();
        WaitForSeconds delay = new WaitForSeconds(_timeBetweenRounds);

        while (currentRound < _numberOfRounds)
        {
            
            for(int i = 0; i < _simultaneousPillars; i++)
            {
                int x;
                do
                {
                    x = Random.Range(-4, 2);
                }
                while (xPosUsed.Contains(x));

                Vector3 spawnPos = new Vector3(x*2, 0, 0);  //*2 so they arent besides one eachother.

                GameObject.Instantiate(_pillarPrefab, spawnPos, Quaternion.identity, GameLogicManager.Instance.EnemyAttackParent);
            }

            currentRound++;
            yield return delay;
        }
    }
}
