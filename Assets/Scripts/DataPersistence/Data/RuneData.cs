using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class RuneData 
{
    public List<string> collectedRunes;
    public List<string> equipedRunes;

    public RuneData(){
        collectedRunes = new List<string>();
        equipedRunes = new List<string>();
    }
}
