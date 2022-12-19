using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurntableInteractable : ChoiceInteractable
{
    public static AudioSource _source;
    [SerializeField] private AudioClip[] _songs;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    public override void ProcessChoice(int index)
    {
        _source.clip = _songs[index];
        _source.Play();
    }
}
