using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    public static PrologueManager Instance { get; private set; } = null;
    [SerializeField] private PrologueUI _prologueUI = null;
    public PrologueUI PrologueUI => _prologueUI;
    [SerializeField] private int _playerorderinLayer = 10;
    public int PlayerorderinLayer => _playerorderinLayer;
    [SerializeField] private GameObject[] _despawnOnPrologue;
    [SerializeField] private GameObject[] _despawnOnEpilogue;
    public Transform PlayerTransform;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if(Inventory.Instance.HasBeatenBoss)
        {
            foreach (GameObject go in _despawnOnEpilogue)
            {
                Destroy(go);
            }
        }
        else
        {
            foreach(GameObject go in _despawnOnPrologue)
            {
                Destroy(go);
            }

        }
    }

    public void EndPrologue()
    {
        StartCoroutine(EndPrologueCoroutine());
    }

    private IEnumerator EndPrologueCoroutine()
    {
        StartCoroutine(GameLogicManager.FadeOut(TurntableInteractable._source, 2f));
        yield return PrologueUI.TotalFadeOut(true, 2);
        
        if (!Inventory.Instance.HasBeatenBoss)
        {
            SceneManager.LoadScene("BattleScene");
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
