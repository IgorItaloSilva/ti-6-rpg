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
        Vector3 dir = (target.position - enemyController.transform.position); // Direção
        Quaternion desiredRotation = Quaternion.LookRotation(dir); // Direção de rotação
        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, desiredRotation, 0.15f); // Rotacionar constantemente para encarar o jogador
        if (rb.velocity.magnitude < 4) // determinar velocidade maxima do inimigo e aplicar velocidade
            rb.velocity += dir.normalized * 55 * Time.fixedDeltaTime;
        enemyController.SetBlendTree("Velocity", rb.velocity.magnitude); // aplicar velocidade na blend tree
        if (Vector3.Distance(target.position, enemyController.transform.position) < minDistanceSkill - (minDistanceSkill / 2)) // checar se esta na distancia minima de ativar a próxima ação
            ExitAction();
    }

    public override void ExitAction()
    {
        enemyController.SetBoolAnimation("isRunning", false);
        enemyController.ChangeAction(false);
    }
}
