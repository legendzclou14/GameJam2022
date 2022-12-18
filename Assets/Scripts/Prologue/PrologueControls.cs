using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PrologueControls : MonoBehaviour
{
    public static bool IsInteracting = false;
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
    private Interactable _currentInteractable = null;

    private void FixedUpdate()
    {
        Move();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed && _currentInteractable != null)
        {
            if(!IsInteracting)
            {
                _currentInteractable.Interact();
                IsInteracting = true;
            }
            else
            {
                _currentInteractable.NextDialogue();
            }
        }
    }


    public void ReadMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!IsInteracting)
            {
                ChangeSprite();
                _moveInputValue = context.ReadValue<Vector2>();
            }
            else if (PrologueManager.Instance.PrologueUI.InChoice)
            {
                _moveInputValue = Vector2.zero;
                Vector2 input = context.ReadValue<Vector2>();
                if (input.x < -_spriteDeadzone)
                {
                    PrologueManager.Instance.PrologueUI.LeftChoice();
                }
                else if (input.x > _spriteDeadzone)
                {
                    PrologueManager.Instance.PrologueUI.RightChoice();
                }
            }
        }
        else
        {
            _moveInputValue = Vector2.zero;
            ChangeSprite();
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
        if (!IsInteracting)
        {
            _moveAmount = _moveInputValue * _movementMultiplier;
            transform.Translate(_moveAmount);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            _currentInteractable = collision.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactable"))
        {
            _currentInteractable = null;
        }
    }
}
