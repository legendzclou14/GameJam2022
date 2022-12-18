using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] Slider _playerHPSlider = null;
    [SerializeField] Slider _enemyHPSlider = null;
    public GameObject TextBoxGO = null;
    [SerializeField] TextMeshProUGUI _textBoxText = null;
    [SerializeField] Image _talkerPortrait = null;
    [SerializeField] Sprite[] _talkerSprites = null;
    [SerializeField] private AudioSource _voiceSource;
    [SerializeField] private AudioClip[] _voiceClips;
    [SerializeField] private Image _atkItem;
    [SerializeField] private Image _shieldItem;
    [SerializeField] private Image _healItem;

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

    public void UseItem(ItemType type)
    {
        switch (type)
        {
            case ItemType.ATK_BOOST:
                _atkItem.enabled = false;
                break;

            case ItemType.SHIELD:
                _shieldItem.enabled = false;
                break;

            case ItemType.HEAL:
                _healItem.enabled = false;
                break;

            default:
                break;
        }
    }

    public IEnumerator StartOfGame()
    {
        if (Inventory.Instance.AtkBoost > 0)
        {
            _atkItem.enabled = true;
        }
        if (Inventory.Instance.Shields > 0)
        {
            _shieldItem.enabled = true;
        }
        if (Inventory.Instance.Heals > 0)
        {
            _healItem.enabled = true;
        }

        _playerHPSlider.gameObject.SetActive(true);
        _enemyHPSlider.gameObject.SetActive(true);

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

    public IEnumerator FillTextBox(TalkingCharacter textBoxContent)
    {
        _textBoxText.text = string.Empty;
        _voiceSource.clip = _voiceClips[(int)textBoxContent.CharacterTalking];
        _talkerPortrait.sprite = _talkerSprites[(int)textBoxContent.CharacterTalking];

        _voiceSource.Play();
        foreach (char i in textBoxContent.TextToSay)
        {
            _textBoxText.text += i;
            yield return new WaitForSeconds(0.05f);
        }

        _voiceSource.Stop();
    }

    public void ClearTextBox()
    {
        _textBoxText.text = string.Empty;
    }
}

public enum TextBoxTalker
{
    PLAYER = 0, 
    ENEMY_PHASE_ONE = 1,
    ENEMY_PHASE_TWO = 2
}
