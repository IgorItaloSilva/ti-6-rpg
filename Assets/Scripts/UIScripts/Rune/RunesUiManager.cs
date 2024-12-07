using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RunesUiManager : MonoBehaviour
{
    public static RunesUiManager instance;
    [SerializeField]HorizontalLayoutGroup equipedRunesLayout;
    [SerializeField]GameObject scrollContent;
    [SerializeField]GameObject runeButtonprefab;
    RuneButton[]equipedRunes = new RuneButton[Enum.GetNames(typeof(Enums.RuneType)).Length];
    List<RuneButton> allRunesButtons = new();
    int equipedRunesAmmount;
    static int index;
    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        index=0;
        equipedRunesAmmount=0;
    }
    public void Setup(){
        foreach(RuneSO runeSO in RuneManager.instance.runeInventory ){
            GameObject newRuneButtonGO=Instantiate(runeButtonprefab,scrollContent.transform);
            RuneButton runeButton = newRuneButtonGO.GetComponent<RuneButton>();
            runeButton.SetRuneAndTexts(runeSO);
            runeButton.id=index;
            index++;
            allRunesButtons.Add(runeButton);
        }
    }
    public void UpdateRunes(){
        if(allRunesButtons.Count!=RuneManager.instance.runeInventory.Count){
            for(int i=allRunesButtons.Count;i<RuneManager.instance.runeInventory.Count;i++){
                GameObject newRuneButtonGO=Instantiate(runeButtonprefab,scrollContent.transform);
                RuneButton runeButton = newRuneButtonGO.GetComponent<RuneButton>();
                runeButton.SetRuneAndTexts(RuneManager.instance.runeInventory[i]);
                allRunesButtons.Add(runeButton);
                runeButton.id=index;
                index++;
            }
        }
    }
    public void EquipRune(int runeButtonindex){//esse parametro é o index que nos passamos pro runeButton ao criar ele
        RuneSO rune = allRunesButtons[runeButtonindex].rune;
        if(equipedRunes[(int)rune.Type]==null){
            GameObject newRuneButtonGO=Instantiate(runeButtonprefab,equipedRunesLayout.transform);
            RuneButton runeButton = newRuneButtonGO.GetComponent<RuneButton>();
            runeButton.SetRuneAndTexts(rune);
            equipedRunes[(int)rune.Type]=runeButton;
            equipedRunesAmmount++;
        }else{
            Destroy(equipedRunes[(int)rune.Type].gameObject);
            GameObject newRuneButtonGO=Instantiate(runeButtonprefab,equipedRunesLayout.transform);
            newRuneButtonGO.transform.SetSiblingIndex((int)rune.Type);
            RuneButton runeButton = newRuneButtonGO.GetComponent<RuneButton>();
            runeButton.SetRuneAndTexts(rune);
            equipedRunes[(int)rune.Type]=runeButton;
        }
        if(equipedRunesAmmount>=3){
            equipedRunesLayout.childControlWidth=true;
        }
        else{
            equipedRunesLayout.childControlWidth=false;
        }
    }
}
