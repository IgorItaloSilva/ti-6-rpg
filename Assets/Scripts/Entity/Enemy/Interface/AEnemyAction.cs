using UnityEngine;


public abstract class AEnemyAction
{

    protected Rigidbody rb; // Rigidbody do personagem atual
    protected EnemyController enemyController; // Script que está executando esta ação
    protected Transform target;
    protected float minDistanceSkill;


    public virtual void StartAction(EnemyController enemyController) // Metodo inicial para ações de ataque (Definir na nova ação qual )
    {
        rb = enemyController.GetRB(); // Declara Rigidbody do personagem
        target = enemyController.GetTarget();
        this.enemyController = enemyController; // Declara variavel do personagem
    }

    // ------
    public abstract void UpdateAction(); // O que acontece a cada frame da ação

    public abstract void ExitAction(); // Conclusão da ação (Qual o index da habilidade para aplicar o cooldown)

    public virtual void ActionWithAnimator() { }

    public virtual void CanExit() { }
    public virtual void EndAnimation() { }

    public float GetMinDistanceSkill() { return minDistanceSkill; }

    public virtual void SetDistance(float distance) { minDistanceSkill = distance; }
    public virtual void SetRestTime(float restTime) { }

}