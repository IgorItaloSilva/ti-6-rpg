using System;

public class MapaTiagoEvents {
    public event Action onCutScenePlayed;
    public void PlayCutScene(){
        if(onCutScenePlayed!=null){
            onCutScenePlayed();
        }
    }
}