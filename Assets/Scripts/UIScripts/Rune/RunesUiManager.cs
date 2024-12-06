using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunesUiManager : MonoBehaviour
{
    [SerializeField]GameObject scrollContent;
    [SerializeField]GameObject runeButtonprefab;
    RuneButton[]equipedRunes = new RuneButton[Enum.GetNames(typeof(Enums.RuneType)).Length];
    List<RuneButton> allRunesButtons = new();
    void Start()
    {
        Setup();
    }
    public void Setup(){
        foreach(RuneSO runeSO in RuneManager.instance.runeInventory ){
            GameObject newRuneButtonGO=Instantiate(runeButtonprefab,scrollContent.transform);
            RuneButton runeButton = newRuneButtonGO.GetComponent<RuneButton>();
            runeButton.SetRuneAndTexts(runeSO);
        }
    }
    public void Update(){
        if(allRunesButtons.Count!=RuneManager.instance.runeInventory.Count){
            for(int i=allRunesButtons.Count;i<RuneManager.instance.runeInventory.Count;i++){
                GameObject newRuneButtonGO=Instantiate(runeButtonprefab,scrollContent.transform);
                RuneButton runeButton = newRuneButtonGO.GetComponent<RuneButton>();
                runeButton.SetRuneAndTexts(RuneManager.instance.runeInventory[i]);
            }
        }
    }
}
