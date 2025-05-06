using System;
using UnityEngine;
[Serializable]
public class ObjectiveData
{
    public bool hasStarted;
    public bool hasFinished;
    public string stringData;

    public ObjectiveData(){
        hasStarted=false;
        hasFinished=false;
        stringData="";
    }
}
