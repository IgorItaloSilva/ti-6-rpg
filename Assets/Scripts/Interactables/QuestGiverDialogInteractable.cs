using UnityEngine;
using UnityEngine.InputSystem;

public class QuestGiverDialogInteractable : DialogueInteractable
{
    [Header("Dialogos variaveis")]
    [SerializeField]Dialogue dialoguePosAceitarQuest;
    [SerializeField]Dialogue dialoguePosCompletarQuest;
    Dialogue selectedDialog;
    [SerializeField]ObjectiveSO objectiveSO;

    protected override void Awake()
    {
        base.Awake();
        ignoreSaveLoad=true;
        selectedDialog=dialogue;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        GameEventsManager.instance.objectiveEvents.OnObjectiveStarted+=QuestAccepted;
        GameEventsManager.instance.objectiveEvents.OnObjectiveCompleted+=QuestCompleted;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        GameEventsManager.instance.objectiveEvents.OnObjectiveStarted-=QuestAccepted;
        GameEventsManager.instance.objectiveEvents.OnObjectiveCompleted-=QuestCompleted;
    }
    protected override void Interact(InputAction.CallbackContext context){
        if(inRange && !PlayerStateMachine.Instance.IsLocked && !DialogueManager.instance.isChatting){
            DialogueManager.instance.StartDialogue(selectedDialog);
        }
    }
    void QuestAccepted(string id){
        if(id==objectiveSO.Id){
            selectedDialog=dialoguePosAceitarQuest;
        }
    }
    void QuestCompleted(string id){
        if(id==objectiveSO.Id){
            selectedDialog=dialoguePosCompletarQuest;
        }
    }
}
