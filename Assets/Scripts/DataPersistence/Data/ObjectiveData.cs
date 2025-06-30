using System;
using UnityEngine;
[Serializable]
public class ObjectiveData
{
    public bool hasStarted;
    public bool hasFinished;
    public string stringData;
    public string displayCompletedMessage;
    public bool alreadyCheckedProgressAlreadyMade;

    public ObjectiveData()
    {
        hasStarted = false;
        hasFinished = false;
        stringData = "";
        displayCompletedMessage = "";
        alreadyCheckedProgressAlreadyMade = false;
    }
    public ObjectiveData(bool Started)
    {
        hasStarted = true;
        hasFinished = false;
        stringData = "";
        displayCompletedMessage = "";
        alreadyCheckedProgressAlreadyMade = true;
    }
}
