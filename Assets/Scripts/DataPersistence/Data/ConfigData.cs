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
        volumeMaster = 0;
        volumeMusic = 0;
        volumeVFX = -19;
    }
    public ConfigData(float vM,float vMu,float vV)
    {
        volumeMaster = vM;
        volumeMusic = vMu;
        volumeVFX = vV;
    }
}
