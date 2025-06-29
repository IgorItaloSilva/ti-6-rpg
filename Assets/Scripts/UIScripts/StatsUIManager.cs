using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class StatsUIManager : MonoBehaviour
{
    public static StatsUIManager instance;
    [Header("Stats Base")]
    [SerializeField] TextMeshProUGUI con;
    [SerializeField] TextMeshProUGUI dex;
    [SerializeField] TextMeshProUGUI str;
    [SerializeField] TextMeshProUGUI inte;
    [SerializeField] TextMeshProUGUI potionLevel;
    [Header("Exp e Level")]
    [SerializeField] TextMeshProUGUI level;
    [SerializeField] TextMeshProUGUI nextLevelExp;
    [SerializeField] TextMeshProUGUI carriedExp;
    [Header("Advanced stats")]
    [SerializeField] TextMeshProUGUI lifeInfo;
    [SerializeField] TextMeshProUGUI manaInfo;
    [SerializeField] TextMeshProUGUI lightAttackDamage;
    [SerializeField] TextMeshProUGUI heavyAttackDamage;
    [SerializeField] TextMeshProUGUI magicAttackDamage;
    [SerializeField] TextMeshProUGUI potionHeal;
    [Header("Coisas Level Up")]
    [SerializeField] GameObject levelUpStuff;
    [SerializeField] GameObject applyButton;
    [SerializeField] TextMeshProUGUI pointsToSpend;
    [Header("Cor dos Textos")]
    [SerializeField] Color textColor = Color.white; //ESTÁ AQUI CASO A GENTE QUEIRA MUDAR ELAS DEPOIS
    public bool isSimulating { get; private set; }
    public int levelUpPoints;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("Já tinhamos um statsUIManager, então me destrui");
            Destroy(gameObject);
        }
    }
    void OnEnable()
    {//REMOVIDO PARA USAR SINGLETON
        GameEventsManager.instance.playerEvents.onInformLevelUpPoints += AjustLevelUpPoints;
        //GameEventsManager.instance.uiEvents.onReviceBaseStatsInfo+=ReciveBaseStatsInfo;
        //GameEventsManager.instance.uiEvents.onReviceExpStatsInfo+=ReciveExpStatsInfo;
        //GameEventsManager.instance.uiEvents.onReviceAdvancedStatsInfo+=ReciveAdvancedStatsInfo;
        //GameEventsManager.instance.uiEvents.onSimulateChangeBaseValue+=SimulateChangeBaseStat;
        //GameEventsManager.instance.uiEvents.onSimulateChangeAdvancedValue+=SimulateChangeAdvancedValue;
        //GameEventsManager.instance.uiEvents.onReciveLevelUpInfo+=ReciveLevelUpInfo;
    }
    void OnDisable()
    {//REMOVIDO PARA USAR SINGLETON
        GameEventsManager.instance.playerEvents.onInformLevelUpPoints -= AjustLevelUpPoints;
        //GameEventsManager.instance.uiEvents.onReviceBaseStatsInfo-=ReciveBaseStatsInfo;
        //GameEventsManager.instance.uiEvents.onReviceExpStatsInfo-=ReciveExpStatsInfo;
        //GameEventsManager.instance.uiEvents.onReviceAdvancedStatsInfo-=ReciveAdvancedStatsInfo;
        //GameEventsManager.instance.uiEvents.onSimulateChangeBaseValue-=SimulateChangeBaseStat;
        //GameEventsManager.instance.uiEvents.onSimulateChangeAdvancedValue-=SimulateChangeAdvancedValue;
        //GameEventsManager.instance.uiEvents.onReciveLevelUpInfo-=ReciveLevelUpInfo;
    }
    void Start()
    {
        UpdateValues();
        //Setup Sliders
    }
    public void UpdateValues()
    {
        RequestAllStatsInfo();
    }

    public void ReciveBaseStatsInfo(int con, int dex, int str, int inte, int potionLevel)
    {
        this.con.text = con.ToString();
        this.str.text = str.ToString();
        this.dex.text = dex.ToString();
        this.inte.text = inte.ToString();
        this.potionLevel.text = potionLevel.ToString();
    }
    public void ReciveExpStatsInfo(int level, int carriedExp, int expToNextLevel)
    {
        this.level.text = level.ToString();
        this.carriedExp.text = carriedExp.ToString();
        this.nextLevelExp.text = expToNextLevel.ToString();
    }
    public void ReciveAdvancedStatsInfo(float currentLife, float maxLife,
                                        float magicDamage, float lightAttackDamage, float heavyAtackDamage, float lifeToheal)
    {
        lifeInfo.text = currentLife.ToString("F0") + "/" + maxLife.ToString("F0");
        magicAttackDamage.text = magicDamage.ToString("F0");
        this.lightAttackDamage.text = lightAttackDamage.ToString("F0");
        this.heavyAttackDamage.text = heavyAtackDamage.ToString("F0");
        potionHeal.text = lifeToheal.ToString("F0");

    }
    public void ReciveAdvancedManaInfo(float currentMana,float maxMana)
    {
        manaInfo.text = currentMana.ToString("F0") + "/" + maxMana.ToString("F0");
    }
    public void ReciveLevelUpInfo(int pointsToSpend, bool isNearCampfire)
    {
        if (isNearCampfire)
        {
            levelUpStuff.SetActive(true);
            this.pointsToSpend.text = pointsToSpend.ToString();
            if (pointsToSpend > 0)
            {
                this.pointsToSpend.color = Color.green;
            }
            else
            {
                this.pointsToSpend.color = Color.white;
            }
        }
        else
        {
            levelUpStuff.SetActive(false);
        }
    }
    void RequestAllStatsInfo()
    {
        GameEventsManager.instance.uiEvents.RequestBaseStatsInfo();
        GameEventsManager.instance.uiEvents.RequestExpStatsInfo();
        GameEventsManager.instance.uiEvents.RequestAdvancedStatsInfo();
        GameEventsManager.instance.uiEvents.RequestLevelUpInfo();
        PlayerStateMachine.Instance.GetAdvancedManaInfo();
    }
    //COISAS DO LEVEL UP
    public void SimulateChangeBaseValue(int id, int newValue, bool isDifferent)
    {//Chamado pelo PlayerStats
        Color aux = isDifferent ? Color.green : textColor;
        switch (id)
        {
            case 0:
                this.con.text = newValue.ToString();
                this.con.color = aux;
                break;
            case 1:
                this.dex.text = newValue.ToString();
                this.dex.color = aux;
                break;
            case 2:
                this.str.text = newValue.ToString();
                this.str.color = aux;
                break;
            case 3:
                this.inte.text = newValue.ToString();
                this.inte.color = aux;
                break;
            case 4:
                potionLevel.text = newValue.ToString();
                potionLevel.color = aux;
                break;
        }
    }
    public void SimulateChangeAdvancedValue(int hardcodedId, float currentLifeOrMana, float value, bool isDifferent)
    {//Chamado pelo PlayerStats
        Color aux = isDifferent ? Color.green : textColor;
        switch (hardcodedId)
        {
            case 0:
                lifeInfo.text = currentLifeOrMana.ToString("F0") + "/" + value.ToString("F0");
                lifeInfo.color = aux;
                break;
            case 1:
                manaInfo.text = currentLifeOrMana.ToString("F0") + "/" + value.ToString("F0");
                manaInfo.color = aux;
                break;
            case 2:
                this.lightAttackDamage.text = value.ToString("F0");
                this.lightAttackDamage.color = aux;
                break;
            case 3:
                this.heavyAttackDamage.text = value.ToString("F0");
                this.heavyAttackDamage.color = aux;
                break;
            case 4:
                magicAttackDamage.text = value.ToString("F0");
                magicAttackDamage.color = aux;
                break;
            case 5:
                potionHeal.text = value.ToString("F0");
                potionHeal.color = aux;
                break;
        }
    }
    public void IncreaseStatusButtonPressed(int statusId)
    {//Chamado pelo Botão da UI
        if (levelUpPoints > 0)
        {
            isSimulating = true;
            applyButton.SetActive(true);
            GameEventsManager.instance.uiEvents.ChangeStatusButtonPressed(statusId, true);
        }
    }
    public void DecreaseStatusButtonPressed(int statusId)
    {//Chamado pelo Botão da UI
        if (isSimulating)
        {
            /* isSimulating = true;
            applyButton.SetActive(true); */
            GameEventsManager.instance.uiEvents.ChangeStatusButtonPressed(statusId, false);
        }
    }
    public void ConfirmLevelUp()
    {//Chamado pelo Botão da UI
        isSimulating = false;
        applyButton.SetActive(false);
        ResetColors();
        GameEventsManager.instance.uiEvents.ConfirmLevelUp();
    }
    public void CancelSimulation()
    {//Chamado ao sair da tela de Stats da Ui, se ainda estivermos simulando
        isSimulating = false;
        applyButton.SetActive(false);
        ResetColors();
        GameEventsManager.instance.uiEvents.DiscardLevelUp();
    }
    public void ColorAtributes(int[] runeBuffAmount)
    {
        for (int i = 0; i < runeBuffAmount.Length; i++)
        {
            PaintAtributeText(i, runeBuffAmount[i]);
        }
    }
    public void PaintAtributeText(int Id, int buffAmount)
    {
        Color cor = Color.white;
        if (buffAmount < 0) cor = Color.red;
        if (buffAmount > 0) cor = Color.green;
        switch (Id)
        {
            case 0:
                con.color = cor;
                lifeInfo.color = cor;
                break;
            case 1:
                dex.color = cor;
                lightAttackDamage.color = cor;
                break;
            case 2:
                str.color = cor;
                heavyAttackDamage.color = cor;
                break;
            case 3:
                inte.color = cor;
                manaInfo.color = cor;
                magicAttackDamage.color = cor;
                break;
            case 4:
                potionLevel.color = cor;
                potionHeal.color = cor;
                break;
        }
    }
    void ResetColors()
    {
        lifeInfo.color = textColor;
        manaInfo.color = textColor;
        magicAttackDamage.color = textColor;
        lightAttackDamage.color = textColor;
        heavyAttackDamage.color = textColor;
        con.color = textColor;
        dex.color = textColor;
        str.color = textColor;
        inte.color = textColor;
        potionLevel.color = textColor;
        potionHeal.color = textColor;
    }
    void AjustLevelUpPoints(int levelUpPoints)
    {
        this.levelUpPoints = levelUpPoints;
    }
}
