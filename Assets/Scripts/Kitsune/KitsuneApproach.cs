using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitsuneApproach : AEnemyAction
{
    public override void SetDistance(float distance)
    {
        base.SetDistance(distance);
    }

    public override void StartAction(OldEnemyController enemyController)
    {
        base.StartAction(enemyController);
        base.enemyController.SetBoolAnimation("isRunning", true);
        Debug.Log("Approach: " + minDistanceSkill);
    }

    public override void UpdateAction()
    {
        Vector3 dir = (target.position - enemyController.transform.position); // Dire��o
        Quaternion desiredRotation = Quaternion.LookRotation(dir); // Dire��o de rota��o
        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, desiredRotation, 0.15f); // Rotacionar constantemente para encarar o jogador
        if (rb.linearVelocity.magnitude < 4) // determinar velocidade maxima do inimigo e aplicar velocidade
            rb.linearVelocity += dir.normalized * 55 * Time.fixedDeltaTime;
        enemyController.SetBlendTree("Velocity", rb.linearVelocity.magnitude); // aplicar velocidade na blend tree
        if (Vector3.Distance(target.position, enemyController.transform.position) < minDistanceSkill - (minDistanceSkill / 2)) // checar se esta na distancia minima de ativar a pr�xima a��o
            ExitAction();
    }

    public override void ExitAction()
    {
        enemyController.SetBoolAnimation("isRunning", false);
        enemyController.ChangeAction(false);
    }
}
