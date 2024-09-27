using UnityEngine;


public class BotTestApproach : AEnemyAction
{
    public override void SetDistance(float distance)
    {
        base.SetDistance(distance);
    }

    public override void StartAction(EnemyController enemyController)
    {
        base.StartAction(enemyController);
        base.enemyController.SetBoolAnimation("isRunning", true);
        Debug.Log("Approach: " + minDistanceSkill);
        

    }

    public override void UpdateAction()
    {
        //Debug.Log("Velocity: " + rb.velocity.magnitude);
        Vector3 dir = (target.position - enemyController.transform.position);
        //rb.transform.LookAt(Vector3.Slerp(enemyController.transform.position + enemyController.transform.forward, dir, 0.15f));
        Quaternion desiredRotation = Quaternion.LookRotation(dir);
        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, desiredRotation, 0.15f);
        if (rb.velocity.magnitude < 4)
            rb.velocity += dir.normalized * 55 * Time.fixedDeltaTime;
        enemyController.SetBlendTree("Velocity", rb.velocity.magnitude);
        if (Vector3.Distance(target.position, enemyController.transform.position) < minDistanceSkill - (minDistanceSkill / 2))
            ExitAction();
    }

    public override void ExitAction()
    {
        enemyController.SetBoolAnimation("isRunning", false);
        enemyController.ChangeAction(false);
    }
}

