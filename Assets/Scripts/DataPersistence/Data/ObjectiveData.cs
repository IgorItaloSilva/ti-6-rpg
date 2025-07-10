using System;
using UnityEngine;
[Serializable]
public class ObjectiveData
{
    public bool hasStarted;
    public bool hasFinished;
    public string stringData;
    public string displayCompletedMessage;
    public bool questWasRefused;

    public ObjectiveData(bool started = false,bool refused = false)
    {
        hasStarted = started;
        hasFinished = false;
        stringData = "";
        displayCompletedMessage = "";
        questWasRefused = refused;
    }
    /* public ObjectiveData(bool Started)
    {
        hasStarted = true;
        hasFinished = false;
        stringData = "";
        displayCompletedMessage = "";
        questWasRefused = false;
    } */
}
