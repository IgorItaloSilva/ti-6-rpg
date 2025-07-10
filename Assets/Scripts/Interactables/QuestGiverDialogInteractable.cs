using UnityEngine;
using UnityEngine.InputSystem;

public class QuestGiverDialogInteractable : DialogueInteractable
{
    [Header("Dialogos variaveis")]
    [SerializeField] Dialogue dialoguePosAceitarQuest;
    [SerializeField] Dialogue dialoguePosCompletarQuest;
    [SerializeField] Dialogue dialogueRefusedQuest;
    Dialogue selectedDialog;
    [SerializeField] ObjectiveSO objectiveSO;
    [SerializeField] GameObject indicadorQuest;

    protected override void Awake()
    {
        base.Awake();
        ignoreSaveLoad = true;
        selectedDialog = dialogue;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        GameEventsManager.instance.objectiveEvents.OnObjectiveStarted += QuestAccepted;
        GameEventsManager.instance.objectiveEvents.OnObjectiveCompleted += QuestCompleted;
        GameEventsManager.instance.objectiveEvents.onObjectiveRefused += QuestRefused;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        GameEventsManager.instance.objectiveEvents.OnObjectiveStarted -= QuestAccepted;
        GameEventsManager.instance.objectiveEvents.OnObjectiveCompleted -= QuestCompleted;
        GameEventsManager.instance.objectiveEvents.onObjectiveRefused -= QuestRefused;
    }
    protected override void Interact(InputAction.CallbackContext context)
    {
        if (inRange && !PlayerStateMachine.Instance.IsLocked && !DialogueManager.instance.isChatting)
        {
            DialogueManager.instance.StartDialogue(selectedDialog);
        }
    }
    void QuestAccepted(string id)
    {
        if (id == objectiveSO.Id)
        {
            selectedDialog = dialoguePosAceitarQuest;
            indicadorQuest.SetActive(false);
        }
    }
    void QuestCompleted(string id)
    {
        if (id == objectiveSO.Id)
        {
            selectedDialog = dialoguePosCompletarQuest;
            indicadorQuest.SetActive(false);
        }
    }
    void QuestRefused(string id)
    {
        if (id == objectiveSO.Id)
        {
            selectedDialog = dialogueRefusedQuest;
            indicadorQuest.SetActive(false);
        }
    }
}
