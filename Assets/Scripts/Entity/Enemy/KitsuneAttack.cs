using System.Collections;
using UnityEngine;

public class KitsuneAttack : MonoBehaviour, IEnemyAction
{
    // Variável de controle
    private Coroutine coroutine;

    public void StartAction()
    {

        // Inicia a corrotina para esperar um tempo específico
        coroutine = StartCoroutine(WaitTime(2.0f, "Climax")); // 2 segundos de espera
        
    }

    public void UpdateAction()
    {
        // Implementar o que deve acontecer a cada frame
    }

    public void ExitAction()
    {
        // Implementar o que deve acontecer quando a ação é finalizada
    }


    private void Climax()
    {
        Debug.Log("Attack");
    }

    public IEnumerator WaitTime(float value, string method)
    {
        // Espera pelo tempo especificado
        yield return new WaitForSeconds(value);
        if(method != "")
        {
            Invoke(method, 0);
        }
    }
}

