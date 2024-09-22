using UnityEngine;


public class BotTestKickCombo : AEnemyAction
{
    int countAction = 0;

    public BotTestKickCombo()
    {
        minDistanceSkill = 3;
    }


    public override void StartAction(EnemyController enemyController)
    {
        base.StartAction(enemyController);
        base.enemyController.SetBoolAnimation("isAttacking", true);
        
        countAction = 0;
        Debug.Log("Attack");
    }

    public override void UpdateAction(){  }

    public override void ActionWithAnimator()
    {

        if (countAction < 3)
        {
            Vector3 dir = (target.position - enemyController.transform.position).normalized;
            rb.transform.LookAt(target);
            rb.AddForce(dir * 3.5f, ForceMode.Impulse);
            rb.velocity = dir * 200 * Time.fixedDeltaTime;
            countAction++;
            if (Vector3.Distance(target.position, enemyController.transform.position) > 1.5f)
                enemyController.SetBoolAnimation("isAttacking", false);
        }else
            ExitAction();
    }


    public override void ExitAction()
    {
        enemyController.SetBoolAnimation("isAttacking", false);
        enemyController.ChangeAction(true);
    }
}
