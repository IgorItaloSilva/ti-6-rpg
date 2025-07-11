using System;

public class UIEvents
{

    public event Action<float, bool> onLifeChange;
    public void LifeChange(float vidaAtual, bool wasCrit)
    {
        if (onLifeChange != null)
        {
            onLifeChange(vidaAtual, wasCrit);
        }
    }

    /* public event Action<int,float> onUpdateSliders;
    public void UpdateSliders(int id,float maxValue){
        if(onUpdateSliders!=null){
            onUpdateSliders(id,maxValue);
        }
    } */
    public event Action onSavedGame;
    public void SavedGame()
    {
        if (onSavedGame != null)
        {
            onSavedGame();
        }
    }
    public event Action OnDialogOpened;
    public void DialogOpen()
    {
        if (OnDialogOpened != null)
        {
            OnDialogOpened();
        }
    }
    public event Action<int, int> onSkillTreeMoneyChange;
    public void SkillTreeMoneyChange(int index, int value)
    {
        if (onSkillTreeMoneyChange != null)
        {
            onSkillTreeMoneyChange(index, value);
        }
    }
    public event Action<int, int, int, int> onReviceBaseStatsInfo;
    public void ReciveBaseStatsInfo(int con, int str, int dex, int inte)
    {
        if (onReviceBaseStatsInfo != null)
        {
            onReviceBaseStatsInfo(con, str, dex, inte);
        }
    }
    public event Action<int, float> onReviceExpStatsInfo;
    public void ReciveExpStatsInfo(int level, int currentExp)
    {
        if (onReviceExpStatsInfo != null)
        {
            onReviceExpStatsInfo(level, currentExp);
        }
    }
    public event Action<float, float, float, float, float, float, float> onReviceAdvancedStatsInfo;
    public void ReciveAdvancedStatsInfo(float currentLife, float maxLife, float currentMana, float maxMana,
                                float magicDamage, float ligthAttackDamage, float heavyAtackDamage)
    {
        if (onReviceAdvancedStatsInfo != null)
        {
            onReviceAdvancedStatsInfo(currentLife, maxLife, currentMana, maxMana, magicDamage, ligthAttackDamage, heavyAtackDamage);
        }
    }
    public event Action onRequestBaseStatsInfo;
    public void RequestBaseStatsInfo()
    {
        if (onRequestBaseStatsInfo != null)
        {
            onRequestBaseStatsInfo();
        }
    }
    public event Action onRequestExpStatsInfo;
    public void RequestExpStatsInfo()
    {
        if (onRequestExpStatsInfo != null)
        {
            onRequestExpStatsInfo();
        }
    }
    public event Action onRequestAdvancedStatsInfo;
    public void RequestAdvancedStatsInfo()
    {
        if (onRequestAdvancedStatsInfo != null)
        {
            onRequestAdvancedStatsInfo();
        }
    }
    public event Action onRequestLevelUpInfo;
    public void RequestLevelUpInfo()
    {
        if (onRequestLevelUpInfo != null)
        {
            onRequestLevelUpInfo();
        }
    }

    //COISAS LEVEL UP
    public event Action<int, int, bool> onSimulateChangeBaseValue;
    public void SimulateChangeBaseValue(int statId, int newValue, bool isDifferent)
    {
        if (onSimulateChangeBaseValue != null)
        {
            onSimulateChangeBaseValue(statId, newValue, isDifferent);
        }
    }
    public event Action<int, float, float, bool> onSimulateChangeAdvancedValue;
    public void SimulateChangeAdvancedValue(int hardcodedId, float currentLifeOrMana, float newValue, bool isDifferent)
    {
        if (onSimulateChangeAdvancedValue != null)
        {
            onSimulateChangeAdvancedValue(hardcodedId, currentLifeOrMana, newValue, isDifferent);
        }
    }
    /* public event Action<int,bool> onReciveLevelUpInfo;
    public void ReciveLevelUpInfo(int poitsToSpend, bool isNearCampfire){
        if(onReciveLevelUpInfo!=null){
            onReciveLevelUpInfo(poitsToSpend,isNearCampfire);
        }
    } */
    public event Action<int, bool> onChangeStatusButtonPressed;
    public void ChangeStatusButtonPressed(int statusId, bool isBuying)
    {
        if (onChangeStatusButtonPressed != null)
        {
            onChangeStatusButtonPressed(statusId, isBuying);
        }
    }
    public event Action onConfirmLevelUp;
    public void ConfirmLevelUp()
    {
        if (onConfirmLevelUp != null)
        {
            onConfirmLevelUp();
        }
    }
    public event Action onDiscardLevelUp;
    public void DiscardLevelUp()
    {
        if (onDiscardLevelUp != null)
        {
            onDiscardLevelUp();
        }
    }

    public event Action onPauseGame;
    public void PauseGame()
    {
        if (onPauseGame != null)
        {
            onPauseGame();
        }
    }

    public event Action onUnpauseGame;
    public void UnpauseGame()
    {
        if (onUnpauseGame != null)
        {
            onUnpauseGame();
        }
    }
    public event Action<float, float, string> OnBossInfoDisplayed;
    public void BossInfoDisplay(float currentHp, float maxhp, string name)
    {
        if (OnBossInfoDisplayed != null)
        {
            OnBossInfoDisplayed(currentHp, maxhp, name);
        }
    }
    public event Action<float> OnUpdateBossLife;
    public void UpdateBossLife(float currentLife)
    {
        if (OnUpdateBossLife != null)
        {
            OnUpdateBossLife(currentLife);
        }
    }
    public event Action onRequestPlayerHealthInfo;
    public void RequestPlayerHealthInfo()
    {
        if (onRequestPlayerHealthInfo != null)
        {
            onRequestPlayerHealthInfo();
        }
    }
    public event Action onRequestExpInfo;
    public void RequestExpInfo()
    {
        if (onRequestExpInfo != null)
        {
            onRequestExpInfo();
        }
    }
    public event Action onRequestPotionAmmountInfo;
    public void RequestPotionAmmountInfo()
    {
        if (onRequestPotionAmmountInfo != null)
        {
            onRequestPotionAmmountInfo();
        }
    }
    public event Action onBuyLevelClicked;
    public void BuyLevelClicked()
    {
        if (onBuyLevelClicked != null)
        {
            onBuyLevelClicked();
        }
    }
    public event Action<int> onExpCorroutineFinished;
    public void ExpCorroutineFinished(int expAmmount) {
        if (onExpCorroutineFinished != null)
        {
            onExpCorroutineFinished(expAmmount);
        }
    }
}
