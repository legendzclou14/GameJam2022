using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PlaneMovement : MonoBehaviour
{
    [SerializeField] private float _movementMultiplier = 1f;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    [SerializeField] private Sprite _upSprite = null;
    [SerializeField] private Sprite _downSprite = null;
    [SerializeField] private Sprite _leftSprite = null;
    [SerializeField] private Sprite _rightSprite = null;
    [SerializeField] private Sprite _idleSprite = null;
    [SerializeField] private float _spriteDeadzone = 0.5f;
    private Vector2 _moveInputValue = Vector2.zero;
    private Vector3 _moveAmount = Vector3.zero;
    
    private void FixedUpdate()
    {
        Move();       
    }

    public void ReadMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _moveInputValue = context.ReadValue<Vector2>();
            ChangeSprite();
        }
        else
        {
            _moveInputValue = Vector2.zero;
        }
    }

    private void ChangeSprite()
    {
        if (_moveInputValue.y > _spriteDeadzone)
        {
            _spriteRenderer.sprite = _upSprite;
        }
        else if (_moveInputValue.y < -_spriteDeadzone)
        {
            _spriteRenderer.sprite = _downSprite;
        }
        else
        {
            if (_moveInputValue.x > _spriteDeadzone)
            {
                _spriteRenderer.sprite = _rightSprite;
            }
            else if (_moveInputValue.x < -_spriteDeadzone)
            {
                _spriteRenderer.sprite = _leftSprite;
            }
            else
            {
                _spriteRenderer.sprite = _idleSprite;
            }
        }
    }

    private void Move()
    {
        _moveAmount = _moveInputValue * _movementMultiplier;
        transform.Translate(_moveAmount);
    }
}
