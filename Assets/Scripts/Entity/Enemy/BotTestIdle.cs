using UnityEngine;


public class BotTestIdle : AEnemyAction
{
    float restTime;
    public BotTestIdle(AEnemyBehave enemyBehave) : base(enemyBehave)
    {
        this.enemyBehave = enemyBehave;
    }

    public override void StartAction(EnemyController _enemyBehave)
    {
        restTime = Random.Range(0.5f, 1.5f);
        base.StartAction(_enemyBehave);
        enemyController.SetBoolAnimation("isIdle", true);
        Debug.Log("");
        Debug.Log("Idle");
    }

    public override void UpdateAction()
    {
        if (restTime <= 0)
            BehaveLogic();
        restTime = Mathf.Max(0, restTime - Time.deltaTime);
    }

    public void BehaveLogic()
    {
        Vector3 thisForward = enemyController.transform.forward;
        Vector3 dir = (target.position - enemyController.transform.position).normalized;
        thisForward.y = 0;
        dir.y = 0;
        if(Vector3.Angle(enemyController.transform.forward, dir) > 130f)
        {
            enemyController.SetBoolAnimation("isIdle", false);
            ExitAction(enemyBehave.GetAction(4));
        }else if (enemyBehave.SkillReady())
        {
            enemyController.SetBoolAnimation("isIdle", false);
            ExitAction(enemyBehave.GetAction(1));
        }
        else
        {
            if (Vector3.Distance(target.position, enemyController.transform.position) <= 4.5f)
            {
                enemyController.SetBoolAnimation("isIdle", false);
                ExitAction(enemyBehave.GetAction(2));
            }
        }
    }

}
