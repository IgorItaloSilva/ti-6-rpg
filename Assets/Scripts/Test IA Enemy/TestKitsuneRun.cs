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
        Vector3 dir = (target.position - enemyController.transform.position); // direção onde o jogador está
        Quaternion desiredRotation = Quaternion.LookRotation(dir); // Rotação desejada
        desiredRotation.x = 0f;
        desiredRotation.z = 0f;
        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, desiredRotation, 0.15f);
        if (rb.velocity.magnitude < 4)
            rb.velocity += dir.normalized * 55 * Time.fixedDeltaTime;
        if((target.position - enemyController.transform.position).magnitude <= nextMinRange)
            ExitAction(enemyController.GetAttackActions());

    }

    public override void ExitAction(TestEnemyActions enemyAction)
    {
        animator.SetBool("isRunning", false);
        enemyController.SetAttack();

    }
}
