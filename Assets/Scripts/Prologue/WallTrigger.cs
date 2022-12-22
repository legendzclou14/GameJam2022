using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _wallsToFade;
    private Coroutine _fadeCoroutine = null;
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

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
        float alphaStart = -1;
        foreach (SpriteRenderer wall in _wallsToFade)
        {
            if (wall != null)
            {
                alphaStart = wall.color.a;
                break;
            }
        }

        if (alphaStart == -1)
        {
            alphaTarget = visible ? 0 : 1;
        }

        float timer = 0;
        float amount = 0;
        List<SpriteRenderer> sprites = new List<SpriteRenderer>();
        foreach (var renderer in _wallsToFade)
        {
            if (renderer != null)
            {
                sprites.Add(renderer);
            }
        }
        while (timer < .5f)
        {
            amount = Mathf.Lerp(alphaStart, alphaTarget, timer / .3f);
            foreach(var renderer in sprites)
            {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, amount);
            }
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (var renderer in sprites)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alphaTarget);
        }
        _fadeCoroutine = null;
    }
}
