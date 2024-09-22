using UnityEngine;


public class BotTestIdle : AEnemyAction
{
    float restTime;


    public override void SetRestTime(float restTime)
    {
        this.restTime = restTime;
        Debug.Log("First rest time: " + this.restTime);
    }

    public override void StartAction(EnemyController enemyController)
    {
        base.StartAction(enemyController);
        enemyController.SetBoolAnimation("isIdle", true);
        enemyController.SetBlendTree("Velocity", 0);
        Debug.Log("Idle: " + restTime);
    }

    public override void UpdateAction()
    {
        if (restTime <= 0)
            ExitAction();
        restTime = Mathf.Max(0, restTime - Time.deltaTime);
    }

    public override void ExitAction()
    {
        enemyController.SetBoolAnimation("isIdle", false);
        enemyController.ChangeAction(false);
    }

}