using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{   
    [SerializeField] private float crossFadeRate = 1.5f; 

    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;
    [SerializeField] private AudioSource soundSource;
    
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    [SerializeField] private AudioClip levelBGMusic;        
    
    private AudioSource _activeMusic;
    private AudioSource _inactiveMusic;
        
    private bool _isCrossFading;
    
    private string _musicVolumeIdentifier = "MusicVolume";
    private string _soundVolumeIdentifier = "SoundVolume";
        
    private float _musicVolume;
    public float MusicVolume
    {
        get {return _musicVolume;}
        set
        {
            _musicVolume = value;
            
            if (music1Source != null && !_isCrossFading)
            {
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume;
            }            
            PlayerPrefs.SetFloat(_musicVolumeIdentifier, value);
            musicSlider.value = value;
        }
    }    

    public float SoundVolume 
    {
        get {return AudioListener.volume;}
        set 
        {
            AudioListener.volume = value;
            PlayerPrefs.SetFloat(_soundVolumeIdentifier, value);
            soundSlider.value = value;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        soundSource.PlayOneShot(clip);
    }    

    public void PlayLevelMusic()
    {
        PlayMusic(levelBGMusic);
    }    

    public void StopMusic()
    {
        _activeMusic.Stop();
        _inactiveMusic.Stop();
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (!_isCrossFading)
        {
            _activeMusic.clip = clip;
            _activeMusic.Play();
        }
        else
        {
            StartCoroutine(CrossFadeMusic(clip));
        }        
    }

    private void Awake()
    {   
        float musicVolume = 0.5f;
        float soundVolume = 0.5f;

        if (PlayerPrefs.HasKey(_musicVolumeIdentifier))
        {                        
            musicVolume = PlayerPrefs.GetFloat(_musicVolumeIdentifier);			            
        }         

        if (PlayerPrefs.HasKey(_soundVolumeIdentifier))
        {            
            soundVolume = PlayerPrefs.GetFloat(_soundVolumeIdentifier);			                        
        } 

        if (PlayerPrefs.HasKey("_Mute"))
		{            
			int mute = PlayerPrefs.GetInt("_Mute");
			if (mute == 1) 
            {
                musicVolume = 0;
                soundVolume = 0;                
            }			
		} 
		else 
		{			
			PlayerPrefs.SetInt("_Mute", 0);
		}       
        
        MusicVolume = musicVolume;
        SoundVolume = soundVolume;
    }
    
    private void Start()
    {                
        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerPause = true;        
        
        if (music1Source == null || music2Source == null)
        {
            if (music1Source == null)
            {
                _activeMusic = music2Source;
            }
            else
            {
                _activeMusic = music1Source;
            }
        }
        else
        {
            _activeMusic = music1Source;        
            _inactiveMusic = music2Source;        
        }        
    }        

    private IEnumerator CrossFadeMusic(AudioClip clip)
    {
        _isCrossFading = true;
        _inactiveMusic.clip = clip;
        _inactiveMusic.volume = 0;
        _inactiveMusic.Play();
        float scaledRate = crossFadeRate * _musicVolume;
        
        while (_activeMusic.volume > 0) 
        {
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;
            yield return null; 
        }
        
        AudioSource temp = _activeMusic;
        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;
        _inactiveMusic = temp;
        _inactiveMusic.Stop();
        _isCrossFading = false;
    }    
}