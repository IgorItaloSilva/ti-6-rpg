using UnityEngine;

public class TreeInteractable :  SkillPointInteractable

{
    protected override void Start()
    {
        base.Start();
        CanInteract=true;
    }
    protected override void Rescue()
    {
        base.Rescue();
        //toca um efeito magico de purificação
    }
    public override void Die()
    {
        base.Die();
        //desativa a arvore bonitinha
        //ativa a arvore destruida
    }
}
