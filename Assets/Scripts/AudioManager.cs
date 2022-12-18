using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    private struct SFXSoundType
    {
        [Range(0, 1)] public float Volume;
        public AudioClip AudioClip;
        public SFXSound SFXSound;
    }

    [System.Serializable]
    public enum SFXSound
    {
        CLICK,
        HOVER,
        HURT,
        PLAYERDIE
    }

    [SerializeField]
    private AudioSource _musicAudioSource = default;

    [SerializeField]
    private float _musicVolume = 0.15f;

    [SerializeField]
    private AudioSource _sfxAudioSource = default;

    [SerializeField]
    private List<SFXSoundType> _sfx;

    private Dictionary<SFXSound, SFXSoundType> _sfxDictionnary = new Dictionary<SFXSound, SFXSoundType>();

    private bool _isMusicMuted;
    private bool _isSFXMuted;

    public bool IsMusicMuted => _isMusicMuted;
    public bool IsSFXMuted => _isSFXMuted;

    private static AudioManager instance = default;
    public static AudioManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {

        foreach (SFXSoundType item in _sfx)
        {
            _sfxDictionnary.Add(item.SFXSound, item);
        }
    }

    public void PlayMusic()
    {
        _musicAudioSource.volume = _musicVolume;
        _musicAudioSource.Play();
    }
    
    public void StopMusic()
    {
        _musicAudioSource.Stop();
    }

    public void MuteMusic()
    {
        _isMusicMuted = true;
        _musicAudioSource.volume = 0.0f;
    }

    public void UnmuteMusic()
    {
        _isMusicMuted = false;
        _musicAudioSource.volume = _musicVolume;
    }

    public void PlaySFX(SFXSound sFXSound)
    {
        _sfxAudioSource.clip = _sfxDictionnary[sFXSound].AudioClip;
        if (_isSFXMuted)
        {
            _sfxAudioSource.volume = 0;
        }
        else
        {
            _sfxAudioSource.volume = _sfxDictionnary[sFXSound].Volume;
        }
        _sfxAudioSource.Play();
    }


    public void MuteSFX()
    {
        _isSFXMuted = true;
    }

    public void UnmuteSFX()
    {
        _isSFXMuted = false;
    }
}
