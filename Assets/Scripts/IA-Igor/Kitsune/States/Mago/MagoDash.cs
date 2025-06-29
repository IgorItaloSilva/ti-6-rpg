using UnityEngine;

public class MagoDash : EnemyBaseState
{
    float timer;
    bool canAttack;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void OneExecution()
    {
        restTime = 1;
        animator.CrossFade("Dash", 0.15f);
        lookTime = 0.45f;
        canAttack = true;
        timer = 0;
    }

    // Update is called once per frame
    public override void StateUpdate()
    {
        if (canAttack) {
            if (timer >= 1.3f) {
                enemyBehave.UseWeapon();
                canAttack = false;
            }
            else {
                charControl.transform.rotation = ApplyRotation();
            }

        }
        else {
            if (timer <= 2.08f)
                charControl.Move(charControl.transform.forward * speed * 6 * Time.deltaTime + Vector3.up * ApplyGravity());
            else if (timer >= 3.120f)
                {
                    enemyBehave.SetRest(restTime);
                    enemyBehave.StartIdle();
                    enemyBehave.ChoseSkill();
                    return;
                }

        }
        timer += Time.deltaTime;
    }
    
}