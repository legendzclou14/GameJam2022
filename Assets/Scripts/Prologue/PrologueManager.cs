using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueManager : MonoBehaviour
{
    public static PrologueManager Instance { get; private set; } = null;
    [SerializeField] private PrologueUI _prologueUI = null;
    public PrologueUI PrologueUI => _prologueUI;

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

    public void EndPrologue()
    {
        StartCoroutine(EndPrologueCoroutine());
    }

    private IEnumerator EndPrologueCoroutine()
    {
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
