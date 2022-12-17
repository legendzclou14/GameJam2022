using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    [SerializeField] private bool _moving = false;
    [SerializeField] private Direction _movementDirection = Direction.LEFT;
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private float _lifetime = 2f;
    [SerializeField] private bool _destroyOnHit = true;
    [SerializeField] private int _damageAmount = 1;

    private bool _damageTarget;
    private Vector3 _updateMovement;
    private string _targetTag = "";

    protected virtual void Awake() 
    {
        float movement = _movementDirection == Direction.LEFT ? -1 * _speed : 1 * _speed;
        _updateMovement = new Vector3(movement, 0, 0);
        
        if (_lifetime > 0)
        {
            Destroy(gameObject, _lifetime);
        }

        if (gameObject.CompareTag("DamagePlayer"))
        {
            _targetTag = "Player";
        }
        else if (gameObject.CompareTag("DamageEnemy"))
        {
            _targetTag = "Enemy";
        }
    }

    private void FixedUpdate() 
    {
        if (_moving)
        {
            transform.Translate(_updateMovement);
        }

        DamageOnTick();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_targetTag))
        {
            _damageTarget = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_targetTag))
        {
            _damageTarget = false;
        }
    }

    private void DamageOnTick()
    {
        if (_damageTarget)
        {
            if (gameObject.CompareTag("DamagePlayer"))
            {
                GameLogicManager.Instance.Player.Damage(_damageAmount);
            }
            else if (gameObject.CompareTag("DamageEnemy"))
            {
                GameLogicManager.Instance.EnemyBoss.Damage(_damageAmount);
            }

            if (_destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }
}

public enum Direction
{
    LEFT,
    RIGHT
}