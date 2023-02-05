using System.Collections.Generic;
using System.Collections;
using UnityEngine;


/// <summary>
/// The singleton Audio Manager.
/// </summary>
public class AudioManager : MonoBehaviour 
{
    /// <summary>
    /// The different music available.
    /// </summary>
    public enum Music
    {
        Game,
    }


    /// <summary>
    /// The different sound effects available.
    /// </summary>
    public enum SoundEffect
    {
        
    }



    static AudioManager                         _instance;


    Dictionary<Music, AudioClip[]>              _music;
    Dictionary<SoundEffect, AudioClip[]>        _SoundEffects;

    List<AudioSource>                           _soundEffectSources;
    AudioSource                                 _loopingSoundEffectSource;
    AudioSource                                 _musicSource;

    const int                                   AUDIO_EFFECT_SOURCES = 10;



    /// <summary>
    /// The singleton of Audio Manager.
    /// </summary>
    /// <returns></returns>
    static public AudioManager Instance()
    {
        if (_instance == null)
        {
            //Create audio manager
            GameObject audioManager = new GameObject("AudioManager");

            //add AudioManager to the object
            _instance = audioManager.AddComponent<AudioManager>();
            //initialize that object
            _instance.Initialise();

            //stop it from destroying itself
            DontDestroyOnLoad(audioManager);
        }

        return _instance;
    }


    /// <summary>
    /// Initialise the Audio Manager and load all of the audio.
    /// </summary>
    private void Initialise()
    {
        // initialise the resources
        _music = new Dictionary<Music, AudioClip[]>(System.Enum.GetValues(typeof(Music)).Length);
        _SoundEffects = new Dictionary<SoundEffect, AudioClip[]>(System.Enum.GetValues(typeof(SoundEffect)).Length);

        // create the audio sources
        // and load the audio content
        LoadAudioSources();
        LoadContent();
    }


    /// <summary>
    /// Create the neccessary audio sources.
    /// </summary>
    private void LoadAudioSources()
    {
        _soundEffectSources = new List<AudioSource>();

        AudioSource audioSource;
        //sets a music source
        audioSource = gameObject.AddComponent<AudioSource>();
        _musicSource = audioSource;
        _musicSource.playOnAwake = true;
        _musicSource.loop = true;

        StartCoroutine(LoadAudioEffectSources());

        //sets a looping sound effect source
        audioSource = gameObject.AddComponent<AudioSource>();
        _loopingSoundEffectSource = audioSource;
        _loopingSoundEffectSource.playOnAwake = false;
        _loopingSoundEffectSource.loop = true;
    }

    private IEnumerator LoadAudioEffectSources()
    {
        AudioSource audioSource;
        for (int i = 0; i < AUDIO_EFFECT_SOURCES; i++)
        {//adds audio source componenets to the object
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            _soundEffectSources.Add(audioSource);

            yield return null;
        }
    }


    /// <summary>
    /// Load all of the audio content
    /// </summary>
    private void LoadContent()
    {
        // Music
        int count = System.Enum.GetValues(typeof(Music)).Length;
        for (int i = 0; i < count; i++)
        {
            Music music = (Music)i;
            _music.Add(music, GetAllAudioContent("Audio/Music/" + music.ToString()));
        }

        // UI Sound Effects
        count = System.Enum.GetValues(typeof(SoundEffect)).Length;
        for (int i = 0; i < count; i++)
        {
            SoundEffect soundEffect = (SoundEffect)i;
            _SoundEffects.Add(soundEffect, GetAllAudioContent("Audio/UI/" + soundEffect.ToString()));
        }
    }

    private AudioClip[] GetAllAudioContent(string path)
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(path);

        return clips;
    }

    private AudioClip[] GetAudioContent(string path)
    {
        AudioClip[] clips = new AudioClip[1]
        {
            Resources.Load<AudioClip>(path)
        };

        return clips;
    }


    /// <summary>
    /// Play specific music (song)
    /// </summary>
    /// <param name="music"></param>
    public void PlayMusic(Music music)
    {
        AudioClip clip = PickRandomAudioClip(_music[music]);

        if (_musicSource.clip != clip)
        {// if the music clip isn't already playing
            _musicSource.Stop();
            _musicSource.clip = clip;
            _musicSource.Play();
        }
    }



    /// <summary>
    /// Play a specific sound effect.
    /// </summary>
    /// <param name="soundEffect"></param>
    public void PlayUISoundEffect(SoundEffect soundEffect, float volume = 1.0f)
    {
        AudioClip clip = PickRandomAudioClip(_SoundEffects[soundEffect]);
        bool played = false;

        for (int i = 0; i < _soundEffectSources.Count; i++)
        {//look for an avaialable channel
            if (!_soundEffectSources[i].isPlaying)
            {//if its free play the sound effect
                _soundEffectSources[i].pitch = Random.Range(0.8f, 1.2f);
                _soundEffectSources[i].PlayOneShot(clip, volume);
                played = true;
                break;
            }
        }

        if (!played)
        {
            Debug.LogWarning("Not played Sound Effect: " +
                                soundEffect.ToString() +
                             " no audio source available");
            GenerateExtraSoundEffectSource(clip, volume);
        }
    }


    private AudioClip PickRandomAudioClip(AudioClip[] clips)
    {
        AudioClip clip = clips[0];

        if (clips.Length > 1)
        {
            int index = Random.Range(0, clips.Length);
            clip = clips[index];
        }

        return clip;
    }


    private void GenerateExtraSoundEffectSource(AudioClip clip, float volume)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.PlayOneShot(clip, volume);
        _soundEffectSources.Add(audioSource);
    }
}
