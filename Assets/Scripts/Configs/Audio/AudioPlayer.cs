using UnityEngine;
using System;
using Random = System.Random;

public class AudioPlayer : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioClip[] footstepSounds;
    public AudioSource musicSource, footstepSource;
    public static AudioPlayer instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void Start()
    {        
        footstepSource = PlayerStateMachine.Instance.gameObject.transform.GetChild(5).GetComponent<AudioSource>();

        PlayMusic("MainTheme");
        PlaySFX("AmbientSound");
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
        else if(!s.source)
        {
            if(musicSource) musicSource.PlayOneShot(s.clip);
        }
        else
        {
            s.source.PlayOneShot(s.clip);
        }
    }
    
public void StopSFX(string name)
{
    Sound s = Array.Find(sfxSounds, x => x.name == name);

    if (s == null)
    {
        Debug.Log("SFX Not Found");
        return;
    }

    AudioSource playSource = s.source ?? musicSource;

    if (playSource.isPlaying)
    {
        playSource.Stop();
    }
}

    public void PlayFootstepSound()
    {
        footstepSource.PlayOneShot(footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Length)]);
    }
}
