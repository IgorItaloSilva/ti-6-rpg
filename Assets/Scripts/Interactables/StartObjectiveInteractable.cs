using UnityEngine;

public class StartObjectiveInteractable : Interactable
{
    [SerializeField]ObjectiveSO objectiveSO;

    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player")){
            ObjectiveManager.instance?.StartQuest(objectiveSO);
            AlreadyInterated=true;
            Save();
            gameObject.SetActive(false);
        }
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if(AlreadyInterated)gameObject.SetActive(false);
    }
}
