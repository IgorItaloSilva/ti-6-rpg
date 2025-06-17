using UnityEngine;

public class DialogChoiceAux : MonoBehaviour
{
    [SerializeField] DialogueAnswerInteractable dialogueAnswerInteractable;//aqui vai um dialogInteractable
    [SerializeField] GameObject nextLevelPortal;//infelizmente isso é um migue do caralho
    public void Activate()
    {
        if(nextLevelPortal!=null)nextLevelPortal.SetActive(true);
        dialogueAnswerInteractable.Activate();
    }
}
