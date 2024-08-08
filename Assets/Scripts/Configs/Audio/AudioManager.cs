using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



[DefaultExecutionOrder(-98)]
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    Slider masterSlider, musicSlider, effectSlider;
    const string exposedMaster = "MasterVolume";
    const string exposedMusic = "MusicVolume";
    const string exposedEffect = "EffectsVolume";
    [SerializeField] Button btnShake; // ------ Teste


    void Start()
    {
        if (!GameManager.instance.audioManager)
        {
            GameManager.instance.audioManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        
    }

    public void SetSliders(Slider[] sliders, Button btn)
    {
        masterSlider = sliders[0];
        musicSlider = sliders[1];
        effectSlider = sliders[2];
        btnShake = btn; // ----- Teste
        SetOnChange();
    }

    private void SetOnChange()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        effectSlider.onValueChanged.AddListener(SetEffectVolume);
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetEffectVolume(effectSlider.value);
        btnShake.onClick.AddListener(ShakeCam); //  ----- Teste
    }

    void SetMasterVolume(float value)
    {
        audioMixer.SetFloat(exposedMaster, Mathf.Log10(value) * 20);
    }

    void SetMusicVolume(float value)
    {
        audioMixer.SetFloat(exposedMusic, Mathf.Log10(value) * 20);
    }
    
    void SetEffectVolume(float value)
    {
        audioMixer.SetFloat(exposedEffect, Mathf.Log10(value) * 20);
    }

    void ShakeCam() // ----- Teste
    {
        GameManager.instance?.shakeEffect?.Invoke();
    }

}