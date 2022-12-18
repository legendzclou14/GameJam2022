using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Animator _animatorPhase1 = null;
    [SerializeField] private Animator _animatorPhase2 = null;
    [SerializeField] private int _maxHPFirstPhase = 200;
    [SerializeField] private int _maxHPSecondPhase = 200;
    [SerializeField] private float _timeBetweenAttacksPhaseOne = 15;
    [SerializeField] private float _timeBetweenAttacksPhaseTwo = 15;
    [SerializeField] private GameObject _zigZagAttackPrefab;
    [SerializeField] private GameObject _pillarAttackPrefab;
    [SerializeField] private GameObject _bombAttackPrefab;
    [SerializeField] private GameObject _rayAttackPrefab;
    [SerializeField] private SpriteRenderer _phase1Renderer;
    [SerializeField] private GameObject _phase2GO;
    [SerializeField] private AudioSource _hurtSource;
    public Transform RaySpawnPoint;
    private int _idleAnim, _pillarAnim, _bulletAnim, _bombAnim, _deathAnim, _phase2AbilityAnim, _phase2RayAnim, _phase2IdleAnim;
    
    private int _maxHP = 0;
    private int _currentHP = 0;
    private int _phase = 1;
    private bool _canTakeDamage = false;

    public Action<float> OnHealthChanged;
    public Action OnPhaseOneOver;
    public Action OnPhaseTwoOver;

    private void Awake()
    {
        _maxHP = _maxHPFirstPhase;
        _currentHP = _maxHP;

        _idleAnim = Animator.StringToHash("Idle");
        _pillarAnim = Animator.StringToHash("Pillar");
        _bulletAnim = Animator.StringToHash("Bullet");
        _bombAnim = Animator.StringToHash("Bomb");
        _deathAnim = Animator.StringToHash("Death");
        _phase2AbilityAnim = Animator.StringToHash("Ability");
        _phase2RayAnim = Animator.StringToHash("DeathRay");
        _phase2IdleAnim = Animator.StringToHash("IdlePhase2");
    }

    public void PlayAnimation(int animationID)
    {
        if (animationID == _phase2AbilityAnim || animationID == _phase2RayAnim || animationID == _phase2IdleAnim)
        {
            if (_animatorPhase2 != null)
            {
                _animatorPhase2.SetTrigger(animationID);
                return;
            }
        }

        if (_animatorPhase1 != null)
        {
            _animatorPhase1.SetTrigger(animationID);
        }
    }

    public void SecondPhaseTransition()
    {
        _phase1Renderer.enabled = false;
        _phase2GO.SetActive(true);
    }

    public void OnStartGame()
    {
        _canTakeDamage = true;
        StartCoroutine(PhaseOneAttacks());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void CheckHP()
    {
        _currentHP = _currentHP > _maxHP ? _maxHP : _currentHP;
        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke((float)_currentHP / _maxHP);
        }

        if (_currentHP <= 0)
        {
            _canTakeDamage = false;
            
            StopAllCoroutines();
            if (_phase == 1)
            {
                PlayAnimation(_deathAnim);
                OnPhaseOneOver.Invoke();
            }
            else
            {
                OnPhaseTwoOver.Invoke();
                _phase = 3;
            }
        }
    }

    public void Damage(int damageAmount)
    {
        if (_canTakeDamage)
        {
            _hurtSource.Play();
            _currentHP -= damageAmount;
            CheckHP();
        }
    }

    public void StartZigZagAttack()
    {
        if (_phase == 1)
        {
            PlayAnimation(_bulletAnim);
        }
        else if (_phase == 2)
        {
            PlayAnimation(_phase2AbilityAnim);
        }

        GameObject.Instantiate(_zigZagAttackPrefab, GameLogicManager.Instance.EnemyAttackParent);
    }
    
    public void StartPillarAttack()
    {
        if (_phase == 1)
        {
            PlayAnimation(_pillarAnim);
        }
        else if (_phase == 2)
        {
            PlayAnimation(_phase2AbilityAnim);
        }

        GameObject.Instantiate(_pillarAttackPrefab, GameLogicManager.Instance.EnemyAttackParent);
    }

    public void StartBombAttack()
    {
        if (_phase == 1)
        {
            PlayAnimation(_bombAnim);
        }
        else if (_phase == 2)
        {
            PlayAnimation(_phase2AbilityAnim);
        }

        GameObject.Instantiate(_bombAttackPrefab, GameLogicManager.Instance.EnemyAttackParent);
    }

    public void StartRayAttack()
    {
        PlayAnimation(_phase2RayAnim);

        GameObject.Instantiate(_rayAttackPrefab, GameLogicManager.Instance.EnemyAttackParent);
    }

    public void StartSecondPhase()
    {
        _maxHP = _maxHPSecondPhase;
        _currentHP = _maxHP;
        _canTakeDamage = true;
        _phase = 2;
        StartCoroutine(PhaseTwoAttacks());
    }

    private void StartAttack(int attackIndex)
    {
        if (!GameLogicManager.Instance.IsInDialogue)
        {
            switch (attackIndex)
            {
                case 0:
                    StartZigZagAttack();
                    break;

                case 1:
                    StartPillarAttack();
                    break;

                case 2:
                    StartBombAttack();
                    break;

                case 3:
                    StartRayAttack();
                    break;

                default:
                    break;
            }
        }
    }

    private IEnumerator PhaseOneAttacks()
    {
        int[] startArray = { -1, -1, -1 };
        int[] choicesArray = { 0, 1, 2 };
        int[] lastAttackOrder = {-1, -1, -1};
        int[] currentAttackOrder = { -1, -1, -1};
        List<int> choices = new List<int>(choicesArray);
        int currentChoice;

        while (_phase == 1)
        {
            do {
                currentChoice = choices[UnityEngine.Random.Range(0, choices.Count)];
                currentAttackOrder[0] = currentChoice;
            } while (currentAttackOrder[0] == lastAttackOrder[2]);

            choices.Remove(currentChoice);

            if (currentAttackOrder[0] == lastAttackOrder[0])
            {
                do
                {
                    currentChoice = choices[UnityEngine.Random.Range(0, choices.Count)];
                    currentAttackOrder[1] = currentChoice;
                } while (currentAttackOrder[1] == lastAttackOrder[1]);
                choices.Remove(currentChoice);
            }
            else
            {
                currentChoice = choices[UnityEngine.Random.Range(0, choices.Count)];
                currentAttackOrder[1] = currentChoice;
                choices.Remove(currentChoice);
            }

            currentChoice = choices[0];
            currentAttackOrder[2] = currentChoice;
            choices.Remove(currentChoice);

            for (int i = 0; i < 3; i++)
            {
                StartAttack(currentAttackOrder[i]);
                yield return new WaitForSeconds(_timeBetweenAttacksPhaseOne);
            }

            Array.Copy(currentAttackOrder, lastAttackOrder, 3);
            Array.Copy(startArray, currentAttackOrder, 3);
            choices.AddRange(choicesArray);
        }
    }

    private IEnumerator PhaseTwoAttacks()
    {
        int lastNotUsed = UnityEngine.Random.Range(0, 3);
        int[] choicesArray = { 0, 1, 2 };
        List<int> choices = new List<int>(choicesArray);
        List<int> chosenAttacks = new List<int>();
        int current;

        while (_phase == 2)
        {
            current = lastNotUsed;
            chosenAttacks.Add(current);
            choices.Remove(current);

            current = choices[UnityEngine.Random.Range(0, 2)];
            chosenAttacks.Add(current);
            choices.Remove(current);

            lastNotUsed = choices[0];
            choices.Remove(lastNotUsed);

            choices.AddRange(choicesArray);

            if ((float)_currentHP / _maxHP < 0.5f && UnityEngine.Random.Range(0, 3) == 0)
            {
                StartAttack(3);
            }
            else
            {
                foreach(int attackIndex in chosenAttacks)
                {
                    StartAttack(attackIndex);
                }
            }

            chosenAttacks.Clear();
            yield return new WaitForSeconds(_timeBetweenAttacksPhaseTwo);
        }
    }
}
