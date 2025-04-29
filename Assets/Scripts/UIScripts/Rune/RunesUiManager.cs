using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RunesUiManager : MonoBehaviour
{
    public static RunesUiManager instance;
    [SerializeField]GameObject equipedBladeRuneSlot;
    [SerializeField]GameObject equipedGuardRuneSlot;
    [SerializeField]GameObject equipedHandleRuneSlot;
    [SerializeField]GameObject scrollContent;
    [SerializeField]GameObject runeButtonprefab;
    RuneButton[]equipedRunesButtons = new RuneButton[Enum.GetNames(typeof(Enums.KatanaPart)).Length];
    List<RuneButton> allRunesButtons = new();
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
        GameObject parent=null;
            switch(rune.Part){
                case Enums.KatanaPart.Blade:
                    parent=equipedBladeRuneSlot;
                break;
                case Enums.KatanaPart.Guard:
                    parent=equipedGuardRuneSlot;
                break;
                case Enums.KatanaPart.Handle:
                    parent=equipedHandleRuneSlot;
                break;
            }
        if(equipedRunesButtons[(int)rune.Part]!=null)Destroy(equipedRunesButtons[(int)rune.Part].gameObject);
        GameObject newRuneButtonGO=Instantiate(runeButtonprefab,parent.transform);
        RuneButton runeButton = newRuneButtonGO.GetComponent<RuneButton>();
        runeButton.SetRuneAndTexts(rune);
        runeButton.id=runeButtonindex;
        runeButton.isEquiped=true;
        equipedRunesButtons[(int)rune.Part]=runeButton;
        RuneManager.instance.EquipRune(runeButtonindex);
    }
    public void Unequip(int runeButtonindex){
        RuneSO rune = allRunesButtons[runeButtonindex].rune;
        Debug.Log($"A runa que vai ser desequipada é da part {rune.Part} e tem id de {runeButtonindex}");
        if(equipedRunesButtons[(int)rune.Part]==null)Debug.LogError("Uma runa que não está equipada está tentando desequipar a categoria dela???");
        Debug.Log($"Vou tentar destrui o rune button {rune.Part} que contem {equipedRunesButtons[(int)rune.Part]}");
        Destroy(equipedRunesButtons[(int)rune.Part].gameObject);
        equipedRunesButtons[(int)rune.Part]=null;
        RuneManager.instance.UnequipRune(runeButtonindex);
    }
}
