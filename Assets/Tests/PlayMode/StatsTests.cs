using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


public class StatsTests
{
    [OneTimeSetUp]
    public void Settup(){
        GameObject eventsManager = new GameObject();
        eventsManager.AddComponent<GameEventsManager>();
    }
    [UnityTest]
    public IEnumerator StatsTestsWithEnumeratorPasses()
    {
        GameObject gameObject = new();
        PlayerStats stats = gameObject.AddComponent<PlayerStats>();
        float vida = stats.CurrentLife;
        stats.TakeDamage(50,Enums.DamageType.Magic);
        Assert.AreEqual(vida-50f,stats.CurrentLife);

        yield return null;
    }
}
