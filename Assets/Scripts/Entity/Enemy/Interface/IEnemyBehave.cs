using UnityEngine;

public interface IEnemyBehave
{
    public bool CheckDistance(); // Checar distancia entre player e personagem
    public void IsDistant(); // Caso esteja distante do jogador
    public void IsClose(); // Caso esteja perto do jogador

    public void ChangeActionStage(); // Mudan�a de estagio da a��o

    public Rigidbody GetRB(); // retorna Rigidbody
}
