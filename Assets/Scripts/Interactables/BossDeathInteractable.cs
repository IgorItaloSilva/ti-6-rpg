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
        base.Rescue();
        GameEventsManager.instance.playerEvents.PlayerGainExp(expIfRescued);
    }
    public override void Die()
    {
        base.Die();
        bossController.ActualDeath();
        //Efeito de morrer no boss...
    }
}
