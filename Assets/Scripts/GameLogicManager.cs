using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    public static GameLogicManager Instance { get; private set; }

    [SerializeField] private Transform _bulletsParent = null;
    public Transform BulletParent { get { return _bulletsParent; } }
    [SerializeField] private Transform _enemyAttackParent = null;
    public Transform EnemyAttackParent { get { return _enemyAttackParent; } }
    [SerializeField] private Player _player = null;
    public Player Player { get { return _player; } }
    [SerializeField] private Enemy _enemyBoss = null;
    public Enemy EnemyBoss { get { return _enemyBoss; } }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Trying to create second GameLogicManager");
        }
    }

    public void GameOver(bool win)
    {

    }

}
