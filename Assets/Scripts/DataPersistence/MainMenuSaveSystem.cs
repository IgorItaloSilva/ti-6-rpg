using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : Menu
{
    [Header("Menu Navegation")]
    [SerializeField] private SaveSlotMenu saveSlotMenu;
    [SerializeField] Button newGameButton;
    [SerializeField] Button continueGameButton;
    [SerializeField] Button loadGameButton;
    void Start(){
        ActivateButtonsDependingOnData();
    }
    public void NewGameButton(){
        saveSlotMenu.ActivateMenu(false);
        DeactivateMenu();
    }
    public void LoadGameButton(){
        saveSlotMenu.ActivateMenu(true);
        DeactivateMenu();
    }
    public void ContinueButton(){
        string levelToBeLoaded=DataPersistenceManager.instance.GetDataLevelName();
        Debug.Log($"o continue conseguiu {levelToBeLoaded}");
        if(levelToBeLoaded==""){
            Debug.Log("Como não tinha um level indo pro indice 1");
            SceneManager.LoadSceneAsync(1);
        }
        else{
            Debug.Log("Indo pro level " + levelToBeLoaded);
            SceneManager.LoadSceneAsync(levelToBeLoaded);
        }
        SceneManager.LoadSceneAsync("Hud",LoadSceneMode.Additive);
    }
    public void DeactivateMenu(){
        gameObject.SetActive(false);
    }
    public void ActivateMenu(){
        gameObject.SetActive(true);
        ActivateButtonsDependingOnData();
    }
    public void ActivateButtonsDependingOnData(){
        Debug.Log($"temos data? {DataPersistenceManager.instance.HasData()}");
        if(!DataPersistenceManager.instance.HasData()){
            continueGameButton.interactable = false;
            loadGameButton.interactable = false;
        }
    }
}
