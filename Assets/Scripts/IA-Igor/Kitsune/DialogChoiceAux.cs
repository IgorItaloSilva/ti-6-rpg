using UnityEngine;

public class DialogChoiceAux : MonoBehaviour
{
    [SerializeField] GameObject dialogInteractableGO;//aqui vai um dialogInteractable
    [SerializeField] EnemyDestroyDialogAnswer enemyDestroyDialogAnswer;//precisa ser um go separado
    [SerializeField] GameObject nextLevelPortal;//infelizmente isso é um migue do caralho
    EnemyBehaviour enemyBehaviour;
    void Awake()
    {
        if (enemyDestroyDialogAnswer == null) Debug.Log("Não consegui meu EnemyDestroyDialog");
        enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
        DialogueInteractable di = dialogInteractableGO.GetComponent<DialogueInteractable>();
        if (di == null) Debug.Log("N consegui pegar o dialogInteractable");
        Dialogue d = di.dialogue;
        if (d == null) Debug.Log("Não consegui pegar o dialogue");
        for (int i = 0; i < d.dialogue.Count; i++)
        {
            if (d.dialogue[i].needsAnswer)
            {
                Debug.Log("Achei o speech que precisa de resposta");
                if (enemyDestroyDialogAnswer != null)
                {
                    enemyDestroyDialogAnswer.myEnemyBehaviour = enemyBehaviour;
                    d.dialogue[i].dialogAnswer = enemyDestroyDialogAnswer;
                }
            }
        }
    }
    public void Activate()
    {
        if(nextLevelPortal!=null)nextLevelPortal.SetActive(true);
        dialogInteractableGO?.SetActive(true);
    }
}
