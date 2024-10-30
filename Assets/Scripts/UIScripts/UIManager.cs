using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("Outros scripts de UI")]
    public SkillTreeUIManager skillTreeUIManager;
    public StatsUIManager statsUIManager;
    [Header("Referencias Internas")]
    [SerializeField]private Slider lifeSlider;
    [SerializeField]private Image saveIcon;
    [SerializeField]private GameObject painelPause;
    [SerializeField]private GameObject painelStats;
    [SerializeField]private GameObject hideblePartUI;
    private UIScreens currentUIScreen;
    public enum UIScreens{
        Closed = -1,
        MainPause = 0,
        Stats,
        SkillTree,
        Weapon,
        System
    }
    
    
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onUpdateSliders+=UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange+=UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame+=FeedBackSave;
    }
    void OnDisable(){
        GameEventsManager.instance.uiEvents.onUpdateSliders-=UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange-=UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame-=FeedBackSave;
    }
    void Start()
    {
        if(instance==null){
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        if(!skillTreeUIManager){
            Debug.Log("o skillTreeUIManager está vazio");
            skillTreeUIManager=GetComponent<SkillTreeUIManager>();
        }
        if(!statsUIManager){
            Debug.Log("o statsUiManager está vazio");
            statsUIManager=GetComponent<StatsUIManager>();
        }
        currentUIScreen = UIScreens.Closed;
        AjustUiOnStart();
    }
    

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame){
            if(currentUIScreen==UIScreens.Closed){
                SwitchToScreen((int)UIScreens.MainPause);
            }else 
            if(currentUIScreen == UIScreens.MainPause){
                SwitchToScreen((int)UIScreens.Closed);
            }
            else{
                SwitchToScreen((int)UIScreens.MainPause);
            }
        }
        /* if(Mouse.current.leftButton.wasPressedThisFrame){
            Debug.Log($"pos do mouse = {Mouse.current.position.ReadValue()}");
        } */
    }
    private void UpdateHealth(float vidaAtual){//MUDAR PARA UM MANAGER DE STATS DEPOIS
        if(lifeSlider!=null){
            lifeSlider.value=vidaAtual;
        }
    }
    private void UpdateSliders(int id,float minValue,float maxValue){////MUDAR PARA UM MANAGER DE STATS DEPOIS
        switch(id){
            case 0:
                if(lifeSlider!=null){
                    lifeSlider.minValue=minValue;
                    lifeSlider.maxValue=maxValue;
                }
            break;
            default: return;
        }
    }
    private void FeedBackSave(){//OK
        if(saveIcon!=null){
            saveIcon.gameObject.SetActive(true);
            StartCoroutine(SpinSaveIcon());
        }
    }
    public void QuitGame(){
        //MUDAR PARA O GAME MANAGER DEPOIS. FECHAR O JOGO NÃO É RESPONSABILIDADE DO UI MANAGER
        Application.Quit();
    }
    public void VoltarMainMenu(){
        //MUDAR PARA O GAME MANAGER DEPOIS. time scale e laod de cena não são responsabilidade da UI
        Time.timeScale=1f;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    IEnumerator SpinSaveIcon(){//OK
        float timerTotal=5f;
        while(timerTotal>0){
            timerTotal-=Time.unscaledDeltaTime;
            saveIcon.rectTransform.Rotate(Vector3.forward,-5);
            yield return null;
        }
        saveIcon.gameObject.SetActive(false);
    }
    private void UnpauseGame(){
        //Mudar isso quando começar a usar um game manager. Essa função deveria fazer só a parte da UI, e se pa nem existir
        //Mudar a time scale não é responsabilidade de um manager de UI
        Time.timeScale=1f;
        Cursor.lockState=CursorLockMode.Locked;
        Cursor.visible=false;
        painelPause.SetActive(false);
    }
    private void PauseGame(){
        //Mudar isso quando começar a usar um game manager. Essa função deveria fazer só a parte da UI, e se pa nem existir
        //Mudar a time scale não é responsabilidade de um manager de UI
        Time.timeScale=0f;
        Cursor.lockState=CursorLockMode.Confined;
        Cursor.visible=true;
        painelPause.SetActive(true);
        hideblePartUI.SetActive(true);
    }
    private void AjustUiOnStart(){
        if(!painelPause){
            Debug.LogWarning("O nosso uiManager não tem referencia ao menu de pause");
        }
        else{
            painelPause.SetActive(false);
            currentUIScreen=UIScreens.Closed;
        }
        skillTreeUIManager?.AjustUiOnStart();
        painelStats.SetActive(false);
    }
    public void SwitchToScreen(int destinationUiScreen){
        Debug.Log($"Trocado Para a tela {(UIScreens)destinationUiScreen}");
        //desativa a tela atual
        switch(currentUIScreen){
            case UIScreens.Closed: break;
            case UIScreens.MainPause:
                hideblePartUI.SetActive(false);
            break;
            case UIScreens.Stats:
                painelStats.SetActive(false);
            break;
            case UIScreens.SkillTree:
                skillTreeUIManager?.AlternarPainelSkillTree();
            break;
            case UIScreens.Weapon: break;
            case UIScreens.System: break;
            default: Debug.LogWarning("A tela atual é indefinida"); break;
        }
        //ativa a tela de destino
        switch((UIScreens)destinationUiScreen){
            case UIScreens.Closed:
                UnpauseGame();
                currentUIScreen=UIScreens.Closed;
            break;
            case UIScreens.MainPause:
                PauseGame();
                currentUIScreen=UIScreens.MainPause;
            break;
            case UIScreens.SkillTree:
            if(!skillTreeUIManager)Debug.Log("tentamos trocar de tela sem ter referencia ao skill tree manager");
                skillTreeUIManager?.AlternarPainelSkillTree();
                currentUIScreen=UIScreens.SkillTree; 
            break;
            case UIScreens.Stats:
                painelStats.SetActive(true);
                statsUIManager.UpdateValues();
                currentUIScreen=UIScreens.Stats;
            break;
            case UIScreens.Weapon: 
                currentUIScreen=UIScreens.Weapon;
            break;
            case UIScreens.System: 
                currentUIScreen=UIScreens.System;
            break;
            default: Debug.LogWarning("A tela destino é indefinida"); break;
        }
    }
    
}

