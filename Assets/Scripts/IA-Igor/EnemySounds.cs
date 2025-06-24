using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "EnemySounds", menuName = "ScriptableObjects/EnemySounds")]
public class EnemySounds : ScriptableObject
{
    public List<AudioClip> damageSounds;
    public List<AudioClip> deathSounds;
    public List<AudioClip> attackSounds;
    private List<AudioClip> _listToPlay;

    public enum SoundType
    {
        Damage,
        Death,
        Attack
    }

    public void PlaySound(SoundType type, AudioSource source)
    {
        _listToPlay = type switch
        {
            SoundType.Damage => damageSounds,
            SoundType.Death => deathSounds,
            SoundType.Attack => attackSounds,
            _ => attackSounds
        };
        if (_listToPlay.Count > 0)
            source.PlayOneShot(_listToPlay[Random.Range(0, _listToPlay.Count)]);
        else
            Debug.LogWarning("Tried to play sound of type " + type + " but list is empty.");
    }
}