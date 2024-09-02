using System.Collections;
using UnityEngine;


public class EnemyBehaviour : MonoBehaviour, IEnemyBehave
{
    public IEnemyAction[] action;
    public IEnemyAction currentAction;
    int index = 0;


    public bool CheckDistance()
    {
        throw new System.NotImplementedException();
    }

    public void IsClose()
    {
        throw new System.NotImplementedException();
    }

    public void IsDistant()
    {
        throw new System.NotImplementedException();
    }

    public void ChangeAction()
    {
        currentAction = action[index];
        currentAction.StartAction();
        index++;
        if(index == action.Length)
            index = 0;
    }

    public IEnumerator WaitTime(float value, string method)
    {
        yield return new WaitForSeconds(value);
    }
}
