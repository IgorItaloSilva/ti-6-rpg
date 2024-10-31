using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestKitsuneIdle : TestEnemyActions
{
    float nextMinRange;

    protected override void AdditionalStart() 
    {
        animator.SetBool("isIdle", true);
        nextMinRange = enemyController.GetNextMinRange();
    }

    public override void UpdateAction()
    {
        if (InRestTime())
            return;

        if((target.position - enemyController.transform.position).magnitude > nextMinRange)
            ExitAction(enemyController.GetMoveActions(1));
        else
        {
            ExitAction(enemyController.GetAttackActions());
        }
        //Debug.Log(target);
        ////  -----   Checar se está olhando para o alvo   -----  \\
        //Vector3 dir = (target.position - enemyController.transform.position).normalized;
        //dir.y = 0;
        //if (Vector3.Angle(transform.forward, dir) > 60f) // Checar se o jogador está em um determinado angulo de visão
        //    Debug.Log(Vector3.Angle(enemyController.transform.forward, dir)); //currentAction.ExitAction(movementActions[3]);

    }

    public override void ExitAction(TestEnemyActions enemyAction)
    {
        animator.SetBool("isIdle", false);
        enemyController.SetMovement(enemyAction);

    }

    public override void AccelerateRest()
    {
        
    }

}
