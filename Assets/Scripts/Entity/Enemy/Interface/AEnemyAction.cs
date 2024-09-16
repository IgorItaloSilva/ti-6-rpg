using UnityEngine;


public abstract class AEnemyAction
{

    protected Rigidbody rb; // Rigidbody do personagem atual
    protected EnemyController enemyController; // Script que est� executando esta a��o
    protected Transform target;
    protected float minDistanceSkill;


    public virtual void StartAction(EnemyController enemyController) // Metodo inicial para a��es de ataque (Definir na nova a��o qual )
    {
        rb = enemyController.GetRB(); // Declara Rigidbody do personagem
        target = enemyController.GetTarget();
        this.enemyController = enemyController; // Declara variavel do personagem
    }

    // ------
    public abstract void UpdateAction(); // O que acontece a cada frame da a��o

    public abstract void ExitAction(); // Conclus�o da a��o (Qual o index da habilidade para aplicar o cooldown)

    public virtual void ActionWithAnimator() { }

    public virtual void CanExit() { }
    public virtual void EndAnimation() { }

    public float GetMinDistanceSkill() { return minDistanceSkill; }

    public virtual void SetDistance(float distance) { minDistanceSkill = distance; }
    public virtual void SetRestTime(float restTime) { }

}