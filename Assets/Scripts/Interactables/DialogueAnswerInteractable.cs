using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueAnswerInteractable : DialogueInteractable
{
    EnemyDestroyDialogAnswer enemyDestroyDialogAnswer;
    Speech speechToBeChanged;
    EnemyBehaviour enemyBehaviour;
    Collider[] results = new Collider[2];
    LayerMask layerMask =1<<9;
    protected override void Awake()
    {
        //Debug.Log((int)layerMask);
        base.Awake();
        enemyDestroyDialogAnswer = gameObject.GetComponent<EnemyDestroyDialogAnswer>();
        if (enemyDestroyDialogAnswer == null) Debug.Log("Não consegui meu EnemyDestroyDialog");
        enemyBehaviour = transform.parent.GetComponent<EnemyBehaviour>();
        if (enemyBehaviour == null) Debug.Log("Não consegui meu EnemyDestroyDialog");
        enemyDestroyDialogAnswer.myEnemyBehaviour = enemyBehaviour;
        for (int i = 0; i < dialogue.dialogue.Count; i++)
        {
            if (dialogue.dialogue[i].needsAnswer)
            {
                Debug.Log("Achei o speech que precisa de resposta");
                speechToBeChanged = dialogue.dialogue[i];
            }
        }
    }
    protected override void Interact(InputAction.CallbackContext context)
    {
        if (inRange && !PlayerStateMachine.Instance.IsLocked && !DialogueManager.instance.isChatting)
        {
            speechToBeChanged.dialogAnswer = enemyDestroyDialogAnswer;
            DialogueManager.instance.StartDialogue(dialogue);
            AlreadyInterated = true;
            Save();
            if (!dialogue.repeatable) gameObject.SetActive(false);
        }
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        if (Active)
        {
            canvas.SetActive(true);
            inRange = true;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (Active)
        {
            inRange = false;
            canvas.SetActive(false);
        }
    }
    public void Activate()
    {
        Active = true;
        canvas.SetActive(true);
        int n= Physics.OverlapSphereNonAlloc(transform.position, sphereCollider.radius, results, layerMask) ;
        Debug.Log(n);
        if (n > 0)
        {
            for (int i = 0; i < n; i++)
            {
                Debug.Log(results[i].tag);
                if (results[i].CompareTag("Player"))
                {
                    inRange = true;
                }
            }
        }
    }
}
