using UnityEngine;

public class BossDeathInteractable : SkillPointInteractable
{
    public ActualEnemyController bossController;
    public int expIfRescued;
    public void Activate(){
        Active=true;
        CanInteract=true;
        Save();
    }
    protected override void Rescue()
    {
        GameEventsManager.instance.playerEvents.PlayerGainExp(expIfRescued);
        base.Rescue();
    }
    public override void Die()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Dark);
        AlreadyInterated=true;
        Active=false;
        Save();
        gameObject.SetActive(false);
        bossController.ActualDeath();
        //Efeito de morrer no boss...
    }
}
