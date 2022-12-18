using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrologueUI : MonoBehaviour
{
    public GameObject TextBoxGO = null;
    [SerializeField] TextMeshProUGUI _textBoxText = null;
    [SerializeField] private AudioSource _voiceSource;
    [SerializeField] private Image _flashImage;
    [SerializeField] private GameObject _choiceButtonsPrefab;
    [SerializeField] private Transform _choiceButtonsParent;
    private List<ChoiceButton> _choiceButtons = new List<ChoiceButton>();

    private void Awake()
    {
        TextBoxGO.SetActive(false);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator TotalFadeOut(bool on, float time = 0.35f)
    {
        float alphaTarget = on ? 1 : 0;
        float alphaStart = on ? 0 : 1;

        float timer = 0;
        float amount = 0;
        while (timer < time)
        {
            amount = Mathf.Lerp(alphaStart, alphaTarget, timer / time);
            _flashImage.color = new Color(0, 0, 0, amount);
            timer += Time.deltaTime;
            yield return null;
        }
        _flashImage.color = new Color(0, 0, 0, alphaTarget);
    }

    public IEnumerator FillTextBox(string textBoxContent)
    {
        TextBoxGO.SetActive(true);

        _textBoxText.text = string.Empty;
        _voiceSource.Play();
        foreach (char i in textBoxContent)
        {
            _textBoxText.text += i;
            yield return new WaitForSeconds(0.05f);
        }

        _voiceSource.Stop();
    }

    public void ClearTextBox()
    {
        _textBoxText.text = string.Empty;
        TextBoxGO.SetActive(false);
    }

    private IEnumerator ChoiceDelay()
    {
        _canChangeChoice = false;
        yield return new WaitForSeconds(0.25f);
        _canChangeChoice = true;
    }

    int _currentButtonIndex = 0;
    public bool InChoice = false;
    private bool _canChangeChoice = true;

    public void ShowChoiceButtons(string[] buttonLabels)
    {
        InChoice = true;
        foreach (string label in buttonLabels)
        {
            ChoiceButton button = GameObject.Instantiate(_choiceButtonsPrefab, _choiceButtonsParent).GetComponent<ChoiceButton>();
            button.Init(label);
            _choiceButtons.Add(button);
        }

        _currentButtonIndex = 0;
        UpdateChoice();
    }

    public void LeftChoice()
    {
        if(InChoice && _canChangeChoice)
        {
            StartCoroutine(ChoiceDelay());
            _choiceButtons[_currentButtonIndex].Highlight(false);
            _currentButtonIndex--;
            UpdateChoice();
        }
    }
    
    public void RightChoice()
    {
        if (InChoice && _canChangeChoice)
        {
            StartCoroutine(ChoiceDelay());
            _choiceButtons[_currentButtonIndex].Highlight(false);
            _currentButtonIndex++;
            UpdateChoice();
        }
    }

    public void UpdateChoice()
    {
        if(_currentButtonIndex >= _choiceButtons.Count)
        {
            _currentButtonIndex = 0;
        }
        else if (_currentButtonIndex < 0)
        {
            _currentButtonIndex = _choiceButtons.Count - 1;
        }
        _choiceButtons[_currentButtonIndex].Highlight(true);
    }

    public int SelectButton()
    {
        foreach(var button in _choiceButtons)
        {
            Destroy(button.gameObject);
        }
        _choiceButtons.Clear();
        InChoice = false;
        return _currentButtonIndex;
    }
}
