using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameSound[] sounds;

    public static AudioManager instance;

    [Serializable]
    public class GameSound 
    {
        public AudioManager.Sound sound;
        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;

        [Range(.1f, 3f)]
        public float pitch;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }

    public enum Sound 
    {
        Click,
        Menu,
        Start,
        Brif,
        Level,
        Win,
        Lost
    }


    void Awake () 
    {
        if (instance == null) 
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (GameSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public void PlaySound(Sound sound) {
        AudioSource audioSource = GetAudioSource(sound);
        audioSource.Play();
    }

    public void StopSound(Sound sound) {
        AudioSource audioSource = GetAudioSource(sound);
        audioSource.Stop();
    }

    public void StopSmoothly(Sound sound) {
        AudioSource audioSource = GetAudioSource(sound);
        audioSource.Stop();
    }

    private AudioSource GetAudioSource(Sound sound) {

        GameSound soundSource = Array.Find(sounds, s => s.sound.Equals(sound));

        if(soundSource == null)
        {
            Debug.LogError("Sound " + sound + " not found!"); 
            return null;  
        }
            
        return soundSource.source;
    }
}
