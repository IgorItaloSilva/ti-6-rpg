using UnityEngine;

public class BossSkillPointDialogAnswers : DialogAnswer
{
    [SerializeField] int expIfRescued;
    [HideInInspector] public EnemyBehaviour enemyBehaviour;
    public override void Option1()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Dark);
        enemyBehaviour?.ActualDeath();
    }

    public override void Option2()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Light);
        GameEventsManager.instance.playerEvents.PlayerGainExp(expIfRescued);
        //idealmente trocaria o modelo do boss pela versão nõa corrompida, ou colocaria um vfx ou algo assim
    }

    public override void Option3()
    {
        //
    }
}
