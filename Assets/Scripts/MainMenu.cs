using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Image _fadeImage;
    [SerializeField] private GameObject _creditsImage;
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioClip _selectMusic;

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlay);
        _quitButton.onClick.AddListener(OnQuit);
        _creditsButton.onClick.AddListener(OnCloseCredits);

        if (Inventory.Instance != null)
        {
            OnBackToMainMenu();
            Destroy(Inventory.Instance.gameObject);
            Inventory.Instance = null;
        }
        else
        {
            _playButton.Select();
        }

    }

    private void OnDestroy()
    {
        _playButton.onClick.RemoveListener(OnPlay);
        _quitButton.onClick.RemoveListener(OnQuit);
        _creditsButton.onClick.RemoveListener(OnCloseCredits);
    }

    private void OnBackToMainMenu()
    {
        _playButton.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);
        _creditsImage.SetActive(true);
        _creditsButton.gameObject.SetActive(true);
        _creditsButton.Select();
    }

    private void OnPlay()
    {
        StartCoroutine(OnPlayCoroutine());
    }

    private void OnCloseCredits()
    {
        _playButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
        _creditsImage.SetActive(false);
        _creditsButton.gameObject.SetActive(false);
        _playButton.Select();
    }

    private IEnumerator OnPlayCoroutine()
    {
        _musicSource.clip = _selectMusic;
        _musicSource.loop = false;
        _musicSource.Play();
        yield return TotalFadeOut(2);
        SceneManager.LoadScene("PrologueScene");
    }

    public IEnumerator TotalFadeOut(float time)
    {
        float alphaTarget = 1;
        float alphaStart = 0;

        float timer = 0;
        float amount;
        while (timer < time)
        {
            amount = Mathf.Lerp(alphaStart, alphaTarget, timer / time);
            _fadeImage.color = new Color(0, 0, 0, amount);
            timer += Time.deltaTime;
            yield return null;
        }
        _fadeImage.color = new Color(0, 0, 0, alphaTarget);
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
