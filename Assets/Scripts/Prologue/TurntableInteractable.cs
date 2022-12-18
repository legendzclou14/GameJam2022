using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurntableInteractable : ChoiceInteractable
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _songs;
    public override void ProcessChoice(int index)
    {
        _source.clip = _songs[index];
        _source.Play();
    }
}
