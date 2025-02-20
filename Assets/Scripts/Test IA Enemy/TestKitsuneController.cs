using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestKitsuneController : TestEnemyController//,IDamagable
{
    float currentHp = 100;
    [SerializeField]Slider sliderVida;
    protected override void StartEnemy()
    {
        movementActions.Add(new TestKitsuneIdle());
        movementActions.Add(new TestKitsuneRun());
        attackActions.Add(new TestKitsuneHeadButt());
        ShuffleAttacks();
        currentAction = new TestKitsuneIdle();
        currentAction.StartAction(this);
    }
    public void TakeDamage(float damage,Enums.DamageType damageType)
    {
        currentHp -= damage;
        sliderVida.value = currentHp;
        if(currentHp<=0){
            Die();
        }
    }
    public void Die(){
        Destroy(gameObject);
    }

}
