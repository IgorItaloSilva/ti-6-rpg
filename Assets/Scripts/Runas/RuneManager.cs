using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    public static RuneManager instance;
    public RuneSO[] runasEquipadas = new RuneSO[Enum.GetNames(typeof(Enums.RuneType)).Length];
    public List<RuneSO> runeInventory = new List<RuneSO>();

    void OnEnable(){

    }
    void OnDisable(){

    }
    void OnAwake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(this.gameObject);
        }
    }
    void SwitchToRune(RuneSO rune){

    }
    void ApplySelectedRunes(){//Called when we close the UI, applyes the buffs from the selected Runes

    }
}
