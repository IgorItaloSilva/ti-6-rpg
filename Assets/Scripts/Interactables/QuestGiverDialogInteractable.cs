using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuestGiverDialogInteractable : DialogueInteractable
{
    [SerializeField]TextMeshProUGUI textMeshProUGUI;
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
        //se inscriver em evento para saber que completou a quest

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        //se desinscriver em evento para saber que aceitou a quest
        //se desinscriver em evento para saber que completou a quest
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
