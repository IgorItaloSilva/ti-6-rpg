using System;

public class MapaTiagoEvents {
    public event Action onCutScenePlayed;
    public void PlayCutScene(){
        if(onCutScenePlayed!=null){
            onCutScenePlayed();
        }
    }
    public event Action<int> onCollectedItem;
    public void CollectedItem(int itemID){
        if(onCollectedItem!=null){
            onCollectedItem(itemID);
        }
    }
    public event Action<int> OnActivateQuest;
    public void ActivateQuest(int id){
        if(OnActivateQuest!=null){
            OnActivateQuest(id);
        }
    }
    public event Action<int> OnDeactivateQuest;
    public void DeactivateQuest(int id){
        if(OnDeactivateQuest!=null){
            OnDeactivateQuest(id);
        }
    }
}