using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInteractable : Interactable{
    [Header("COLCOAR DIALOGO(S) AQUI!")]
    [SerializeField]protected Dialogue dialogue;
    [SerializeField]protected GameObject canvas;
    bool didntAddInteract = false;
    protected override void Awake()
    {
    base.Awake();
        if (canvas == null)
        {
            canvas = GetComponentInChildren<Canvas>().gameObject;
        }
    }
    protected virtual void OnEnable(){
        if(!PlayerStateMachine.Instance)didntAddInteract=true;
        PlayerStateMachine.Instance?.AddActionToInteract(Interact);
    }
    protected virtual void OnDisable(){
        PlayerStateMachine.Instance?.RemoveActionFromInteract(Interact);
    }
    protected override void Start()
    {
        base.Start();
        if(didntAddInteract){
            PlayerStateMachine.Instance?.AddActionToInteract(Interact);
        }
        if(canvas.activeInHierarchy)canvas.SetActive(false);
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if(AlreadyInterated&&!dialogue.repeatable)gameObject.SetActive(false);
    }
    protected virtual void Interact(InputAction.CallbackContext context){
        if(inRange && !PlayerStateMachine.Instance.IsLocked && !DialogueManager.instance.isChatting){
            DialogueManager.instance.StartDialogue(dialogue);
            AlreadyInterated=true;
            Save();
            if(!dialogue.repeatable)gameObject.SetActive(false);
        }
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        canvas.SetActive(true);
        inRange=true;
    }
    void OnTriggerExit(Collider collider){
        inRange = false;
        canvas.SetActive(false);
    }
    /* void OnTriggerStay(Collider collider){
         Vector3 lookAt = new Vector3(collider.transform.position.x,canvas.transform.position.y,collider.transform.position.z);
            canvas.transform.LookAt(lookAt);
            canvas.transform.Rotate(new Vector3(0,180,0));
    } */
}
