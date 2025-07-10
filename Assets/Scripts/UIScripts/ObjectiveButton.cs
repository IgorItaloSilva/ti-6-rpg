using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectiveButton : MonoBehaviour
{
    ObjectiveSO objectiveSO;
    ObjectiveData objectiveData;
    [SerializeField]TextMeshProUGUI title;
    
    public void SetObjectiveSO(ObjectiveSO objectiveSO){
        this.objectiveSO=objectiveSO;
        title.text=objectiveSO.objectiveTitle;
    }
    public void UpdateObjectiveData(ObjectiveData objectiveData){
        this.objectiveData=objectiveData;
    }
    public void OnClicked(){
        ObjectiveUiManager.instance.SetSelectedQuestTexts(objectiveSO, objectiveData);
    }
}
