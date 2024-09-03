using UnityEngine;

public interface IEnemyBehave
{
    public bool CheckDistance(); // Checar distancia entre player e personagem
    public void IsDistant(); // Caso esteja distante do jogador
    public void IsClose(); // Caso esteja perto do jogador

    public void ChangeActionStage(); // Mudança de estagio da ação

    public Rigidbody GetRB(); // retorna Rigidbody
}
