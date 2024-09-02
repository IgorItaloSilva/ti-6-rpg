using System.Collections;
using UnityEngine;


public class KitsuneIdle : MonoBehaviour, IEnemyAction
{
    public void ExitAction()
    {
        throw new System.NotImplementedException();
    }

    public void StartAction()
    {
        
        Debug.Log("Idle");
    }

    public void UpdateAction()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator WaitTime(float value, string method)
    {
        throw new System.NotImplementedException();
    }
}
