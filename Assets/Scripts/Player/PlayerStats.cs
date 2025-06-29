using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerStats : MonoBehaviour, IDataPersistence, IDamagable
{
    /*  Essa classe foi pensada em ser a classe principal para lidar com os status do jogador,
        ela mantem vários atributos que serão usados em vários outros scripts, e é responsavel por eles.

        Atualemente ela tem poucas funcionalidades, e nem todos os atributos que ela provavelmente deveria ter,
        eles serão adicionados conforme eles forem implementados e nos tivermos uma ideia melhor de como eles vão
        funcionar

        POSSIVELMENTE ADICIONAR MANA E STAMINA AINDA
    */
    //Esses valores estão aqui para testes, depeois de definidos eles devem ser colocados no scriptableObjects
    [SerializeField] float vidaConsMod = 25;
    [SerializeField] float manaIntMod = 20;
    [SerializeField] float magicDamageMod = 3;
    [SerializeField] float lightAttackDamageMod = 3;
    [SerializeField] float heavyAttackDamageMod = 5;
    public int Con { get; private set; }
    public int Str { get; private set; }
    public int Dex { get; private set; }
    public int Int { get; private set; }
    public int CarriedExp { get; private set; } //e depois adicionar a parte visual dessa merda
    public int Level { get; private set; }
    public float CurrentLife { get; private set; }
    public float CurrentMana { get; private set; }
    public float BaseLife { get; private set; }
    public float BaseMana { get; private set; }
    public float BaseMagicDamage { get; private set; }
    public float BaseLightAttackDamage { get; private set; }
    public float BaseHeavyAttackDamage { get; private set; }
    [SerializeField] float maxLife; //testar com valores 1000 + 25*Con
    private float maxMana;
    float magicDamage;
    float lightAttackDamage;

    float heavyAttackDamage;

    //CONTROLES DOS POWER UPS
    private bool PUArmorActive;

    private bool PULifeRegenActive;

    //Coisas de level up
    public int LevelUpPoints { get; private set; }
    private int spentPointsIfCancel;
    public int[] simulatedStatChange;
    public bool isNearCampfire;

    //Coisas de controle geral
    public bool PlayerIsDead { get; private set; }

    private Vector3? respawnPos;

    //Coisas buff de stats das runas
    int[]
        statHasRuneBuff =
            new int[5]; // value of -1, 0 or 1 indicating if there is a change, and whether positive or negative

    int[] runeBuffAmount = new int[5];

    bool hasRuneBuff;

    //Coisas da poção
    public int PotionsAmmount { get; private set; }
    public int PotionLevel;
    int maxPotions;
    float lifeToheal;
    const int maxStartingPotions = 2;
    [SerializeField] int potionLevelMultiplier = 50; //change to const later
    float timerCura;
    const float RegenCooldown = 5f;
    void OnValidate()
    {
        SendHealthInfo();
    }

    void OnEnable()
    {
        GameEventsManager.instance.uiEvents.onRequestBaseStatsInfo += SendBaseStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestExpStatsInfo += SendExpStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestAdvancedStatsInfo += SendAdvancedStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestLevelUpInfo += SendLevelUpInfo;
        GameEventsManager.instance.uiEvents.onChangeStatusButtonPressed += SimulateStatusBuyOrSell;
        GameEventsManager.instance.uiEvents.onConfirmLevelUp += ConfirmChanges;
        GameEventsManager.instance.uiEvents.onDiscardLevelUp += DiscardChanges;
        GameEventsManager.instance.playerEvents.onPlayerRespawned += PlayerRespawn;
        GameEventsManager.instance.uiEvents.onExpCorroutineFinished += GainExp;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp += ActivatePowerUp;
        GameEventsManager.instance.runeEvents.onRuneStatBuff += RuneStatBuff;
        GameEventsManager.instance.skillTreeEvents.onLifeStealHit += HealLife;
        GameEventsManager.instance.uiEvents.onRequestPlayerHealthInfo += SendHealthInfo;
        GameEventsManager.instance.uiEvents.onRequestExpInfo += SendExpInfo;
        GameEventsManager.instance.uiEvents.onRequestPotionAmmountInfo += SendPotionAmmountInfo;
        GameEventsManager.instance.playerEvents.onPlayerPositionSet += AjustPosition;
    }

    void OnDisable()
    {
        GameEventsManager.instance.uiEvents.onRequestBaseStatsInfo -= SendBaseStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestExpStatsInfo -= SendExpStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestAdvancedStatsInfo -= SendAdvancedStatsInfo;
        GameEventsManager.instance.uiEvents.onRequestLevelUpInfo -= SendLevelUpInfo;
        GameEventsManager.instance.uiEvents.onChangeStatusButtonPressed -= SimulateStatusBuyOrSell;
        GameEventsManager.instance.uiEvents.onConfirmLevelUp -= ConfirmChanges;
        GameEventsManager.instance.uiEvents.onDiscardLevelUp += DiscardChanges;
        GameEventsManager.instance.playerEvents.onPlayerRespawned -= PlayerRespawn;
        GameEventsManager.instance.uiEvents.onExpCorroutineFinished -= GainExp;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp += ActivatePowerUp;
        GameEventsManager.instance.runeEvents.onRuneStatBuff -= RuneStatBuff;
        GameEventsManager.instance.skillTreeEvents.onLifeStealHit += HealLife;
        GameEventsManager.instance.uiEvents.onRequestPlayerHealthInfo -= SendHealthInfo;
        GameEventsManager.instance.uiEvents.onRequestExpInfo -= SendExpInfo;
        GameEventsManager.instance.uiEvents.onRequestPotionAmmountInfo -= SendPotionAmmountInfo;
        GameEventsManager.instance.playerEvents.onPlayerPositionSet -= AjustPosition;
        CancelInvoke();
    }

    void Start()
    {
        simulatedStatChange = new int[5];
        CalculateStats();
        //GameEventsManager.instance.uiEvents.UpdateSliders(0,maxLife);//Essas duas funções deveriam ser chamadas
        //GameEventsManager.instance.uiEvents.LifeChange(CurrentLife,false);//             pra stamina e mana tambem
        /* UIManager.instance?.UpdateHealth(CurrentLife,false);
        UIManager.instance.DisplayExpAmmount(CarriedExp);
        não adianta colocar essas coisas aqui pois o UIManager ainda n existe*/
        if (PlayerIsDead)
        {
            Invoke("Die", 1f);
        }
    }
    void Update()
    {
        if (PULifeRegenActive)
        {
            timerCura -= Time.deltaTime;
            if (timerCura <= 0)
            {
                timerCura = RegenCooldown;
                LifeRegenPowerUp();
            }
        }
    }

    private void HealLife(float life)
    {
        if (CurrentLife < maxLife)
        {
            CurrentLife += life;
            if (CurrentLife > maxLife) CurrentLife = maxLife;
            UIManager.instance?.UpdateHealth(CurrentLife, false);
        }
    }

    public void UsePotion()
    {
        if (PotionsAmmount > 0)
        {
            HealLife(lifeToheal);
            PotionsAmmount--;
            UIManager.instance?.DisplayPotionAmmount(PotionsAmmount);
            AudioPlayer.instance.PlaySFX("Drinking");
            AudioPlayer.instance.PlaySFX("Heal");
        }
    }

    private void SendHealthInfo()
    {
        UIManager.instance?.UpdateSliders(0, maxLife); //Essas duas funções deveriam ser chamadas
        //GameEventsManager.instance.uiEvents.LifeChange(CurrentLife,false); //             pra stamina e mana tambem
        UIManager.instance?.UpdateHealth(CurrentLife, false);
    }

    void SendExpInfo()
    {
        //esse chorume existe pq isso aqui n é um singleton, ele responde a um chamado no Start da UI
        UIManager.instance?.DisplayExpAmmount(CarriedExp);
    }

    void SendPotionAmmountInfo()
    {
        //esse chorume existe pq isso aqui n é um singleton, ele responde a um chamado no Start da UI
        UIManager.instance?.DisplayPotionAmmount(PotionsAmmount);
    }

    public void Die()
    {
        //não faz sentido mudar variaveis aqui, pois vamos chamar um load logo após
        DroppedExp.instance?.SetVariablesAndPos(CarriedExp, transform.position);
        GameEventsManager.instance.playerEvents.PlayerDied();
        PlayerStateMachine.Instance.LockPlayer();
        //Debug.Log("Player morreu!");
        PlayerIsDead = true;
        AudioPlayer.instance.PlaySFX("PlayerDeath");
        AudioPlayer.instance.PlayMusic("DeathMusic", false, false);
        PlayerStateMachine.Instance.InCombat = false;
        PlayerIsDead = true;
    }

    private void PlayerRespawn() //chamado pelo game manager depois de dar load
    {
        PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.HasRespawnedHash);
        PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.HasRespawnedHash);

        PlayerStateMachine.Instance.UnlockPlayer();
        HealLife(maxLife);
        UIManager.instance?.UpdateHealth(CurrentLife, false);
        PlayerIsDead = false;
        CarriedExp = 0;
        PotionsAmmount = maxPotions;
        UIManager.instance?.DisplayPotionAmmount(PotionsAmmount);
        UIManager.instance?.DisplayExpAmmount(CarriedExp);
        if (LevelLoadingManager.instance != null)
        {
            transform.position = LevelLoadingManager.instance.RespawnPos();
        }
        else
        {
            transform.position = Vector3.zero;
        }
        /* if (respawnPos.HasValue)
        {
            //Debug.Log("respawnPos tem valor, ent vou colocar minha posição nela");
            transform.position = respawnPos.Value;
        }
        else
        { 
             if (LevelLoadingManager.instance == null)
            {
                //Debug.LogWarning("Não temos um levelLoadingManager, portanto não sabemos onde colocar o jogador ao renascer. Colocando ele no (0,0,0)");
                transform.position = Vector3.zero;
            }
            else
            { 
                //Debug.Log("Como não temos uma respawnPos, vamos voltar por onde o levelLoadingManager mandou");
                respawnPos = LevelLoadingManager.instance.respawnPoint;
                transform.position = respawnPos.Value;
           }
        } */

        AudioPlayer.instance.PlayMusic("MainTheme");
        Physics.SyncTransforms();
    }

    void AjustPosition(Vector3 pos)
    {
        transform.position = pos;
        Physics.SyncTransforms();
    }

    public void CheckPointStatue()
    {
        //Interagir com uma estátua de save

        HealLife(maxLife);
        respawnPos = transform.position;
        PotionsAmmount = maxPotions;
        UIManager.instance?.DisplayPotionAmmount(PotionsAmmount);
    }

    public void SaveData(GameData data)
    {
        PlayerStatsData playerStatsData = new PlayerStatsData(this);
        data.playerStatsData = playerStatsData;
        data.pos = transform.position;
    }

    public void LoadData(GameData data)
    {
        this.Con = data.playerStatsData.con;
        this.Str = data.playerStatsData.str;
        this.Dex = data.playerStatsData.dex;
        this.Int = data.playerStatsData.inte;
        this.CarriedExp = data.playerStatsData.carriedExp;
        this.Level = data.playerStatsData.level;
        this.LevelUpPoints = data.playerStatsData.levelUpPoints;
        this.CurrentLife = data.playerStatsData.currentLife;
        this.BaseLife = data.playerStatsData.baseLife;
        this.BaseMana = data.playerStatsData.baseMana;
        this.CurrentMana = data.playerStatsData.currentMana;
        this.BaseMagicDamage = data.playerStatsData.baseMagicDamage;
        this.BaseLightAttackDamage = data.playerStatsData.baseLightAttackcDamage;
        this.BaseHeavyAttackDamage = data.playerStatsData.baseHeavyAttackDamage;
        this.PlayerIsDead = data.playerStatsData.playerIsDead;
        this.isNearCampfire = data.playerStatsData.isNearCampfire;
        this.PotionsAmmount = data.playerStatsData.potionsAmmount;
        this.PotionLevel = data.playerStatsData.potionLevel;
        AjustPosition(data.pos);
        CalculateStats();
        InformLevelUpPOintsToCheckPoint();
    }

    public void TakeDamage(float damage, Enums.DamageType damageType, bool wasCrit)
    {
        if (PlayerIsDead) return;
        if (PUArmorActive) damage /= 2;
        CurrentLife -= damage;
        //GameEventsManager.instance.uiEvents.LifeChange(CurrentLife,wasCrit);
        Debug.Log("Player tomou dano");
        UIManager.instance?.UpdateHealth(CurrentLife, wasCrit);
        if (damageType == Enums.DamageType.SelfDamage) return;
        PlayerStateMachine.Instance.CameraShake(2f, 0.5f);
        if (CurrentLife <= 0 && !PlayerIsDead)
        {
            CurrentLife = 0;
            PlayerStateMachine.Instance.LockPlayer();
            Die();
        }
        else if (CurrentLife > 0)
        {
            AudioPlayer.instance.PlaySFX("PlayerDamage" + Random.Range(1, 3));
            PlayerStateMachine.Instance.Animator.ResetTrigger(PlayerStateMachine.Instance.TookHitHash);
            PlayerStateMachine.Instance.Animator.SetTrigger(PlayerStateMachine.Instance.TookHitHash);
            PlayerStateMachine.Instance.StaggerPlayer();
        }
    }

    private void ActivatePowerUp(int id)
    {
        //OBS OS IDS SÃO HARD CODED, SE MUDAR A ORDEM DELES PRECISA MUDAR AQUI!!!!!!!
        switch (id)
        {
            case 3:
                PUArmorActive = true;
                //Debug.Log("Ativei o powerUp armor");
                break;
            case 4:
                PULifeRegenActive = true;
                //InvokeRepeating(nameof(LifeRegenPowerUp), 0f, 5f);
                //Debug.Log("Ativei o powerUp regen vida");
                break;
            default: break;
        }
    }
    private void LifeRegenPowerUp()
    {
        if (CurrentLife < maxLife)
        {
            float lifeMissing = maxLife - CurrentLife;
            float healRatio = lifeMissing / 10;
            if (healRatio < 50f) healRatio = 50f;
            HealLife(healRatio);
        }
    }

    void CalculateStats()
    {
        if (!hasRuneBuff)
        {
            maxLife = BaseLife + vidaConsMod * (Con - 10);
            maxMana = BaseMana + manaIntMod * (Int - 10);
            magicDamage = BaseMagicDamage + magicDamageMod * (Int - 10);
            lightAttackDamage = BaseLightAttackDamage + lightAttackDamageMod * (Dex - 10);
            heavyAttackDamage = BaseHeavyAttackDamage + heavyAttackDamageMod * (Str - 10);
            lifeToheal = maxLife / 4 + potionLevelMultiplier * (PotionLevel - 1);
            maxPotions = maxStartingPotions + PotionLevel;
            CalculateWeaponDamage();
            PlayerStateMachine.Instance?.SetMaxManaAndDamage(maxMana, magicDamage);
        }
        else
        {
            maxLife = BaseLife + vidaConsMod * (Con - 10 + runeBuffAmount[0]);
            maxMana = BaseMana + manaIntMod * (Int - 10 + runeBuffAmount[3]);
            magicDamage = BaseMagicDamage + magicDamageMod * (Int - 10 + runeBuffAmount[3]);
            lightAttackDamage = BaseLightAttackDamage + lightAttackDamageMod * (Dex - 10 + runeBuffAmount[1]);
            heavyAttackDamage = BaseHeavyAttackDamage + heavyAttackDamageMod * (Str - 10 + runeBuffAmount[2]);
            lifeToheal = maxLife / 4 + potionLevelMultiplier * (PotionLevel - 1 + runeBuffAmount[4]);
            maxPotions = maxStartingPotions + PotionLevel;
            CalculateWeaponDamage();
        }
    }

    void CalculateWeaponDamage()
    {
        PlayerWeapon katana = GetComponentInChildren<PlayerWeapon>();
        if (katana != null)
        {
            //Debug.Log("O player achou uma arma para setar o dano dela");
            katana.SetDamageAndValues(heavyAttackDamage, lightAttackDamage);
        }
    }

    void RuneStatBuff(bool isActivate, string stat, int amount)
    {
        if (RuneManager.instance.showRuneDebug)
            Debug.Log($"Foi chamado um rune stat buff com isActivavate{isActivate}, do stat {stat} e ammount {amount}");
        hasRuneBuff = true;
        int posNegDiscriminant = amount > 0 ? 1 : -1;
        if (isActivate)
        {
            switch (stat)
            {
                case "vitalidade":
                    statHasRuneBuff[0] = posNegDiscriminant;
                    runeBuffAmount[0] = amount;
                    break;
                case "destreza":
                    statHasRuneBuff[1] = posNegDiscriminant;
                    runeBuffAmount[1] = amount;
                    break;
                case "força":
                    statHasRuneBuff[2] = posNegDiscriminant;
                    runeBuffAmount[2] = amount;

                    break;
                case "inteligência":
                    statHasRuneBuff[3] = posNegDiscriminant;
                    runeBuffAmount[3] = amount;
                    break;
            }
        }
        else
        {
            //Debug.Log("Entrei no false do rune stat buff");
            switch (stat)
            {
                case "vitalidade":
                    statHasRuneBuff[0] = 0;
                    runeBuffAmount[0] = 0;
                    break;
                case "destreza":
                    statHasRuneBuff[1] = 0;
                    runeBuffAmount[1] = 0;
                    break;
                case "força":
                    statHasRuneBuff[2] = 0;
                    runeBuffAmount[2] = 0;

                    break;
                case "inteligência":
                    statHasRuneBuff[3] = 0;
                    runeBuffAmount[3] = 0;
                    break;
            }

            bool allEqZero = false;
            for (int i = 0; i < statHasRuneBuff.Length; i++)
            {
                if (statHasRuneBuff[i] == 0) allEqZero = true;
                else
                {
                    allEqZero = false;
                    break;
                }
            }

            hasRuneBuff = !allEqZero;
        }

        CalculateStats();
        SendBaseStatsInfo();
        SendAdvancedStatsInfo();
    }

    void GainExp(int exp)
    {
        int expToNextLevel = ExpToNextLevel(Level);
        CarriedExp += exp;
        if (CarriedExp >= expToNextLevel)
        {
            CarriedExp -= expToNextLevel;
            LevelUp();
        }
    }

    void LevelUp()
    {
        Level += 1;
        LevelUpPoints += 3;
        SendExpStatsInfo();
        SendLevelUpInfo();
        UIManager.instance?.PlayNotification("Você upou de nível!");
        UIManager.instance?.DisplayExpAmmount(CarriedExp);
        GameEventsManager.instance?.tutorialEvents.LevelUpTutorial();
        DataPersistenceManager.instance.SaveGame();
        InformLevelUpPOintsToCheckPoint();
        GainExp(0);
    }

    int ExpToNextLevel(int level)
    {
        //OBS: ESSA FUNÇÃO DEVERIA SER IDENTICA A FUNÇÃO DE MESMO NOME NO STATSUIMANAGER 
        if (level <= 0)
        {
            //CASO ELA MUDE AQUI ELA DEVERIA SER MUDADA LA TAMBEM
            return 0;
        }
        else return 100 * (int)Mathf.Pow(2, level - 1);
    }

    #region Stats UI

    //Coisas de UI do level up
    void SendBaseStatsInfo()
    {
        //if(!hasRuneBuff){
        //    StatsUIManager.instance?.ReciveBaseStatsInfo(Con,Dex,Str,Int,PotionLevel);
        //}
        //else{
        StatsUIManager.instance?.ReciveBaseStatsInfo(Con + runeBuffAmount[0], Dex + runeBuffAmount[1],
            Str + runeBuffAmount[2],
            Int + runeBuffAmount[3], PotionLevel + runeBuffAmount[4]);
        StatsUIManager.instance?.ColorAtributes(runeBuffAmount);
        //}
    }

    void SendExpStatsInfo()
    {
        StatsUIManager.instance?.ReciveExpStatsInfo(Level, CarriedExp, ExpToNextLevel(Level));
    }

    void SendAdvancedStatsInfo()
    {
        StatsUIManager.instance?.ReciveAdvancedStatsInfo(CurrentLife, maxLife, magicDamage
            , lightAttackDamage, heavyAttackDamage, lifeToheal);
    }

    void SendLevelUpInfo()
    {
        StatsUIManager.instance?.ReciveLevelUpInfo(LevelUpPoints, isNearCampfire);
    }

    public void SimulateStatusBuyOrSell(int statusId, bool isBuying)
    {
        if (isBuying)
        {
            if (LevelUpPoints <= 0) return;
            simulatedStatChange[statusId]++;
            LevelUpPoints--;
            spentPointsIfCancel++;
        }
        else
        {
            if (simulatedStatChange[statusId] < 1) return;
            simulatedStatChange[statusId]--;
            LevelUpPoints++;
            spentPointsIfCancel--;
        }
        
        SendLevelUpInfo();
        SimulateStatusChange(statusId);
    }

    void SimulateStatusChange(int id)
    {
        int displayValue = 0;
        bool isDifferent = (simulatedStatChange[id] != 0) ? true : false;
        switch (id)
        {
            case 0:
                displayValue = hasRuneBuff
                    ? Con + simulatedStatChange[id] + runeBuffAmount[0]
                    : Con + simulatedStatChange[id];
                break;
            case 1:
                displayValue = hasRuneBuff
                    ? Dex + simulatedStatChange[id] + runeBuffAmount[1]
                    : Dex + simulatedStatChange[id];
                break;
            case 2:
                displayValue = hasRuneBuff
                    ? Str + simulatedStatChange[id] + runeBuffAmount[2]
                    : Str + simulatedStatChange[id];
                break;
            case 3:
                displayValue = hasRuneBuff
                    ? Int + simulatedStatChange[id] + runeBuffAmount[3]
                    : Int + simulatedStatChange[id];
                break;
            case 4:
                displayValue = hasRuneBuff
                    ? PotionLevel + simulatedStatChange[id] + runeBuffAmount[4]
                    : PotionLevel + simulatedStatChange[id];
                break;
        }

        StatsUIManager.instance?.SimulateChangeBaseValue(id, displayValue, isDifferent);
        CalculateAdvancedInfoAndSend(id);
    }

    void CalculateAdvancedInfoAndSend(int id)
    {
        bool isDifferent = (simulatedStatChange[id] != 0) ? true : false;
        switch (id)
        {
            case 0:
                float simuMaxLife = hasRuneBuff
                    ? BaseLife + vidaConsMod * (Con + simulatedStatChange[0] - 10 + runeBuffAmount[0])
                    : BaseLife + vidaConsMod * (Con + simulatedStatChange[0] - 10);
                StatsUIManager.instance?.SimulateChangeAdvancedValue(0, CurrentLife, simuMaxLife, isDifferent);
                float simuPotionHeal = hasRuneBuff
                    ? simuMaxLife / 4 + potionLevelMultiplier *
                    (PotionLevel + simulatedStatChange[4] - 1 + runeBuffAmount[4])
                    : simuMaxLife / 4 + potionLevelMultiplier * (PotionLevel + simulatedStatChange[4] - 1);
                StatsUIManager.instance?.SimulateChangeAdvancedValue(5, 0, simuPotionHeal, isDifferent);
                break;
            case 1:
                float simuLightAttackDamage = hasRuneBuff
                    ? BaseLightAttackDamage +
                      lightAttackDamageMod * (Dex + simulatedStatChange[1] - 10 + runeBuffAmount[1])
                    : BaseLightAttackDamage + lightAttackDamageMod * (Dex + simulatedStatChange[1] - 10);
                StatsUIManager.instance?.SimulateChangeAdvancedValue(2, 0, simuLightAttackDamage, isDifferent);
                break;
            case 2:
                float simuHeavyAttackDamage = hasRuneBuff
                    ? BaseHeavyAttackDamage +
                      heavyAttackDamageMod * (Str + simulatedStatChange[2] - 10 + runeBuffAmount[2])
                    : BaseHeavyAttackDamage + heavyAttackDamageMod * (Str + simulatedStatChange[2] - 10);
                StatsUIManager.instance?.SimulateChangeAdvancedValue(3, 0, simuHeavyAttackDamage, isDifferent);
                break;
            case 3:
                int newIntValue = hasRuneBuff
                    ? Int + simulatedStatChange[3] + runeBuffAmount[3]
                    : Int + simulatedStatChange[3];
                float simuMaxMana = BaseMana + manaIntMod * (newIntValue - 10);
                float simuMagicDamage = BaseMagicDamage + magicDamageMod * (newIntValue - 10);
                StatsUIManager.instance?.SimulateChangeAdvancedValue(1, CurrentMana, simuMaxMana, isDifferent);
                StatsUIManager.instance?.SimulateChangeAdvancedValue(4, 0, simuMagicDamage, isDifferent);
                break;
            case 4:
                if (simulatedStatChange[0] != 0)
                {
                    simuMaxLife = hasRuneBuff
                        ? BaseLife + vidaConsMod * (Con + simulatedStatChange[0] - 10 + runeBuffAmount[0])
                        : BaseLife + vidaConsMod * (Con + simulatedStatChange[0] - 10);
                    simuPotionHeal = hasRuneBuff
                        ? simuMaxLife / 4 + potionLevelMultiplier *
                        (PotionLevel + simulatedStatChange[4] - 1 + runeBuffAmount[4])
                        : simuMaxLife / 4 + potionLevelMultiplier * (PotionLevel + simulatedStatChange[4] - 1);
                    StatsUIManager.instance?.SimulateChangeAdvancedValue(5, 0, simuPotionHeal, isDifferent);
                }
                else
                {
                    simuPotionHeal = hasRuneBuff
                        ? maxLife / 4 + potionLevelMultiplier *
                        (PotionLevel + simulatedStatChange[4] - 1 + runeBuffAmount[4])
                        : maxLife / 4 + potionLevelMultiplier * (PotionLevel + simulatedStatChange[4] - 1);
                    StatsUIManager.instance?.SimulateChangeAdvancedValue(5, 0, simuPotionHeal, isDifferent);
                }

                break;
        }
    }

    void ConfirmChanges()
    {
        Con += simulatedStatChange[0];
        Dex += simulatedStatChange[1];
        Str += simulatedStatChange[2];
        Int += simulatedStatChange[3];
        PotionLevel += simulatedStatChange[4];
        CalculateStats();
        SendBaseStatsInfo();
        SendAdvancedStatsInfo();
        for (int i = 0; i < simulatedStatChange.Length; i++) simulatedStatChange[i] = 0;
        spentPointsIfCancel = 0;
        SendLevelUpInfo();
        InformLevelUpPOintsToCheckPoint();
        SendHealthInfo();
        CheckPointStatue();
        DataPersistenceManager.instance?.SaveGame();
    }

    void DiscardChanges()
    {
        LevelUpPoints = LevelUpPoints + spentPointsIfCancel;
        for (int i = 0; i < simulatedStatChange.Length; i++) simulatedStatChange[i] = 0;
        spentPointsIfCancel = 0;
        InformLevelUpPOintsToCheckPoint();
    }
    void InformLevelUpPOintsToCheckPoint()
    {
        GameEventsManager.instance.playerEvents.InformLevelUpPoints(LevelUpPoints);
    }

    #endregion
}