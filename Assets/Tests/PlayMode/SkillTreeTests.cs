using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SkillTreeTests {
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
}
