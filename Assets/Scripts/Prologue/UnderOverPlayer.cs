using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderOverPlayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _object = null;
    int _playerOrder = 0;

    private void Start()
    {
        _playerOrder = PrologueManager.Instance.PlayerorderinLayer;
    }

    private void Update()
    {
        if (PrologueManager.Instance.PlayerTransform.position.y > transform.position.y)
        {
            _object.sortingOrder = _playerOrder + 1;
        }
        else
        {
            _object.sortingOrder = _playerOrder - 1;
        }
    }
}
