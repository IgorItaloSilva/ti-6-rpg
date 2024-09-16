using UnityEngine;


public class BotTestTurn : AEnemyAction
{
    float rotationTime;
    Quaternion desiredRotation;
    Quaternion thisRotation;


    public override void StartAction(EnemyController _enemyBehave)
    {
        base.StartAction(_enemyBehave);
        enemyController.SetBoolAnimation("isTurning", true);
        desiredRotation = Quaternion.LookRotation(target.transform.position - enemyController.transform.position);
        thisRotation = enemyController.transform.rotation;
        rotationTime = 0;
        Debug.Log("Turn");
    }

    public override void UpdateAction()
    {
        if (rotationTime < 1)
        {
            enemyController.transform.rotation = Quaternion.Slerp(thisRotation, desiredRotation, rotationTime);
            rotationTime += Time.fixedDeltaTime;
        }
        else
            ExitAction();
    }

    public override void ExitAction()
    {
        enemyController.SetBoolAnimation("isTurning", false);
        enemyController.ChangeAction(false);
    }
}
