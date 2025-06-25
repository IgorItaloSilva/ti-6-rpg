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
    public ObjectiveUiManager objectiveUiManager;
    public DialogueManager dialogManager;
    [Header("Coisas do Save VFX ")]
    [SerializeField] float saveIconTotalTime;
    [SerializeField] private Image saveIcon;
    [Header("Coisas das barras de Stats ")]
    [SerializeField] private PlayerHealthBar playerhealthBar;
    [Header("Coisas de Exp ")]
    [SerializeField] private TextMeshProUGUI carriedExpText;
    [SerializeField] private GameObject gainedExpTextGO;

    [Header("Referencias Internas")]
    [SerializeField] private GameObject painelPause;
    [SerializeField] private GameObject hideblePausePartUI;
    [SerializeField] private GameObject painelStats;
    [SerializeField] private GameObject painelDeath;
    [SerializeField] private GameObject painelDialog;
    [SerializeField] private GameObject painelTutorial;
    [SerializeField] private GameObject painelPopupTutorial;
    [SerializeField] private GameObject painelQuest;
    [SerializeField] private GameObject painelConfig;
    [SerializeField] private GameObject painelWeapon;
    [SerializeField] private GameObject buttonMainMenu;
    [SerializeField] private GameObject buttonTutorial;
    [SerializeField] Image backgroundTextOptions;
    [SerializeField] Image backgroundTextStats;
    [SerializeField] Image backgroundTextKatana;
    [SerializeField] Image backgroundTextSkillTree;
    [SerializeField] Image backgroundTextQuest;
    [SerializeField] Image backgroundTextTutorial;
    [SerializeField] Color unselectedColor;
    [SerializeField] Color selectedColor;
    [Header("Coisas do VFX de You Died ")]
    [SerializeField] private GameObject youDiedVFXParent;
    [SerializeField] private GameObject youDiedVFXBackgroundGO;
    [SerializeField] private GameObject youDiedVFXTextGO;
    [Header("Coisas da notification box")]
    [SerializeField] private GameObject notificationBox;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private int iterationSteps;
    [SerializeField] private float totalTime;
    [Header("Coisas da barra de vida do boss")]
    [SerializeField] HealthBar bossHealthBar;
    [SerializeField] TextMeshProUGUI bossName;
    [SerializeField] GameObject bossHPBarAndName;
    [Header("Coisas poção")]
    [SerializeField] TextMeshProUGUI potionsAmmountText;
    [Header("Coisas barra Objetivo")]
    [SerializeField] TextMeshProUGUI objectiveTitle;
    [SerializeField] TextMeshProUGUI objectiveText;
    [Header("Coisas Tutorial Popup")]
    [SerializeField] GameObject tutorialLayoutGroup;
    [SerializeField] HorizontalLayoutGroup tutorialHorizontalLayoutGroup;
    [SerializeField] TextMeshProUGUI tutorialTitle;
    [SerializeField] GameObject tutorialTextPrefab;
    [SerializeField] GameObject tutorialImagePlusTextPrefab;
    [SerializeField] GameObject tutorialImagePrefab;
    //coisas do vfx ganahr exp
    int carriedExp;
    int gainedExp;
    private Coroutine gainExpCouroutine;
    bool isGainExpCouroutineRunning;
    bool isGainedExpTextActive;
    private TextMeshProUGUI gainedExpText;
    //Coisas da Ui de morte
    private Text youDiedVFXText;
    private Image youDiedVFXImage;
    public bool isNearCampfire;
    bool toggleConfig = false;
    private const float transparencyRatioYouDiedVfx = 0.05f;
    private const float transparencyRatioYouDiedBackgroundVfx = 0.1f;
    private const float secondsBeforeYouDiedVfxAppears = 2f;
    private const float tranparacyTimeRatioDivideByTen = 0.05f;

    bool playerIsDead;//VARIAVEL DE CONTROLE PRO JOGADOR N CONSEGUIR ABRIR O MENU MORTO
    private UIScreens currentUIScreen;
    public enum UIScreens
    {
        Closed = -1,
        MainPause = 0,
        Stats,
        SkillTree,
        Weapon,
        System,
        Death,
        Dialog,
        Tutorial,
        QuestLog,
        TutorialPopup
    }


    void OnEnable()
    {
        GameEventsManager.instance.uiEvents.onLifeChange += UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame += FeedBackSave;
        GameEventsManager.instance.playerEvents.onPlayerDied += PlayerDied;
        GameEventsManager.instance.uiEvents.OnDialogOpened += OpenDialogPanel;
        GameEventsManager.instance.playerEvents.onPlayerGainExp += PlayExpGain;

    }

    void OnDisable()
    {
        
        GameEventsManager.instance.uiEvents.onLifeChange -= UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame -= FeedBackSave;
        GameEventsManager.instance.playerEvents.onPlayerDied -= PlayerDied;
        GameEventsManager.instance.uiEvents.OnDialogOpened -= OpenDialogPanel;
        GameEventsManager.instance.playerEvents.onPlayerGainExp -= PlayExpGain;
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
        if (!skillTreeUIManager)
        {
            Debug.LogWarning("o skillTreeUIManager está vazio");
            skillTreeUIManager = GetComponent<SkillTreeUIManager>();
        }
        if (!statsUIManager)
        {
            Debug.LogWarning("o statsUiManager está vazio");
            statsUIManager = GetComponent<StatsUIManager>();
        }
        currentUIScreen = UIScreens.Closed;
        runesUiManager = RunesUiManager.instance;
        runesUiManager.Setup();
        objectiveUiManager = ObjectiveUiManager.instance;
        dialogManager = DialogueManager.instance;
        AjustUiOnStart();
        gainedExpText = gainedExpTextGO.GetComponent<TextMeshProUGUI>();
        youDiedVFXText = youDiedVFXTextGO.GetComponent<Text>();
        youDiedVFXImage = youDiedVFXBackgroundGO.GetComponentInChildren<Image>();
        if (GameManager.instance.shouldLoadTutorial)
        {
            SwitchToScreen((int)UIScreens.Tutorial);
            GameManager.instance.PauseGameAndUnlockCursor();
        }
        RequestStartingInfo();
        //youDiedVFXText.color = new Color32(255,0,0,0);
    }
    void RequestStartingInfo()
    {
        RequestHealthBarInfo();
        RequestExpInfo();
        RequestPotionInfo();
    }
    // Update is called once per frame
    void Update()
    {
        if (playerIsDead) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (currentUIScreen == UIScreens.Closed)
            {
                SwitchToScreen((int)UIScreens.MainPause);
                if (isNearCampfire) SwitchToScreen((int)UIScreens.Stats);
            }
            else
            {
                if (toggleConfig)
                    ToggleConfig();
                else
                {
                    //n conferimos o currentUiScreen == chatting pq n precisa, o dialog manager já tem o ischatting
                    if (dialogManager.isChatting)
                    {
                        if (!dialogManager.CurrentSpeech.needsAnswer)
                        {
                            dialogManager.EndDialogue();
                        }
                    }
                    else
                        SwitchToScreen((int)UIScreens.Closed);
                }
            }
        }
        if (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentUIScreen == UIScreens.Dialog)
            {
                if (dialogManager.isChatting)
                {
                    dialogManager.AdvanceDialog();
                }
            }
            if (currentUIScreen == UIScreens.TutorialPopup)
            {
                SwitchToScreen((int)UIScreens.Closed);
            }
        }
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            PlayNotification("teste");

        }

        /* if(Mouse.current.leftButton.wasPressedThisFrame){
            Debug.Log($"pos do mouse = {Mouse.current.position.ReadValue()}");
        } */
    }
    private void RequestHealthBarInfo()
    {//deppois mudar pra mandar a mana tambem
        GameEventsManager.instance.uiEvents.RequestPlayerHealthInfo();
    }
    private void RequestExpInfo()
    {
        GameEventsManager.instance.uiEvents.RequestExpInfo();
    }
    private void RequestPotionInfo()
    {
        GameEventsManager.instance.uiEvents.RequestPotionAmmountInfo();
    }


    public void UpdateHealth(float vidaAtual, bool wasCrit)
    {
        if (playerhealthBar != null)
        {
            playerhealthBar.SetValue(vidaAtual, wasCrit);
        }
    }
    public void UpdateSliders(int id, float maxValue)
    {
        switch (id)
        {
            case 0:
                if (playerhealthBar != null)
                {
                    playerhealthBar.SettupBarMax(maxValue);
                }

                break;
            default: return;
        }
    }
    public void DisplayPotionAmmount(int nPotions)
    {
        potionsAmmountText.text = nPotions.ToString();
    }
    private void FeedBackSave()
    {//OK
        if (saveIcon != null)
        {
            saveIcon.gameObject.SetActive(true);
            StartCoroutine(SpinSaveIcon());
        }
    }
    public void PlayNotification(string text)
    {
        notificationBox.SetActive(true);
        notificationText.text = text;
        StartCoroutine("PlayNotificationVFX");
    }
    public void QuitGame()
    {
        GameManager.instance.ExitGame(true);
    }
    public void ReturnMainMenu()
    {
        DataPersistenceManager.instance?.SaveGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    private void AjustUiOnStart()
    {
        if (!painelPause)
        {
            Debug.LogWarning("O nosso uiManager não tem referencia ao menu de pause");
        }
        else
        {
            painelPause.SetActive(false);
            currentUIScreen = UIScreens.Closed;
        }
        if (notificationBox == null)
        {
            Debug.LogWarning("Não temos a caixa de notificação");
        }
        else
        {
            notificationBox.transform.localPosition = new Vector3(960, 0, 0);
            notificationBox.SetActive(false);
        }
        skillTreeUIManager?.AjustUiOnStart();
        painelStats.SetActive(false);
        painelDeath.SetActive(false);
        youDiedVFXParent.SetActive(false);
        painelDialog.SetActive(false);
        painelTutorial.SetActive(false);
        painelWeapon.SetActive(false);
        painelQuest.SetActive(false);
        bossHPBarAndName.SetActive(false);
        painelPopupTutorial.SetActive(false);
        painelConfig.SetActive(false);
        toggleConfig = false;
    }
    public void PlayerDied()
    {
        playerIsDead = true;
        StartCoroutine("PlayYouDiedAnimation");
    }
    #region Exp
    void OpenDialogPanel()
    {
        SwitchToScreen((int)UIScreens.Dialog);
    }
    public void DisplayExpAmmount(int expAmmount)
    {
        carriedExpText.text = expAmmount.ToString();
        carriedExp = expAmmount;
    }
    int targetAmmount;
    void PlayExpGain(int quantidade)
    {
        //Debug.Log("Chamando a função playExpGain da UI");
        if (isGainExpCouroutineRunning)
        {
            StopCoroutine(gainExpCouroutine);
            if (isGainedExpTextActive)
            {
                gainedExp += quantidade;
            }
            else
            {
                gainedExp = quantidade;
            }
            targetAmmount += quantidade;
        }
        else
        {
            targetAmmount = carriedExp + quantidade;
            gainedExp = quantidade;
        }
        gainedExpText.text = gainedExp.ToString();
        gainedExpTextGO.SetActive(true);
        isGainedExpTextActive = true;
        gainExpCouroutine = StartCoroutine(PlayExpGainAnimation());
    }
    IEnumerator PlayExpGainAnimation()
    {
        //Debug.Log($"Quantidade foi definido como = {targetAmmount}");
        isGainExpCouroutineRunning = true;
        yield return new WaitForSeconds(1f);
        gainedExpTextGO.SetActive(false);
        isGainedExpTextActive = false;
        for (; carriedExp < targetAmmount;)
        {//por algum motivo desconhecido o while n tava funcionando, ent é um for só com a condição kkk
            carriedExp += 1;
            carriedExpText.text = carriedExp.ToString();
            yield return null;
        }
        gainedExp = 0;
        targetAmmount = 0;
        isGainExpCouroutineRunning = false;
    }
    #endregion
    IEnumerator SpinSaveIcon()
    {//OK
        float timerTotal = saveIconTotalTime;
        while (timerTotal > 0)
        {
            timerTotal -= Time.unscaledDeltaTime;
            saveIcon.rectTransform.Rotate(Vector3.forward, -5);
            yield return null;
        }
        saveIcon.gameObject.SetActive(false);
    }
    IEnumerator PlayYouDiedAnimation()
    {
        yield return new WaitForSecondsRealtime(secondsBeforeYouDiedVfxAppears);
        youDiedVFXParent.SetActive(true);
        byte youDiedTextColor = 0;
        float youDiedImageColor = 1;
        youDiedVFXText.color = new Color32(255, 0, 0, youDiedTextColor);
        youDiedVFXImage.color = new Color(0, 0, 0, youDiedImageColor);
        for (int i = 0; i < 20; i++)
        {
            youDiedTextColor += (byte)(transparencyRatioYouDiedVfx * 255);
            if (i % 4 == 0) youDiedTextColor += 3;//correção do erro de arredondamendo de float pra byte
            //Debug.Log(youDiedTextColor);
            youDiedVFXText.color = new Color32(255, 0, 0, youDiedTextColor);
            yield return new WaitForSecondsRealtime(tranparacyTimeRatioDivideByTen);
        }
        yield return new WaitForSecondsRealtime(2);
        for (int i = 10; i > 0; i--)
        {
            youDiedTextColor -= (byte)(transparencyRatioYouDiedVfx * 2 * 255);
            //Debug.Log(youDiedTextColor);
            youDiedVFXText.color = new Color32(255, 0, 0, youDiedTextColor);
            youDiedImageColor -= transparencyRatioYouDiedBackgroundVfx;
            youDiedVFXImage.color = new Color(0, 0, 0, youDiedImageColor);
            yield return new WaitForSecondsRealtime(tranparacyTimeRatioDivideByTen);
        }
        youDiedVFXParent.SetActive(false);
        SwitchToScreen((int)UIScreens.Death);
    }
    IEnumerator PlayNotificationVFX()
    {
        //valor x no local position, inicial = 960, valor final = 560
        int ratio = 400 / iterationSteps;
        float yCoor = 0;//notificationBox.transform.localPosition.y;
        for (int i = 0, x = 960; i <= iterationSteps; i++, x -= ratio)
        {
            notificationBox.transform.localPosition = new Vector3(x, yCoor, 0);
            yield return new WaitForSecondsRealtime(totalTime / iterationSteps);
        }
        yield return new WaitForSecondsRealtime(2);
        for (int i = 0, x = 560; i <= iterationSteps; i++, x += ratio)
        {
            notificationBox.transform.localPosition = new Vector3(x, yCoor, 0);
            yield return new WaitForSecondsRealtime(totalTime / iterationSteps);
        }
    }
    public void BossLifeSettup(float currentLife, float maxLife, string name)
    {
        bossHPBarAndName.SetActive(true);
        bossHealthBar.SettupBarMax(maxLife);
        bossHealthBar.SetValue(currentLife, false);
        bossName.text = name;
    }
    public void UpdateBossLife(float currentHp, bool wasCrit)
    {
        bossHealthBar.SetValue(currentHp, wasCrit);
    }
    public void HideBossLife()
    {
        bossHPBarAndName.SetActive(false);
    }
    public void ObjectiveUpdate(string title, string text)
    {
        objectiveTitle.text = title;
        objectiveText.text = text;
    }
    public void SwitchToScreen(int destinationUiScreen)
    {
        //Debug.Log($"Trocado Para a tela {(UIScreens)destinationUiScreen}");
        //desativa a tela atual
        switch (currentUIScreen)
        {
            case UIScreens.Closed: break;
            case UIScreens.MainPause:
                hideblePausePartUI.SetActive(false);
                backgroundTextOptions.color = unselectedColor;
                break;
            case UIScreens.Stats:
                if (statsUIManager.isSimulating) statsUIManager.CancelSimulation();
                painelStats.SetActive(false);
                backgroundTextStats.color = unselectedColor;
                break;
            case UIScreens.SkillTree:
                skillTreeUIManager?.AlternarPainelSkillTree();
                backgroundTextSkillTree.color = unselectedColor;
                break;
            case UIScreens.Weapon:
                RuneManager.instance?.ApplySelectedRunes();
                painelWeapon.SetActive(false);
                backgroundTextKatana.color = unselectedColor;
                break;
            case UIScreens.System: break;
            case UIScreens.Death:
                GameManager.instance.ReturnFromDeath();
                painelDeath.SetActive(false);
                playerIsDead = false;
                break;
            case UIScreens.Dialog:
                painelDialog.SetActive(false);
                break;
            case UIScreens.Tutorial:
                painelTutorial.SetActive(false);
                backgroundTextTutorial.color = unselectedColor;
                break;
            case UIScreens.QuestLog:
                painelQuest.SetActive(false);
                backgroundTextQuest.color = unselectedColor;
                break;
            case UIScreens.TutorialPopup:
                GameManager.instance?.PauseGameAndUnlockCursor();
                painelPopupTutorial.SetActive(false);
                break;
            default: Debug.LogWarning("A tela atual é indefinida"); break;
        }
        //ativa a tela de destino
        switch ((UIScreens)destinationUiScreen)
        {
            case UIScreens.Closed:
                GameManager.instance.UnpauseGameAndLockCursor();
                painelPause.SetActive(false);
                currentUIScreen = UIScreens.Closed;
                break;
            case UIScreens.MainPause:
                GameManager.instance.PauseGameAndUnlockCursor();
                painelPause.SetActive(true);
                hideblePausePartUI.SetActive(true);
                currentUIScreen = UIScreens.MainPause;
                backgroundTextOptions.color = selectedColor;
                break;
            case UIScreens.SkillTree:
                if (!skillTreeUIManager) Debug.LogWarning("tentamos trocar de tela sem ter referencia ao skill tree manager");
                skillTreeUIManager?.AlternarPainelSkillTree();
                currentUIScreen = UIScreens.SkillTree;
                backgroundTextSkillTree.color = selectedColor;
                break;
            case UIScreens.Stats:
                painelStats.SetActive(true);
                statsUIManager.UpdateValues();
                currentUIScreen = UIScreens.Stats;
                backgroundTextStats.color = selectedColor;
                break;
            case UIScreens.Weapon:
                painelWeapon.SetActive(true);
                runesUiManager.UpdateRunes();
                runesUiManager.DisableAllTexts();
                currentUIScreen = UIScreens.Weapon;
                backgroundTextKatana.color = selectedColor;
                break;
            case UIScreens.System:
                currentUIScreen = UIScreens.System;
                break;
            case UIScreens.Death:
                GameManager.instance.PauseGameAndUnlockCursor();
                painelDeath.SetActive(true);
                currentUIScreen = UIScreens.Death;
                break;
            case UIScreens.Dialog:
                GameManager.instance.PauseGameAndUnlockCursor();
                painelDialog.SetActive(true);
                currentUIScreen = UIScreens.Dialog;
                break;
            case UIScreens.Tutorial:
                painelTutorial.SetActive(true);
                currentUIScreen = UIScreens.Tutorial;
                backgroundTextTutorial.color = selectedColor;
                break;
            case UIScreens.QuestLog:
                painelQuest.SetActive(true);
                ObjectiveUiManager.instance?.WasOpened();
                currentUIScreen = UIScreens.QuestLog;
                backgroundTextQuest.color = selectedColor;
                break;
            case UIScreens.TutorialPopup:
                GameManager.instance.PauseGameAndUnlockCursor();
                painelPopupTutorial.SetActive(true);
                currentUIScreen = UIScreens.TutorialPopup;
                break;
            default: Debug.LogWarning("A tela destino é indefinida"); break;
        }
    }
    public void NearCampfire(bool isNear)
    {
        if (isNear)
        {
            isNearCampfire = true;
            buttonTutorial.SetActive(false);
            buttonMainMenu.SetActive(false);
        }
        else
        {
            isNearCampfire = false;
            buttonTutorial.SetActive(true);
            buttonMainMenu.SetActive(true);
        }
    }
    public void DisplayPopupTutorial(TutorialSO tutorialSo)
    {
        Debug.LogWarning("chamando o tutorial aqui");
        SwitchToScreen((int)UIScreens.TutorialPopup);
        tutorialTitle.text = tutorialSo.Title;
        int childCount = tutorialLayoutGroup.transform.childCount;
        Transform aux;
        for (int child = 0; child < childCount; child++)
        {
            aux = tutorialLayoutGroup.transform.GetChild(child);
            Destroy(aux.gameObject);
        }
        for (int i = 0; i < tutorialSo.Sprites.Length; i++)
        {
            GameObject ImagePlusTextGO = Instantiate(tutorialImagePlusTextPrefab, tutorialHorizontalLayoutGroup.transform, false);
            VerticalLayoutGroup verticalLayoutGroup = ImagePlusTextGO.GetComponentInChildren<VerticalLayoutGroup>();
            GameObject newImage = Instantiate(tutorialImagePrefab, verticalLayoutGroup.transform, false);
            newImage.GetComponent<Image>().sprite = tutorialSo.Sprites[i];
            GameObject newText = Instantiate(tutorialTextPrefab, verticalLayoutGroup.transform, false);
            newText.GetComponent<TextMeshProUGUI>().text = tutorialSo.Texts[i];
        }
    }
    public void HandleCooldowns(int special)
    {
        //fazer toda a logica de cooldown e ai avisa o playerStateMachine
        PlayerStateMachine.Instance.EndSpecialCooldown(special);
    }
    public void ToggleConfig()
    {
        if (toggleConfig)
        {
            toggleConfig = false;
            painelConfig.SetActive(false);
        }
        else
        {
            toggleConfig = true;
            painelConfig.SetActive(true);
        }
    }
}
