using System;
using UnityEngine;

public class ObjectiveEvents
{
    public event Action<string> OnProgressMade;
    public void ProgressMade(string id){
        if(OnProgressMade!=null){
            OnProgressMade(id);
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
