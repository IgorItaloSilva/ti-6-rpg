using System;
using System.Collections.Generic;
using UnityEngine;
public class RuneManager : MonoBehaviour
{
    public static RuneManager instance;
    public RuneSO[] equipedRunes;
    public List<RuneSO> runeInventory;

    void OnEnable(){

    }
    void OnDisable(){

    }
    void Awake(){
        if(instance==null){
            instance=this;
        }
        else{
            Destroy(this.gameObject);
        }
        //runeInventory = new List<RuneSO>();
        equipedRunes = new RuneSO[Enum.GetNames(typeof(Enums.RuneType)).Length];
    }
    public void GainRune(RuneSO newRune){
        runeInventory.Add(newRune);
    }
    void EquipRune(int id){
        equipedRunes[(int)runeInventory[id].Type]=runeInventory[id];
    }
    void ApplySelectedRunes(){//Called when we close the UI, applyes the buffs from the selected Runes
        foreach(RuneSO rune in equipedRunes){
            if(rune!=null){
                //ActivateRune(decidir entre id, ou a runa inteira);
            }
        }
    }
}
