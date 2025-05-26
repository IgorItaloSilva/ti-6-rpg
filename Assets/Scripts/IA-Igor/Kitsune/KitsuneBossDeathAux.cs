using UnityEngine;

public class KitsuneBossDeathAux : MonoBehaviour
{
    [SerializeField] GameObject portalNextLevel;
    [SerializeField] GameObject dialogInteractableBoss;
    [SerializeField] BossSkillPointDialogAnswers bossSkillPointDialogAnswers;
    public void Activate(EnemyBehaviour enemyBehaviour)
    {
        portalNextLevel.SetActive(true);
        dialogInteractableBoss.SetActive(true);
        bossSkillPointDialogAnswers.enemyBehaviour = enemyBehaviour;

    }
}
