using System;
using JetBrains.Annotations;
using UnityEngine;

public class ObjectiveEvents
{
    public event Action<string> OnProgressMade;
    public void ProgressMade(string id)
    {
        if (OnProgressMade != null)
        {
            OnProgressMade(id);
        }
    }
    //public event Action<string,int> OnProgressMadeWithExtraId;
    public void ProgressMadeWithExtraID(string id, int extraId)
    {
        if (OnProgressMade != null)
        {
            OnProgressMade(id);
        }
    }
    public event Action<string> OnObjectiveStarted;
    public void StartObjective(string id)
    {
        if (OnObjectiveStarted != null)
        {
            OnObjectiveStarted(id);
        }
    }
    public event Action<string> OnObjectiveCompleted;
    public void CompleteObjective(string id)
    {
        if (OnObjectiveCompleted != null)
        {
            OnObjectiveCompleted(id);
        }
    }
    public event Action<string> onReciveAlreadyMadeProgress;
    public void ReciveAlreadyMadeProgress(string id)
    {
        if (onReciveAlreadyMadeProgress != null)
        {
            onReciveAlreadyMadeProgress(id);
        }
    }
    //Não faz nada no objective manager, mas os dialogInteractables reagem a ele
    public event Action<string> onObjectiveRefused;
    public void RefuseQuest(string id)
    {
        if (onObjectiveRefused != null)
        {
            onObjectiveRefused(id);
        }
    }
}
