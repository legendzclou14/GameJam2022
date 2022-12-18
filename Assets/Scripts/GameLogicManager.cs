using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogicManager : MonoBehaviour
{
    public static GameLogicManager Instance { get; private set; }

    [SerializeField] private Transform _bulletsParent = null;
    public Transform BulletParent { get { return _bulletsParent; } }
    [SerializeField] private Transform _enemyAttackParent = null;
    public Transform EnemyAttackParent { get { return _enemyAttackParent; } }
    [SerializeField] private Player _player = null;
    public Player Player { get { return _player; } }
    [SerializeField] private Enemy _enemyBoss = null;
    public Enemy EnemyBoss { get { return _enemyBoss; } }
    [SerializeField] private UI _ui = null;
    public UI UI { get { return _ui; } }

    [SerializeField] SpriteRenderer _flashImage = null;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioClip _phaseOneMusic;
    [SerializeField] private AudioClip _phaseTwoMusic;

    public bool IsInDialogue { get; private set; } = false;
    private bool _canSkipDialogue = false;
    private bool _skipDialogue = false;

    [SerializeField] private TalkingCharacter[] _introTalking = null;
    [SerializeField] private TalkingCharacter[] _betweenPhasesTalkingPt1 = null;
    [SerializeField] private TalkingCharacter[] _betweenPhasesTalkingPt2 = null;
    [SerializeField] private TalkingCharacter[] _finaleTalking = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Trying to create second GameLogicManager");
        }

        EnemyBoss.OnPhaseOneOver += (() => StartCoroutine(StartOfSecondPhase()));
        EnemyBoss.OnPhaseTwoOver += (() => StartCoroutine(StartFinaleTalking()));
        Player.OnPlayerDeath += (() => StartCoroutine(PlayerDeathSequence()));

        if (!Inventory.Instance.HasReachedCheckpoint)
        {
            StartCoroutine(StartOfGame());
        }
        else
        {
            StartCoroutine(StartOfSecondPhase());
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines(); 
        EnemyBoss.OnPhaseOneOver -= (() => StartCoroutine(StartOfSecondPhase()));
        EnemyBoss.OnPhaseTwoOver -= (() => StartCoroutine(StartFinaleTalking()));
        Player.OnPlayerDeath -= (() => StartCoroutine(PlayerDeathSequence()));
    }

    public void NextDialogue(InputAction.CallbackContext context)
    {
        if (context.performed && _canSkipDialogue)
        {
            _skipDialogue = true;
        }
    }

    private IEnumerator StartOfGame()
    {
        IsInDialogue = true;
        _canSkipDialogue = false;
        Inventory.Instance.RestoreInventory();
        yield return UI.TotalFadeOut(false, 2f);

        UI.TextBoxGO.SetActive(true);

        foreach (TalkingCharacter textBox in _introTalking)
        {
            yield return UI.FillTextBox(textBox);
            _canSkipDialogue = true;
            yield return new WaitUntil(() => _skipDialogue);
            _canSkipDialogue = false;
            _skipDialogue = false;
        }

        _musicSource.clip = _phaseOneMusic;
        _musicSource.Play();

        UI.TextBoxGO.SetActive(false);
        yield return UI.StartOfGame();
        IsInDialogue = false;

        _player.OnStartGame();
        _enemyBoss.OnStartGame();
    }

    private void KillAllAttacks()
    {
        for (int i = EnemyAttackParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(EnemyAttackParent.transform.GetChild(i).gameObject);
        }
        for (int i = BulletParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(BulletParent.transform.GetChild(i).gameObject);
        }
    }

    private IEnumerator StartOfSecondPhase()
    {
        if (Inventory.Instance.HasReachedCheckpoint)
        {
            IsInDialogue = true;
            _canSkipDialogue = false;
            Inventory.Instance.RestoreInventory();
            yield return UI.TotalFadeOut(false, 2f);
        }
        else
        {
            _musicSource.Stop();
        }

        KillAllAttacks();

        IsInDialogue = true;
        _canSkipDialogue = false;
        UI.TextBoxGO.SetActive(true);

        foreach (TalkingCharacter textBox in _betweenPhasesTalkingPt1)
        {
            yield return UI.FillTextBox(textBox);
            _canSkipDialogue = true;
            yield return new WaitUntil(() => _skipDialogue);
            _canSkipDialogue = false;
            _skipDialogue = false;
        }

        UI.TextBoxGO.SetActive(false);
        UI.ClearTextBox();
        yield return Flash(true, true, 3);
        EnemyBoss.SecondPhaseTransition();
        yield return Flash(false, true, 2);
        UI.TextBoxGO.SetActive(true);

        foreach (TalkingCharacter textBox in _betweenPhasesTalkingPt2)
        {
            yield return UI.FillTextBox(textBox);
            _canSkipDialogue = true;
            yield return new WaitUntil(() => _skipDialogue);
            _canSkipDialogue = false;
            _skipDialogue = false;
        }

        UI.TextBoxGO.SetActive(false);
        _musicSource.clip = _phaseTwoMusic;
        _musicSource.Play();

        if (Inventory.Instance.HasReachedCheckpoint)
        {
            yield return UI.StartOfGame();
            Player.OnStartGame();
        }
        else
        {
            yield return UI.FillEnemyBar();
        }
        IsInDialogue = false;
        EnemyBoss.StartSecondPhase();

        Inventory.Instance.HasReachedCheckpoint = true;
    }

    private IEnumerator StartFinaleTalking()
    {
        KillAllAttacks();

        IsInDialogue = true;
        _canSkipDialogue = false;
        UI.TextBoxGO.SetActive(true);

        foreach (TalkingCharacter textBox in _finaleTalking)
        {
            yield return UI.FillTextBox(textBox);
            _canSkipDialogue = true;
            yield return new WaitUntil(() => _skipDialogue);
            _canSkipDialogue = false;
            _skipDialogue = false;
        }

        yield return UI.TotalFadeOut(true, 2f);
        //load out
    }

    private IEnumerator PlayerDeathSequence()
    {
        _musicSource.Stop();
        KillAllAttacks();
        IsInDialogue = true;

        yield return Flash(true, false, 0.01f);
        UI.ShowUI(false);
        yield return Player.DeathCoroutine(); 
        yield return UI.TotalFadeOut(true, 2f);
        SceneManager.LoadScene("BattleScene");
    }

    private IEnumerator ReloadAfterGameOver()
    {
        Inventory.Instance.HasBeatenBoss = true;
        yield return UI.TotalFadeOut(true, 2f);
        SceneManager.LoadScene("BattleScene");
    }

    public IEnumerator Flash(bool on, bool isWhite = true, float time = 0.35f)
    {
        float alphaTarget = on ? 1 : 0;
        float alphaStart = on ? 0 : 1;
        float flashColor = isWhite ? 1 : 0;

        float timer = 0;
        float amount = 0;
        while (timer < time)
        {
            amount = Mathf.Lerp(alphaStart, alphaTarget, timer / time);
            _flashImage.color = new Color(flashColor, flashColor, flashColor, amount);
            timer += Time.deltaTime;
            yield return null;
        }
        _flashImage.color = new Color(flashColor, flashColor, flashColor, alphaTarget);
    }
}

[System.Serializable]
public struct TalkingCharacter
{
    public TextBoxTalker CharacterTalking;
    public string TextToSay;
}