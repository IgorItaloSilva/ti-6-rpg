using UnityEngine;

public class LevelLoaderInteractable : Interactable{
    [SerializeField]string levelToGo;
    [SerializeField]GameObject vfx;
    [SerializeField]Vector3 startingPosNextLevel;
    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player")){
            if(Active&&CanInteract){
                GameManager.instance.ChangeLevel(levelToGo,startingPosNextLevel);
            }
            Activate();
        }
    }
    protected override void Awake()
    {
        base.Awake();
        CanInteract = false;//redundante, mas seguro
        vfx.SetActive(false);
    }
    public void Activate(){
        CanInteract=true;
        Active=true;
        vfx.SetActive(true);
        Save();
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        if(Active)Activate();
    }
}
