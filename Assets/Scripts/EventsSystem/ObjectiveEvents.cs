using System;
using UnityEngine;

public class ObjectiveEvents
{
    public event Action<string> OnProgessMade;
    public void ProgressMade(string id){
        if(OnProgessMade!=null){
            OnProgessMade(id);
        }
    } 
    public event Action<string> OnObjectiveStarted;
    public void StartObjective(string id){
        if(OnObjectiveStarted!=null){
            OnObjectiveStarted(id);
        }
    } 
    public event Action<string> OnObjectiveCompleted;
    public void CompleteObjective(string id){
        if(OnObjectiveCompleted!=null){
            OnObjectiveCompleted(id);
        }
    } 
}
