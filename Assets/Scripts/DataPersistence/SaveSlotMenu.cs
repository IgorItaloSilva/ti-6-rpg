using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotMenu : Menu
{
    [Header("Menu Navegation")]
    [SerializeField] private MainMenu mainMenu;
    [Header("Menu buttons")]
    [SerializeField]Button backButton;
    [Header("Confirmation Popup Menu")]
    [SerializeField]private ConfirmationMenu confirmationPopupMenu;
    private SaveSlot[] saveSlots;
    private bool isLoadingGame = false;
    private string levelToBeLoaded = "";

    private void Awake(){
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }
    public void OnBackClick(){
        mainMenu.ActivateMenu();
        DeactivateMenu();
    }
    public void ActivateMenu(bool isLoadingGame){
        this.gameObject.SetActive(true);
        this.isLoadingGame=isLoadingGame;
        Dictionary<String,GameData> profilesGameData = DataPersistenceManager.instance.GetAllProfilesGameData();
        backButton.interactable=true;
        GameObject firstSelected = backButton.gameObject;
        //loop nos saves slots
        foreach( SaveSlot saveSlot in saveSlots){
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            Button button = saveSlot.GetComponent<Button>();
            if(button!=null){
                if(profileData == null && isLoadingGame){
                    button.interactable=false;
                    button.GetComponent<EventTrigger>().enabled=false;
                }
                else{
                    button.interactable=true;
                    button.GetComponent<EventTrigger>().enabled=true;
                    if(firstSelected.Equals(backButton.gameObject)){
                        firstSelected = saveSlot.gameObject;
                    }
                }
            }
        }
        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        SetFirstSelected(firstSelectedButton);
    }
    public void DeactivateMenu(){
        gameObject.SetActive(false);
    }
    public void OnSaveSlotClicled(SaveSlot saveSlot){
        DisableMenuButtons();
        if(isLoadingGame){
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            levelToBeLoaded = saveSlot.GetLevelName();
            SaveGameAndLoadScene();
        }
        else if(saveSlot.hasData){
            confirmationPopupMenu.ActivateMenu(
                "Começar um novo jogo aqui apagara esse save. Tem certeza que você quer deletar esse save?",
                //função sim
                ()=>{
                    DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
                    DataPersistenceManager.instance.NewGame();
                    SaveGameAndLoadScene();
                },
                ()=> {
                    this.ActivateMenu(isLoadingGame);
                }
            );
        }
        else{
            DataPersistenceManager.instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
            DataPersistenceManager.instance.NewGame();
            SaveGameAndLoadScene();
        }
    }
    public void SaveGameAndLoadScene(){
        DataPersistenceManager.instance.SaveGame();
        if(levelToBeLoaded==""){
            if(DataPersistenceManager.instance.showDebug)Debug.Log("Como não tinha um level indo pro indice 1");
            GameManager.instance.ChangeLevelFromMainMenu("KitsuneForest", Vector3.zero);
        }
        else{
            if(DataPersistenceManager.instance.showDebug)Debug.Log("Indo pro level " + levelToBeLoaded);
            Vector3 pos = levelToBeLoaded == "MageMap" ? new Vector3(-117.099998f, -5.9000001f, -19.5300007f) : Vector3.zero;
            GameManager.instance.ChangeLevelFromMainMenu(levelToBeLoaded, pos);
        }
        SceneManager.LoadSceneAsync("Hud",LoadSceneMode.Additive);
    }
    public void OnDeleteClick(SaveSlot saveSlot){

        DisableMenuButtons();
        confirmationPopupMenu.ActivateMenu(
            "Tem certeza que quer deletar esse save?",
            ()=>{
                DataPersistenceManager.instance.Delete(saveSlot.GetProfileId());
                ActivateMenu(isLoadingGame);
            },
            ()=>{
                ActivateMenu(isLoadingGame);
            }
        );
    }
    void DisableMenuButtons(){
        foreach (SaveSlot saveSlot in saveSlots){
            saveSlot.SetInteractable(false);
        }
        backButton.interactable=false;
    }

}
