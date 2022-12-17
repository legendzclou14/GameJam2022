using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bomb : DamagingObject
{
    [SerializeField] private float _timeBeforeExplosion = 2;
    [SerializeField] private CircleCollider2D _collider = null;
    [SerializeField] private SpriteRenderer _spriteRenderer = null;


    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(DelayExplosion());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private IEnumerator DelayExplosion()
    {
        yield return new WaitForSeconds(_timeBeforeExplosion);
        _collider.enabled = true;
        _spriteRenderer.color = Color.red;  //debug to have visual feedback, delete when implementing new anim
    }
}
