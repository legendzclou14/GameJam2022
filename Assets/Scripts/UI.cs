using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] Slider _playerHPSlider = null;
    [SerializeField] Slider _enemyHPSlider = null;

    private void OnDestroy()
    {
        StopAllCoroutines();
        GameLogicManager.Instance.Player.OnHealthChanged -= UpdatePlayerHP;
        GameLogicManager.Instance.EnemyBoss.OnHealthChanged -= UpdateEnemyHP;
    }

    private void UpdatePlayerHP(float hp)
    {
        _playerHPSlider.value = hp;
    }

    private void UpdateEnemyHP(float hp)
    {
        _enemyHPSlider.value = hp;
    }

    public IEnumerator StartOfGame()
    {
        float timer = 0;
        float amount = 0;
        float animTime = 2;
        while (timer < animTime)
        {
            amount = Mathf.Lerp(0, 1, timer / animTime);
            _playerHPSlider.value = amount;
            _enemyHPSlider.value = amount;
            timer += Time.deltaTime;
            yield return null;
        }

        amount = 1;
        _playerHPSlider.value = amount;
        _enemyHPSlider.value = amount;

        GameLogicManager.Instance.Player.OnHealthChanged += UpdatePlayerHP;
        GameLogicManager.Instance.EnemyBoss.OnHealthChanged += UpdateEnemyHP;
    }

    public IEnumerator FillEnemyBar()
    {
        float timer = 0;
        float amount = 0;
        float animTime = 2;
        while (timer < animTime)
        {
            amount = Mathf.Lerp(0, 1, timer / animTime);
            _enemyHPSlider.value = amount;
            timer += Time.deltaTime;
            yield return null;
        }

        amount = 1;
        _enemyHPSlider.value = amount;
    }
}
