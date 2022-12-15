using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    [SerializeField] private bool _moving = false;
    [SerializeField] private Direction _movementDirection = Direction.LEFT;
    [SerializeField] private float _speed = 0.1f;
    private Vector3 _updateMovement;
    public int DamageAmount = 1;

    private void Awake() 
    {
        float movement = _movementDirection == Direction.LEFT ? -1 * _speed : 1 * _speed;
        _updateMovement = new Vector3(movement, 0, 0);
    }

    private void FixedUpdate() 
    {
        if (_moving)
        {
            transform.Translate(_updateMovement);
        }
    }

    void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}

public enum Direction
{
    LEFT,
    RIGHT
}