using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

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
    [SerializeField]private GameObject hideblePausePartUI;
    [SerializeField]private GameObject painelStats;
    [SerializeField]private GameObject painelDeath;
    [SerializeField]private GameObject painelDialog;
    [Header("Coisas do VFX de You Died ")]
    [SerializeField]private GameObject youDiedVFXParent;
    [SerializeField]private GameObject youDiedVFXBackgroundGO;
    [SerializeField]private GameObject youDiedVFXTextGO;
    private Text youDiedVFXText;
    private Image youDiedVFXImage;
    private const float transparencyRatioYouDiedVfx = 0.05f;
    private const float transparencyRatioYouDiedBackgroundVfx = 0.1f;
    private const float secondsBeforeYouDiedVfxAppears = 2f;
    private const float tranparacyTimeRatioDivideByTen = 0.05f;
    private UIScreens currentUIScreen;
    public enum UIScreens{
        Closed = -1,
        MainPause = 0,
        Stats,
        SkillTree,
        Weapon,
        System,
        Death,
        Dialog
    }
    
    
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onUpdateSliders+=UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange+=UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame+=FeedBackSave;
        GameEventsManager.instance.playerEvents.onPlayerDied+=PlayerDied;
        GameEventsManager.instance.uiEvents.OnDialogOpened+=OpenDialogPanel;
    }

    void OnDisable()
    {
        GameEventsManager.instance.uiEvents.onUpdateSliders -= UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange -= UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame -= FeedBackSave;
        GameEventsManager.instance.playerEvents.onPlayerDied-=PlayerDied;
        GameEventsManager.instance.uiEvents.OnDialogOpened-=OpenDialogPanel;
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
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
        youDiedVFXText = youDiedVFXTextGO.GetComponent<Text>();
        youDiedVFXImage = youDiedVFXBackgroundGO.GetComponentInChildren<Image>();
        //youDiedVFXText.color = new Color32(255,0,0,0);
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
        if(Keyboard.current.enterKey.wasPressedThisFrame){
            PlayerDied();
        }
        if(Keyboard.current.eKey.wasPressedThisFrame){
            if(currentUIScreen==UIScreens.Dialog){
                SwitchToScreen((int)UIScreens.Closed);
            }
        }
        if(Keyboard.current.numpad1Key.wasPressedThisFrame){
            GameEventsManager.instance.uiEvents.DialogOpen();
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
                if (lifeSlider != null)
                {
                    lifeSlider.minValue = minValue;
                    lifeSlider.maxValue = maxValue;
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
    /* public void VoltarMainMenu(){
        //MUDAR PARA O GAME MANAGER DEPOIS. time scale e laod de cena não são responsabilidade da UI
        Time.timeScale=1f;
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    } */
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
        painelDeath.SetActive(false);
        youDiedVFXParent.SetActive(false);
        painelDialog
.SetActive(false);
    }
    void PlayerDied(){
        StartCoroutine("PlayYouDiedAnimation");
    }
    void OpenDialogPanel(){
        SwitchToScreen((int)UIScreens.Dialog);
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
    IEnumerator PlayYouDiedAnimation(){
        
        yield return new WaitForSecondsRealtime(secondsBeforeYouDiedVfxAppears);
        youDiedVFXParent.SetActive(true);
        byte youDiedTextColor = 0;
        float youDiedImageColor= 1;
        youDiedVFXText.color= new Color32(255,0,0,youDiedTextColor);
        youDiedVFXImage.color = new Color(0,0,0,youDiedImageColor);
        for(int i=0;i<20;i++){
            youDiedTextColor+=(byte)(transparencyRatioYouDiedVfx*255);
            if(i%4==0)youDiedTextColor+=3;//correção do erro de arredondamendo de float pra byte
            //Debug.Log(youDiedTextColor);
            youDiedVFXText.color= new Color32(255,0,0,youDiedTextColor);
            yield return new WaitForSecondsRealtime(tranparacyTimeRatioDivideByTen);
        }
        yield return new WaitForSecondsRealtime(2);
        for(int i=10;i>0;i--){
            youDiedTextColor-=(byte)(transparencyRatioYouDiedVfx*2*255);
            //Debug.Log(youDiedTextColor);
            youDiedVFXText.color= new Color32(255,0,0,youDiedTextColor);
            youDiedImageColor-=transparencyRatioYouDiedBackgroundVfx;
            youDiedVFXImage.color=new Color(0,0,0,youDiedImageColor);
            yield return new WaitForSecondsRealtime(tranparacyTimeRatioDivideByTen);
        }
        youDiedVFXParent.SetActive(false);
        SwitchToScreen((int)UIScreens.Death);

    }
    public void SwitchToScreen(int destinationUiScreen){
        Debug.Log($"Trocado Para a tela {(UIScreens)destinationUiScreen}");
        //desativa a tela atual
        switch(currentUIScreen){
            case UIScreens.Closed: break;
            case UIScreens.MainPause:
                hideblePausePartUI.SetActive(false);
            break;
            case UIScreens.Stats:
                if(statsUIManager.isSimulating)statsUIManager.CancelSimulation();
                painelStats.SetActive(false);
            break;
            case UIScreens.SkillTree:
                skillTreeUIManager?.AlternarPainelSkillTree();
            break;
            case UIScreens.Weapon: break;
            case UIScreens.System: break;
            case UIScreens.Death:
                GameManager.instance.ReturnFromDeath();
                painelDeath.SetActive(false);
            break;
            case UIScreens.Dialog:
                painelDialog
        .SetActive(false);
            break;
            default: Debug.LogWarning("A tela atual é indefinida"); break;
        }
        //ativa a tela de destino
        switch((UIScreens)destinationUiScreen){
            case UIScreens.Closed:
                GameManager.instance.UnpauseGameAndLockCursor();
                painelPause.SetActive(false);
                currentUIScreen=UIScreens.Closed;
            break;
            case UIScreens.MainPause:
                GameManager.instance.PauseGameAndUnlockCursor();
                painelPause.SetActive(true);
                hideblePausePartUI.SetActive(true);
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
            case UIScreens.Death:
                GameManager.instance.PauseGameAndUnlockCursor();
                painelDeath.SetActive(true);
                currentUIScreen=UIScreens.Death;
            break;
            case UIScreens.Dialog:
                GameManager.instance.PauseGameAndUnlockCursor();
                painelDialog
        .SetActive(true);
                currentUIScreen=UIScreens.Dialog;
            break;
            default: Debug.LogWarning("A tela destino é indefinida"); break;
        }
    }
    
}
