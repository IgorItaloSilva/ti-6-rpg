using UnityEngine;
using System;

public class AudioPlayer : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource;
    public static AudioPlayer instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("MainTheme");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name, AudioSource source = null)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX Not Found");
        }
        else if(s.source == null)
        {
            musicSource.PlayOneShot(s.clip);
        }
        else
        {
            s.source.PlayOneShot(s.clip);
        }
    }
}
