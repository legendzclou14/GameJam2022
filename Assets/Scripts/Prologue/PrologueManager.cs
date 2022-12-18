using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
