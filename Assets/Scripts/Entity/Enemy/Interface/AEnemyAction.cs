using System;
using UnityEngine;


public abstract class AEnemyAction : MonoBehaviour 
{
    protected IEnemyBehave enemyBehaviour; // Script que está executando esta ação
    protected Rigidbody rb; // Rigidbody do personagem atual
    protected bool antecipation, climax, dissipation; // Estagios da ação
    protected float cooldownAction; // !!! ATENÇÃO: Definir o cooldown em algum momento da ação(Preferencialmente no StartAction)

    public virtual void StartAction(IEnemyBehave _enemyBehave) // Metodo inicial da ação
    {
        enemyBehaviour = _enemyBehave; // Declara variavel do personagem
        rb = enemyBehaviour.GetRB(); // Declara Rigidbody do personagem
        Antecipation(); // Inicia a ação do personagem pela antecipação
    }

    // ------
    public abstract void UpdateAction(); // O que acontece a cada frame da ação

    public virtual void Antecipation() { } // Estagio inicial
    public virtual void Climax() { } // estagio de frame ativo
    public virtual void Dissipation() { } // Final da ação

    public void ChangeStage() // Mudar o estágio da ação
    {
        if (antecipation)
        {

            climax = true;
        }else if (climax)
        {
            antecipation = false;
            climax = false;
            dissipation = true;
        }
    }

    // ------
    public abstract void ExitAction(AEnemyAction _nextAction); // Conclusão da ação

}