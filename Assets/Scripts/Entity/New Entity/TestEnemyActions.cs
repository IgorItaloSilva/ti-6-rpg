using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestEnemyActions : MonoBehaviour
{
    protected TestEnemyController enemyController; // Controlador do inimigo
    protected Rigidbody rb;
    protected Animator animator;
    protected float restTime;

    protected float minRange; // Minimo de distancia entre o jogador para a a��o acontecer
    protected Transform target;

    public void StartAction(TestEnemyController enemyController)
    {
        this.enemyController = enemyController;
        this.rb = enemyController.GetRigidbody();
        this.animator = enemyController.GetAnimator();
        this.restTime = enemyController.GetRestTime();
        this.target = enemyController.GetTarget();
        AdditionalStart();
    }

    protected virtual void AdditionalStart() { } // Caso precise de declarar algo a mais no come�o da a��o
    // Recomendado:
    // Declarar minRange de acordo com a skill
    // Declarar qual anima��o deve realizar

    public abstract void UpdateAction(); // Atualiza Todo frame

    public abstract void ExitAction(TestEnemyActions enemyAction); // Chamado no final da a��o 
    // Caso tenha sido um ataque, adicionar: enemyController.EnemyAttacked();

    public virtual void AccelerateRest() { }

    public float GetMinRange() { return minRange; }

    public bool InRestTime()
    {
        restTime -= Time.fixedDeltaTime;
        return restTime > 0;
    }

}
