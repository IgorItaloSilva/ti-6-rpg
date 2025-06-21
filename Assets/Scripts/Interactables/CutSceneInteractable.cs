using UnityEngine;
using UnityEngine.Playables;

public class CutSceneInteractable : Interactable
{
    [SerializeField] PlayableDirectorSample playableDirectorSample;

    protected override void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            playableDirectorSample.PlayCutscene();
            AlreadyInterated = true;
            Save();   
            gameObject.SetActive(false);
        }
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if (AlreadyInterated) gameObject.SetActive(false);
    }

}
