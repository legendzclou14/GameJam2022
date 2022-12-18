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

    private void Awake()
    {
        gameObject.SetActive(false);
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
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

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
        gameObject.SetActive(false);
    }
}
