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
    [SerializeField] private Sprite[] _upSprite = null;
    [SerializeField] private Sprite[] _downSprite = null;
    [SerializeField] private Sprite[] _leftSprite = null;
    [SerializeField] private Sprite[] _rightSprite = null;
    [SerializeField] private Sprite[] _idleSprite = null;
    [SerializeField] private float _spriteDeadzone = 0.5f;
    private FireBullets _fireBullets = null;
    private Vector2 _moveInputValue = Vector2.zero;
    private Vector3 _moveAmount = Vector3.zero;
    private Rect cameraRect;

    void Start()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));
        cameraRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
    }

    private void Awake()
    {
        _fireBullets = GetComponent<FireBullets>();
        _fireBullets.IsFiring += ChangeSprite;
    }

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
            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        int spriteIndex = _fireBullets.Firing ? 1 : 0;  //Firing sprites are index 1, no firing are index 0
        if (_moveInputValue.y > _spriteDeadzone)
        {
            _spriteRenderer.sprite = _upSprite[spriteIndex];
        }
        else if (_moveInputValue.y < -_spriteDeadzone)
        {
            _spriteRenderer.sprite = _downSprite[spriteIndex];
        }
        else
        {
            if (_moveInputValue.x > _spriteDeadzone)
            {
                _spriteRenderer.sprite = _rightSprite[spriteIndex];
            }
            else if (_moveInputValue.x < -_spriteDeadzone)
            {
                _spriteRenderer.sprite = _leftSprite[spriteIndex];
            }
            else
            {
                _spriteRenderer.sprite = _idleSprite[spriteIndex];
            }
        }
    }

    private void Move()
    {
        _moveAmount = _moveInputValue * _movementMultiplier;
        transform.Translate(_moveAmount);
        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, cameraRect.xMin, cameraRect.xMax),
            Mathf.Clamp(transform.position.y, cameraRect.yMin, cameraRect.yMax));
    }
}
