using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestEnemyActions : MonoBehaviour
{
    protected TestEnemyController enemyController; // Controlador do inimigo
    protected Rigidbody rb;
    protected Animator animator;
    protected float restTime;
    protected Vector3 dir;

    protected float minRange; // Minimo de distancia entre o jogador para a ação acontecer
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

    protected virtual void AdditionalStart() { } // Caso precise de declarar algo a mais no começo da ação
    // Recomendado:
    // Declarar minRange de acordo com a skill
    // Declarar qual animação deve realizar

    public abstract void UpdateAction(); // Atualiza Todo frame

    public abstract void ExitAction(TestEnemyActions enemyAction); // Chamado no final da ação 
    // Caso tenha sido um ataque, adicionar: enemyController.EnemyAttacked();

    public virtual void AccelerateRest() { }

    protected virtual void TrackTarget(float trackSpeed =8f)
    {
        dir = (target.position - enemyController.transform.position); // direção onde o jogador está
        Quaternion desiredRotation = Quaternion.LookRotation(dir); // Rotação desejada
        desiredRotation.x = 0f;
        desiredRotation.z = 0f;
        enemyController.transform.rotation = Quaternion.Slerp(enemyController.transform.rotation, desiredRotation, trackSpeed * Time.deltaTime);
    }

    protected virtual void GoToTarget(float speed = 4f)
    {
        if (rb.linearVelocity.magnitude < 4)
            rb.linearVelocity += dir.normalized * ((speed * 1000f) * Time.deltaTime);
    }

    public float GetMinRange() { return minRange; }

    public bool InRestTime()
    {
        restTime -= Time.deltaTime;
        return restTime > 0;
    }

}
