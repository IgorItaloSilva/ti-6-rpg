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
    public RunesUiManager runesUiManager;
    [Header("Referencias Internas")]
    [SerializeField]private Slider lifeSlider;
    [SerializeField]private Image saveIcon;
    [SerializeField]private GameObject painelPause;
    [SerializeField]private GameObject hideblePausePartUI;
    [SerializeField]private GameObject painelStats;
    [SerializeField]private GameObject painelDeath;
    [SerializeField]private GameObject painelDialog;
    [SerializeField]private GameObject painelTutorial;
    [SerializeField]private GameObject painelWeapon;
    [Header("Coisas do VFX de You Died ")]
    [SerializeField]private GameObject youDiedVFXParent;
    [SerializeField]private GameObject youDiedVFXBackgroundGO;
    [SerializeField]private GameObject youDiedVFXTextGO;
    [Header("Coisas da notification box")]
    [SerializeField]private GameObject notificationBox;
    [SerializeField]private TextMeshProUGUI notificationText;
    [SerializeField]private int iterationSteps;
    [SerializeField]private float totalTime;
    [Header("Coisas da barra de vida do boss")]
    [SerializeField]Slider bossLife;
    [SerializeField]TextMeshProUGUI bossName;
    [SerializeField]GameObject bossBarraDeVida;
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
        Dialog,
        Tutorial
    }
    
    
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onUpdateSliders+=UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange+=UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame+=FeedBackSave;
        GameEventsManager.instance.playerEvents.onPlayerDied+=PlayerDied;
        GameEventsManager.instance.uiEvents.OnDialogOpened+=OpenDialogPanel;
        GameEventsManager.instance.uiEvents.OnNotificationPlayed+=PlayNotification;
    }

    void OnDisable()
    {
        GameEventsManager.instance.uiEvents.onUpdateSliders -= UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange -= UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame -= FeedBackSave;
        GameEventsManager.instance.playerEvents.onPlayerDied -= PlayerDied;
        GameEventsManager.instance.uiEvents.OnDialogOpened -= OpenDialogPanel;
       GameEventsManager.instance.uiEvents.OnNotificationPlayed-=PlayNotification;
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
        runesUiManager=RunesUiManager.instance;
        runesUiManager.Setup();
        AjustUiOnStart();
        youDiedVFXText = youDiedVFXTextGO.GetComponent<Text>();
        youDiedVFXImage = youDiedVFXBackgroundGO.GetComponentInChildren<Image>();
        if(GameManager.instance.shouldLoadTutorial){
            SwitchToScreen((int)UIScreens.Tutorial);
            GameManager.instance.PauseGameAndUnlockCursor();
        }
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
            if(currentUIScreen==UIScreens.Tutorial){
                SwitchToScreen((int)UIScreens.Closed);
            }
        }
        if(Keyboard.current.numpad1Key.wasPressedThisFrame){
            GameEventsManager.instance.uiEvents.DialogOpen();
        }
        if(Keyboard.current.pKey.wasPressedThisFrame){
            PlayNotification("teste");
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
    void PlayNotification(string text){
        notificationBox.SetActive(true);
        notificationText.text=text;
        StartCoroutine("PlayNotificationVFX");
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void ReturnMainMenu(){
        DataPersistenceManager.instance?.SaveGame();
        Time.timeScale=1f;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    private void AjustUiOnStart(){
        if(!painelPause){
            Debug.LogWarning("O nosso uiManager não tem referencia ao menu de pause");
        }
        else{
            painelPause.SetActive(false);
            currentUIScreen=UIScreens.Closed;
        }
        if(notificationBox==null){
            Debug.LogWarning("Não temos a caixa de notificação");
        }
        else{
            notificationBox.transform.localPosition=new Vector3(960,0,0);
            notificationBox.SetActive(false);
        }
        skillTreeUIManager?.AjustUiOnStart();
        painelStats.SetActive(false);
        painelDeath.SetActive(false);
        youDiedVFXParent.SetActive(false);
        painelDialog.SetActive(false);
        painelTutorial.SetActive(false);
        painelWeapon.SetActive(false);
        bossBarraDeVida.SetActive(false);
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
    IEnumerator PlayNotificationVFX(){
        //valor x no local position, inicial = 960, valor final = 560
        int ratio = 400/iterationSteps;
        float yCoor= 0;//notificationBox.transform.localPosition.y;
        for(int i=0,x=960;i<=iterationSteps;i++,x-=ratio){
            notificationBox.transform.localPosition=new Vector3(x,yCoor,0);
            yield return new WaitForSecondsRealtime(totalTime/iterationSteps);
        }
        yield return new WaitForSecondsRealtime(2);
        for(int i=0,x=560;i<=iterationSteps;i++,x+=ratio){
            notificationBox.transform.localPosition=new Vector3(x,yCoor,0);
            yield return new WaitForSecondsRealtime(totalTime/iterationSteps);
        }
    }
    public void BossLifeSettup(float currentLife,float maxLife,string name){
        bossBarraDeVida.SetActive(true);
        bossLife.maxValue=maxLife;
        bossLife.value=currentLife;
        bossName.text=name;
    }
    public void UpdateBossLife(float currentHp){
        bossLife.value=currentHp;
    }
    public void HideBossLife(){
        bossBarraDeVida.SetActive(false);
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
            case UIScreens.Weapon:
                RuneManager.instance?.ApplySelectedRunes();
                painelWeapon.SetActive(false);
            break;
            case UIScreens.System: break;
            case UIScreens.Death:
                GameManager.instance.ReturnFromDeath();
                painelDeath.SetActive(false);
            break;
            case UIScreens.Dialog:
                painelDialog.SetActive(false);
            break;
            case UIScreens.Tutorial:
                painelTutorial.SetActive(false);
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
                painelWeapon.SetActive(true);
                runesUiManager.UpdateRunes();
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
                painelDialog.SetActive(true);
                currentUIScreen=UIScreens.Dialog;
            break;
            case UIScreens.Tutorial:
                painelTutorial.SetActive(true);
                currentUIScreen=UIScreens.Tutorial;
            break;
            default: Debug.LogWarning("A tela destino é indefinida"); break;
        }
    }
    
}
