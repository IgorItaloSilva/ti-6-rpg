using UnityEngine;

public class LightBeamRoxoInteractable : Interactable
{
    [SerializeField]GameObject vfxGO;
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        vfxGO.SetActive(Active);
    }
    protected override void Start()
    {
        base.Start();
        if(!Active)vfxGO.SetActive(false);
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player")){
            vfxGO.SetActive(false);
            Active=false;
            Save();
        }
    }
    public void Activate(){
        Active=true;
        vfxGO.SetActive(true);
        Save();
    }
}
