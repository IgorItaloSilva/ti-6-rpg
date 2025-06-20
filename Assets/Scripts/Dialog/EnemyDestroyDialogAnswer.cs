using UnityEngine;

public class EnemyDestroyDialogAnswer : DialogAnswer
{
    /*
        A ideia é fazer esse script ser passado para o scritable object dialog,
        esse script aqui ficaria dentro do dialog do inimigo ao algo assim
    */
    [HideInInspector]public EnemyBehaviour myEnemyBehaviour;//definido pelo dialogchoiceAux

    public override void Option1()
    {
        //Destruir o inimigo
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Dark);
        myEnemyBehaviour?.ActualDeath();
    }

    public override void Option2()
    {
        //Purificar o inimigo
        //n deveria ser o actual death né, deveria ter outro death que da metade do exp ou
        //algo assim
        SkillTree.instance?.GainMoney((int)Enums.PowerUpType.Light);
        myEnemyBehaviour?.ActualDeath();
    }

    public override void Option3()
    {
        //n tem essa opção
    }
}
