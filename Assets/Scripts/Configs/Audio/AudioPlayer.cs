using UnityEngine;
using System;
using System.Threading;

public class AudioPlayer : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioClip[] footstepSounds;
    public AudioSource musicSource, footstepSource;
    public static AudioPlayer instance;
    
    private float fadeDuration;
    private float startVolume;
    private float targetVolume;
    private void Awake()
    {
        if (instance == null)
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

    public void PlayMusic(string name, bool loop = true, bool crossfade = true)
    {
        musicSource.loop = loop;
        
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music Not Found");
        }
        else
        {
            if (musicSource.isPlaying && crossfade)
            {
                CrossfadeMusic(name);
            }
            else
            {
                musicSource.clip = s.clip;
                musicSource.Play();
            }
        }
    }
    
    private async void CrossfadeMusic(string newMusicName)
    {
        Sound newSound = Array.Find(musicSounds, x => x.name == newMusicName);
        if (newSound == null) return;

        fadeDuration = 1f; // Duration of the crossfade in seconds
        startVolume = musicSource.volume;
        targetVolume = 0f;

        // Fade out current music
        while (musicSource.volume > targetVolume)
        {
            musicSource.volume -= startVolume / (fadeDuration * 100);
            await System.Threading.Tasks.Task.Delay(10);
        }

        // Change the music clip
        musicSource.clip = newSound.clip;
        musicSource.Play();

        // Fade in new music
        if (!musicSource) return;
        
        while (musicSource.volume < startVolume)
        {
            musicSource.volume += startVolume / (fadeDuration * 100);
            await System.Threading.Tasks.Task.Delay(10);
        }
    }

    public void PlaySFX(string name, AudioSource source = null)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX Not Found");
        }
        else if (!s.source)
        {
            if (musicSource) musicSource.PlayOneShot(s.clip);
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