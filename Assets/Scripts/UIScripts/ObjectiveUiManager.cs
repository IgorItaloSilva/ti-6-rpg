using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObjectiveUiManager : MonoBehaviour
{
    public static ObjectiveUiManager instance;
    [Header("Referencias quest selecionadas")]
    [SerializeField]TextMeshProUGUI objectiveTitleText;
    [SerializeField]TextMeshProUGUI objectiveStartedText;
    [SerializeField]TextMeshProUGUI objectiveConcludedText;
    [SerializeField]TextMeshProUGUI objectiveDescriptionText;
    [Header("Prefab botão")]
    [SerializeField]GameObject prefabQuestButton;
    [SerializeField]RectTransform VerticalLayoutGroup;
    private Dictionary<string,ObjectiveButton> buttons;
    private string currentlyOpenButtonId = "";
    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(this);
        }
        buttons = new Dictionary<string,ObjectiveButton>();
    }
    public void CreateButton(ObjectiveSO objectiveSO, ObjectiveData objectiveData){
        if(buttons.ContainsKey(objectiveSO.Id))return;
        GameObject newButton = Instantiate(prefabQuestButton,VerticalLayoutGroup);
        ObjectiveButton objectiveButton = newButton.GetComponent<ObjectiveButton>();
        objectiveButton.SetObjectiveSO(objectiveSO);
        objectiveButton.UpdateObjectiveData(objectiveData);
        buttons.Add(objectiveSO.Id,objectiveButton);
    }
    public void UpdateData(string id, ObjectiveData objectiveData){
        if(buttons.ContainsKey(id)){
            buttons[id].UpdateObjectiveData(objectiveData);
            if(currentlyOpenButtonId==id){
                buttons[id].OnClicked();
            }
        }
        else{
            Debug.Log("Tentamos upar a data de um botão que não existe");
        }
    }
    public void WasOpened(){
        if(currentlyOpenButtonId==""){//não cliquei em nenhuma quest
            if(buttons.Count>0){//tem pelo menos 1 botão
                buttons[buttons.Keys.ToList()[0]].OnClicked();
                
            }
        }
    }
    public void SetSelectedQuestTexts(ObjectiveSO objectiveSO, ObjectiveData objectiveData){
        currentlyOpenButtonId=objectiveSO.Id;
        objectiveTitleText.text= objectiveSO.objectiveTitle;
        if(objectiveData.hasStarted){
            objectiveStartedText.gameObject.SetActive(true);
            objectiveStartedText.text = "Sim";
            objectiveStartedText.color = Color.green;
        }
        else{
            objectiveStartedText.gameObject.SetActive(true);
            objectiveStartedText.text = "Não";
            objectiveStartedText.color = Color.red;
        }
        if(objectiveData.hasFinished){
            objectiveConcludedText.gameObject.SetActive(true);
            objectiveConcludedText.text = "Sim";
            objectiveConcludedText.color = Color.green;
        }
        else{
            objectiveConcludedText.gameObject.SetActive(true);
            objectiveConcludedText.text = "Não";
            objectiveConcludedText.color = Color.red;
        }
        objectiveDescriptionText.text = objectiveData.displayCompletedMessage;
    }
}
