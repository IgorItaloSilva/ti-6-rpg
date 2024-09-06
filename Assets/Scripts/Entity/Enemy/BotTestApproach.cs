using System.Collections;
using UnityEngine;

public class BotTestApproach : AEnemyAction
{
    public BotTestApproach(AEnemyBehave enemyBehave) : base(enemyBehave)
    {
        this.enemyBehave = enemyBehave;
    }

    public override void StartAction(EnemyController _enemyBehave)
    {
        base.StartAction(_enemyBehave);
        enemyController.SetBoolAnimation("isRunning", true);
        Debug.Log("");
        Debug.Log("Approach");
    }

    public override void UpdateAction()
    {
        Vector3 dir = (target.position - enemyController.transform.position).normalized;
        rb.transform.LookAt(target);
        rb.velocity = dir * 180 * Time.fixedDeltaTime;
        if (Vector3.Distance(target.position, enemyController.transform.position) < 1.5f)
        {
            enemyController.SetBoolAnimation("isRunning", false);
            ExitAction(enemyBehave.GetAction(3));
        }
    }
}

