using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
public class RuneManager : MonoBehaviour
{
    public static RuneManager instance;
    public RuneSO[] equipedRunes;
    public RuneSO[] unequipedRunes;
    public List<RuneSO> runeInventory;
    bool[] hasChanged;

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
        equipedRunes = new RuneSO[Enum.GetNames(typeof(Enums.KatanaPart)).Length];
        unequipedRunes = new RuneSO[Enum.GetNames(typeof(Enums.KatanaPart)).Length];
        hasChanged = new bool[Enum.GetNames(typeof(Enums.KatanaPart)).Length];
    }
    public void GainRune(RuneSO newRune){
        GameEventsManager.instance.uiEvents.NotificationPlayed("Você coletou a rune: "+newRune.Nome);
        runeInventory.Add(newRune);
    }
    public void EquipRune(int index){//NOssa Ui passa o index do array de runas dela, que é igual ao nosso, por isso usado o ID diretamente
        Debug.Log($"Vou equipar a runa de {index}, que é a {runeInventory[index]}");
        RuneSO rune = runeInventory[index];
        if(equipedRunes[(int)rune.Part]==null){
            equipedRunes[(int)rune.Part]=runeInventory[index];
        }
        else{
            unequipedRunes[(int)rune.Part]=equipedRunes[(int)rune.Part];
            equipedRunes[(int)rune.Part]=runeInventory[index];
        }
        hasChanged[(int)rune.Part]=true;
    }
    public void ApplySelectedRunes(){//Called when we close the UI, applyes the buffs from the selected Runes
        for(int i =0;i<hasChanged.Length;i++){
            if(hasChanged[i]){
                //Deactivate a que estava esquipada
                RuneSO rune = unequipedRunes[i];
                if(rune!=null){
                    Debug.Log($"Estou dando deapply na runa {rune}");
                    ActivateDeactivateRune(rune,false);
                }
                //Activate a nova esquipada
                rune = equipedRunes[i];
                if(rune!=null){
                    Debug.Log($"Estou dando apply na runa {rune}");
                    ActivateDeactivateRune(rune,true);
                }
                hasChanged[i]=false;
            }
        }
    }
    void ActivateDeactivateRune(RuneSO rune,bool isActivate){
        Debug.Log($"Vou ativar a runa {rune.name}");
        bool parseResult;
        switch(rune.runeActivationCode){
            case Enums.RuneActivationCode.DamageBuff:
            {
                int dano = 0;
                parseResult = false;
                string[] strings = rune.DescriptionText.Split(" ");
                Debug.Log(strings.Length);
                foreach(string s in strings){
                    parseResult = int.TryParse(s,out dano);
                    if(parseResult)break;
                }
                Debug.Log($"resultado do parse {parseResult} e dano é {dano}");
                if(parseResult){
                    GameEventsManager.instance.runeEvents.RuneDamageBuff(isActivate,dano);
                }
            }
            break;
            case Enums.RuneActivationCode.StatsBuff:
            {
                string stat = "";
                int amount = 0;
                parseResult = false;
                string[] strings = rune.DescriptionText.Split(" ");
                Debug.Log(strings.Length);
                foreach(string s in strings){
                    Debug.Log(s);
                    if(!parseResult)parseResult = int.TryParse(s,out amount);
                    if(s.Equals("vitalidade",StringComparison.OrdinalIgnoreCase)){stat ="vitalidade"; }
                    if(s.Equals("destreza",StringComparison.OrdinalIgnoreCase)){stat ="destreza"; }
                    if(s.Equals("força",StringComparison.OrdinalIgnoreCase)){stat ="força"; }
                    if(s.Equals("inteligência",StringComparison.OrdinalIgnoreCase)){stat ="inteligência"; }
                }
                if(!parseResult)Debug.Log($"O parse da runa não funcionou, não sabemos quanto do stat: {stat} devemos aumentar");
                Debug.Log($"resultado do parse {parseResult} e dano é {amount}");
                if(parseResult){
                    GameEventsManager.instance.runeEvents.RuneStatBuff(isActivate,stat,amount);
                }
            }
            break;
            case Enums.RuneActivationCode.TradeOff:
            break;
            case Enums.RuneActivationCode.OtherBonus:
                
            break;
        }
    }
}
