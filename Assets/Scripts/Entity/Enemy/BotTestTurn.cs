using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTestTurn : AEnemyAction
{
    float rotationTime;
    Quaternion desiredRotation;
    Quaternion thisRotation;

    public BotTestTurn(AEnemyBehave enemyBehave) : base(enemyBehave)
    {
        this.enemyBehave = enemyBehave;
    }

    public override void StartAction(EnemyController _enemyBehave)
    {
        base.StartAction(_enemyBehave);
        enemyController.SetBoolAnimation("isTurning", true);
        //desiredRotation = Quaternion.FromToRotation(enemyController.transform.forward, (enemyController.transform.position - target.transform.position).normalized);
        desiredRotation = Quaternion.LookRotation(target.transform.position - enemyController.transform.position);
        thisRotation = enemyController.transform.rotation;
        rotationTime = 0;
        Debug.Log("");
        Debug.Log("Turn");
    }

    public override void UpdateAction()
    {
        Debug.Log(desiredRotation);
        if (rotationTime < 1)
        {
            enemyController.transform.rotation = Quaternion.Slerp(thisRotation, desiredRotation, rotationTime);
            rotationTime += Time.fixedDeltaTime;
        }
        else
            EndAnimation();
    }

    public override void EndAnimation()
    {
        enemyController.SetBoolAnimation("isTurning", false);
        ExitAction(enemyBehave.GetAction(0));
    }

}
