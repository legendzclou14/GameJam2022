using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private Image _buttonImage;
    [SerializeField] private TextMeshProUGUI _buttonText;
    public string Label = "";

    private void Awake()
    {
        Highlight(false);
    }

    public void Init(string label)
    {
        Label = label;
        _buttonText.text = Label;
    }

    public void Highlight(bool highlight)
    {
        float target = highlight ? 1 : 0.5f;
        _buttonImage.color = new Color(target, target, target);
    }
}
