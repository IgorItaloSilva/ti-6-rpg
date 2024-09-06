using UnityEngine;


public class BotTestAttack : AEnemyAction
{
    int countAction = 0;

    public BotTestAttack(AEnemyBehave enemyBehave) : base(enemyBehave)
    {
        this.enemyBehave = enemyBehave;
    }

    public override void StartAction(EnemyController _enemyBehave)
    {
        base.StartAction(_enemyBehave);
        enemyController.SetBoolAnimation("isAttacking", true);
        countAction = 0;
        canExit = false;
        Debug.Log("");
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
            if (Vector3.Distance(target.position, enemyBehave.transform.position) > 1.5f)
            {
                enemyController.SetBoolAnimation("isAttacking", false);
            }
        }
    }

    public override void EndAnimation()
    {
        enemyController.SetBoolAnimation("isAttacking", false);
        enemyBehave.SkillUsed(3);
        ExitAction(enemyBehave.GetAction(0));
    }

}
