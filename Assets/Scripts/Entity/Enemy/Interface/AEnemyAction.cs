using System;
using UnityEngine;


public abstract class AEnemyAction : MonoBehaviour 
{
    protected IEnemyBehave enemyBehaviour; // Script que est� executando esta a��o
    protected Rigidbody rb; // Rigidbody do personagem atual
    protected bool antecipation, climax, dissipation; // Estagios da a��o
    protected float cooldownAction; // !!! ATEN��O: Definir o cooldown em algum momento da a��o(Preferencialmente no StartAction)

    public virtual void StartAction(IEnemyBehave _enemyBehave) // Metodo inicial da a��o
    {
        enemyBehaviour = _enemyBehave; // Declara variavel do personagem
        rb = enemyBehaviour.GetRB(); // Declara Rigidbody do personagem
        Antecipation(); // Inicia a a��o do personagem pela antecipa��o
    }

    // ------
    public abstract void UpdateAction(); // O que acontece a cada frame da a��o

    public virtual void Antecipation() { } // Estagio inicial
    public virtual void Climax() { } // estagio de frame ativo
    public virtual void Dissipation() { } // Final da a��o

    public void ChangeStage() // Mudar o est�gio da a��o
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
    public abstract void ExitAction(AEnemyAction _nextAction); // Conclus�o da a��o

}