using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class PlaneMovement : MonoBehaviour
{
    [SerializeField] private float _movementMultiplier = 1f;
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
        }
        else
        {
            _moveInputValue = Vector2.zero;
        }
    }

    private void Move()
    {
        _moveAmount = _moveInputValue * _movementMultiplier;
        transform.Translate(_moveAmount);
    }
}
