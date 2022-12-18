using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _wallsToFade;
    private Coroutine _fadeCoroutine = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FadeWalls(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FadeWalls(true);
        }
    }

    public void FadeWalls(bool visible)
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }
        _fadeCoroutine = StartCoroutine(FadeTargetWalls(visible));
    }

    private IEnumerator FadeTargetWalls(bool visible)
    {
        float alphaTarget = visible ? 1 : 0;
        float alphaStart = _wallsToFade[0].color.a;

        float timer = 0;
        float amount = 0;
        while (timer < .5f)
        {
            amount = Mathf.Lerp(alphaStart, alphaTarget, timer / .3f);
            foreach(var renderer in _wallsToFade)
            {
                renderer.color = new Color(0, 0, 0, amount);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (var renderer in _wallsToFade)
        {
            renderer.color = new Color(0, 0, 0, amount);
        }
        _fadeCoroutine = null;
    }
}
