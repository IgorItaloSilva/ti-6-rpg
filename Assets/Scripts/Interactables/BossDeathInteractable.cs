using UnityEngine;

public class BossDeathInteractable : SkillPointInteractable
{
    ActualEnemyController bossController;
    public void Activate(){
        Active=true;
        Save();
    }
    public override void Die()
    {
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Dark);
        AlreadyInterated=true;
        Active=false;
        Save();
        gameObject.SetActive(false);
        //Efeito de morrer no boss...
    }
}
