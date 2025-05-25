using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Configuracao : MonoBehaviour, IDataPersistence
{
    public static Configuracao instance;
    //Coisas Volume
    [Header("Coisas Audio")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider sliderMaster;
    [SerializeField] Slider sliderMusic;
    [SerializeField] Slider sliderVFX;
    const string exposedMaster = "MasterVolume";
    const string exposedMusic = "MusicVolume";
    const string exposedEffect = "EffectsVolume";
    ConfigData configData;
    float volumeMaster;
    float volumeMusic;
    float volumeVFX;
    //Coisas resolução
    [Header("Coisas Resolução")]
    [SerializeField] TMP_Dropdown resolutionsDropDown;
    [SerializeField] Toggle fullScreenToggle;
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    float currentRefreshRate;
    int currentResolutionIndex = 0;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }
    void Start()
    {
        SettupAudioValuesFromSavedData();
        resolutions = Screen.resolutions;
        fullScreenToggle.isOn = Screen.fullScreen;
        filteredResolutions = new List<Resolution>();
        currentRefreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
        resolutionsDropDown.ClearOptions();
        for (int i = 0; i < resolutions.Length; i++)
        {
            if ((float)resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionsDropDown.AddOptions(options);
        resolutionsDropDown.value = currentResolutionIndex;
        resolutionsDropDown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        currentResolutionIndex = resolutionIndex;
    }
    public void SetMasterVolume(float value)//Colocado manualmente pelo slider
    {
        volumeMaster = value;
        if (value <= -29)
        {
            audioMixer.SetFloat(exposedMaster, -80f);
        }
        else
        {
            audioMixer.SetFloat(exposedMaster, volumeMaster);
        }

    }

    public void SetMusicVolume(float value)//Colocado manualmente pelo slider
    {
        volumeMusic = value;
        if (value <= -29)
        {
            audioMixer.SetFloat(exposedMusic, -80f);
        }
        else
        {
            audioMixer.SetFloat(exposedMusic, volumeMusic);
        }
    }

    public void SetEffectVolume(float value)//Colocado manualmente pelo slider
    {
        volumeVFX = value;
        if (value <= -29)
        {
            audioMixer.SetFloat(exposedEffect, -80f);
        }
        else
        {
            audioMixer.SetFloat(exposedEffect, volumeVFX);
        }
    }
    public void LoadData(GameData gameData)
    {
        configData = gameData.configData;
    }

    public void SaveData(GameData gameData)
    {
        ConfigData newconfigData = new ConfigData(volumeMaster, volumeMusic, volumeVFX);
        gameData.configData = newconfigData;
    }
    private void SettupAudioValuesFromSavedData()
    {
        SetMasterVolume(configData.volumeMaster);
        if (sliderMaster != null) sliderMaster.value = volumeMaster;
        SetMusicVolume(configData.volumeMusic);
        if (sliderMusic != null) sliderMusic.value = volumeMusic;
        SetEffectVolume(configData.volumeVFX);
        if (sliderVFX != null) sliderVFX.value = volumeVFX;
    }
    public void ApplyConfig()
    {
        Resolution resolution = filteredResolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, fullScreenToggle.isOn);
    }
}
