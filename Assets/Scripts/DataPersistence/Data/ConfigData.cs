using System;
using UnityEngine;
[Serializable]
public class ConfigData
{
    public float volumeMaster;
    public float volumeMusic;
    public float volumeVFX;

    public ConfigData()
    {
        volumeMaster = -5;
        volumeMusic = -5;
        volumeVFX = -5;
    }
    public ConfigData(float vM,float vMu,float vV)
    {
        volumeMaster = vM;
        volumeMusic = vMu;
        volumeVFX = vV;
    }
}
