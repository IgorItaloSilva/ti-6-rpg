using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SaveSlot : MonoBehaviour
{
    [Header("ProfileId")]
    [SerializeField] private string profileId = "";
    [Header("Content")]
    [SerializeField]private GameObject noDataContent;
    [SerializeField]private GameObject hasDataContent;
    [SerializeField]private TextMeshProUGUI saveSlotName;
    [SerializeField]private TextMeshProUGUI currentLevelName;
    [SerializeField]private TextMeshProUGUI timeText;
    private string levelName;
    private Button saveSlotButton;
    [Header("Delete Save Button")]
    [SerializeField]private Button deleteButton;
    public bool hasData {get;private set;} = false;

    public void Awake(){
        saveSlotButton = this.GetComponent<Button>();
    }
    public void SetData(GameData data){
        if(data==null){
            hasData=false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            deleteButton.gameObject.SetActive(false);
        }
        else
        {
            hasData=true;
            hasDataContent.SetActive(true);
            noDataContent.SetActive(false);
            deleteButton.gameObject.SetActive(true);

            saveSlotName.text = "Save "+ profileId;
            DateTime time = DateTime.FromBinary(data.lastUpdated); 
            timeText.text = time.ToString();
            currentLevelName.text=data.currentLevel;
            levelName=data.currentLevel;
        }
    }
    public string GetProfileId(){
        return this.profileId;
    }
    public string GetLevelName(){
        return this.levelName;
    }
    public void SetInteractable(bool interactable){
        saveSlotButton.interactable = interactable;
        saveSlotButton.GetComponent<EventTrigger>().enabled=false;
    }
}
