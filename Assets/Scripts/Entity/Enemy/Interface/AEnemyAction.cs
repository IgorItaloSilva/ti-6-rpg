using UnityEngine;


public abstract class AEnemyAction
{
    protected AEnemyBehave enemyBehave;
    protected EnemyController enemyController; // Script que est� executando esta a��o
    protected Transform target;
    protected Rigidbody rb; // Rigidbody do personagem atual
    protected bool antecipation, climax; // Estagios da a��o
    protected bool canExit = false;

    public AEnemyAction(AEnemyBehave enemyBehave)
    {
        this.enemyBehave = enemyBehave;
    }


    public virtual void StartAction(EnemyController _enemyController) // Metodo inicial da a��o
    {
        enemyController = _enemyController; // Declara variavel do personagem
        target = enemyController.GetTarget();
        rb = enemyController.GetRB(); // Declara Rigidbody do personagem
        antecipation = true;
    }

    // ------
    public abstract void UpdateAction(); // O que acontece a cada frame da a��o

    protected void ExitAction(AEnemyAction action) // Conclus�o da a��o
    {
        enemyController.SetAction(action);
    } 

    public virtual void ActionWithAnimator() { }

    public virtual void CanExit() { }
    public virtual void EndAnimation() { }

}