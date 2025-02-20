using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class StatsTests
{
    /*
    Devido a natureza do metodo de load os valores iniciais não podem
    ser definidos para a classe de Stats, seus valores base serão testados 
    depois em testes de integração.
    Esses testes unitários serevem para conferir o funcionamento das funções
    auxiliares
    */
    [OneTimeSetUp]
    /*
    O singleton do sistema de eventos do jogo é chamado inumeras vezes ao
    longo do código, portanto vamos precisar de um para não receber 
    erros de null object.
    */
    public void Settup(){
        GameObject eventsManager = new GameObject();
        eventsManager.AddComponent<GameEventsManager>();
    }
    [UnityTest]
    /*
    A função TakeDamage eventualmente terá um comportamento diferente
    dependendo do tipo do dano que é tomado. Portanto vamos testar os 2 tipos

    A função take damage tira o valor passado como paramentro da vida do jogador, 
    e caso a nova vida seja menor ou igual a zero chama a função de morrrer.
    */
    public IEnumerator TakeMagicDamage()
    {
        GameObject gameObject = new();
        PlayerStats stats = gameObject.AddComponent<PlayerStats>();
        float vida = stats.CurrentLife;
        stats.TakeDamage(50,Enums.DamageType.Magic,false);
        Assert.AreEqual(vida-50f,stats.CurrentLife);

        yield return null;
    }
    [UnityTest]
    public IEnumerator TakePhysicalDamage()
    {
        GameObject gameObject = new();
        PlayerStats stats = gameObject.AddComponent<PlayerStats>();
        float vida = stats.CurrentLife;
        stats.TakeDamage(50,Enums.DamageType.Regular,false);
        Assert.AreEqual(vida-50f,stats.CurrentLife);

        yield return null;
    }
    [UnityTest]
    public IEnumerator TakeDamageWithArmorPowerUp()
    {
        GameObject gameObject = new();
        PlayerStats stats = gameObject.AddComponent<PlayerStats>();
        float vida = stats.CurrentLife;
        GameEventsManager.instance.skillTreeEvents.ActivatePowerUp(3);
        //a função de receber AtviarPowerUp é privada, e está inscrita no 
        //observer que é chamado por esse evento "ActivatePowerUp"
        //O valor 3 é o ID hardcored definido no scriptable object do power up de armadura
        stats.TakeDamage(50,Enums.DamageType.Regular,false);
        Assert.AreEqual(vida-50f/2,stats.CurrentLife);
        //o power up deveria reduzir o dano tomado ao meio

        yield return null;
    }
    [UnityTest]
    /*
    Testa a capacidade do player de ganhar exp adaquadamente
    */
    public IEnumerator GainingExp(){
        GameObject gameObject = new();
        PlayerStats stats = gameObject.AddComponent<PlayerStats>();
        int currentExp=stats.Exp;
        GameEventsManager.instance.playerEvents.PlayerGainExp(50);
        //a função de receber exp é privada, e está inscrita no 
        //observer que é chamado por esse evento "PlayerGainExp"
        Assert.AreEqual(currentExp+50,stats.Exp);
        yield return null;
    }
    [UnityTest]
    /*
    Testa a capacidade do player de ganhar level adaquadamente. Ganhar um
    nível depende de duas outras funções, a gain exp, que está sendo testada
    nesse codigo, e a expToNextLevel, que foi copiada aqui também
    */
    public IEnumerator Gaininglevel(){
        GameObject gameObject = new();
        PlayerStats stats = gameObject.AddComponent<PlayerStats>();
        //Como não podemos iniciar com o valor correto, o nivel começa
        //no 0, ao invés do 1, então estamos dando 1 de exp para forçar um level up
        //para poder simular o estado inicial do codigo
        GameEventsManager.instance.playerEvents.PlayerGainExp(1);
        int currentLevel = stats.Level;
        int expToNextLevel = ExpToNextLevel(currentLevel);
        GameEventsManager.instance.playerEvents.PlayerGainExp(expToNextLevel);
        Assert.AreEqual(stats.Level,currentLevel+1);
        yield return null;
    }
    /*
    Função auxiliar para calcular a quantidade de exp necessária para o
    proximo nivel. Ela é privada na classe original, então ela foi
    copiada e colada para ca. Mudanças na formula precisam ser alteradas
    manualmente aqui
    */
    int ExpToNextLevel(int level){
        if(level==0){
            return 0;
        }
        else return 100*(int)Mathf.Pow(2,level-1);
    }
}
