using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneIdle : AEnemyAction
{
    float restTime;


    public override void SetRestTime(float restTime)
    {
        this.restTime = restTime;
        //Debug.Log("First rest time: " + this.restTime);
    }
    public override void UpdateAction()
    {
        base.StartAction(enemyController);
        enemyController.SetBoolAnimation("isIdle", true);
        enemyController.SetBlendTree("Velocity", 0);
        //Debug.Log("Idle: " + restTime);
    }

    public override void ExitAction()
    {
        enemyController.SetBoolAnimation("isIdle", false);
        enemyController.ChangeAction(false);
    }

}
