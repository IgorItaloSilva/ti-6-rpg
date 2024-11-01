using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestKitsuneRun : TestEnemyActions
{
    float nextMinRange;
    protected override void AdditionalStart()
    {
        animator.SetBool("isRunning", true);
        nextMinRange = enemyController.GetNextMinRange();
    }

    public override void UpdateAction()
    {
        TrackTarget();
        GoToTarget();
        if((target.position - enemyController.transform.position).magnitude <= nextMinRange)
            ExitAction(enemyController.GetAttackActions());

    }

    public override void ExitAction(TestEnemyActions enemyAction)
    {
        rb.velocity = Vector3.zero;
        animator.SetBool("isRunning", false);
        enemyController.SetAttack();

    }
}
