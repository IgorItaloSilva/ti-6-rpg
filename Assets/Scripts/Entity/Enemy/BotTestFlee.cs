using UnityEngine;


public class BotTestFlee : AEnemyAction
{
    public BotTestFlee(AEnemyBehave enemyBehave) : base(enemyBehave)
    {
        this.enemyBehave = enemyBehave;
    }

    public override void StartAction(EnemyController _enemyBehave)
    {
        base.StartAction(_enemyBehave);
        enemyController.SetBoolAnimation("isRunning", true);
        Debug.Log("");
        Debug.Log("Flee");
    }

    public override void UpdateAction()
    {
        Vector3 dir = (enemyController.transform.position - target.position).normalized;
        rb.transform.LookAt(dir + enemyController.transform.position);
        rb.velocity = dir * 180 * Time.fixedDeltaTime;

        if (enemyBehave.SkillReady())
        {
            enemyController.SetBoolAnimation("isRunning", false);
            ExitAction(enemyBehave.GetAction(0));
        }
    }
}