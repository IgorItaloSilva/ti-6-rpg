using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]GameObject boss;
    [SerializeField]GameObject statue;
    [SerializeField]LightBeamRoxoInteractable vfxRoxo;
    [SerializeField]int numberOfPillarsRequired;
    int numActivePillars;

    void OnEnable(){
        GameEventsManager.instance.levelEvents.onPillarActivated+=PillarActivated;
    }
    void OnDisable(){
        GameEventsManager.instance.levelEvents.onPillarActivated-=PillarActivated;
    }
    void PillarActivated(){
        numActivePillars++;
        if(numActivePillars>=numberOfPillarsRequired)
            SpawnBoss();
    }
    void SpawnBoss(){
        statue?.SetActive(false);
        boss?.SetActive(true);
        vfxRoxo?.Activate();
    }
}
