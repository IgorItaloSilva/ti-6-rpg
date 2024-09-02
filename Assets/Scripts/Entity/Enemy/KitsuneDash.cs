using System.Collections;
using UnityEngine;


public class KitsuneDash : MonoBehaviour, IEnemyAction
{


    // Variável de controle
    private Coroutine coroutine;


    public void ExitAction()
    {
        throw new System.NotImplementedException();
    }

    public void StartAction()
    {
        //StartCoroutine("WaitTime", 1f);
        Debug.Log("Dash");

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
