using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
public class RuneManager : MonoBehaviour,IDataPersistence
{
    public static RuneManager instance;
    public RuneSO[] equipedRunes{get;private set;}
    RuneSO[] unequipedRunes;
    public List<RuneSO> runeInventory{get;private set;}
    Dictionary<string,RuneSO> auxSaveLoadDictionary;
    bool[] hasChanged;
    [Header("COLOCAR TODAS AS RUNAS AQUI!")]
    [SerializeField]List<RuneSO>BibliotecaDeRunas;
    public bool showRuneDebug;

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
        runeInventory = new List<RuneSO>();
        int size = Enum.GetNames(typeof(Enums.KatanaPart)).Length;
        equipedRunes = new RuneSO[size];
        unequipedRunes = new RuneSO[size];
        hasChanged = new bool[size];
        auxSaveLoadDictionary=new();
        foreach(RuneSO rune in BibliotecaDeRunas){
            if(auxSaveLoadDictionary.ContainsKey(rune.saveLoadId)){
                Debug.LogWarning("Tentamos adicionar um runa no auxDictionary que já está nele. " +
                "Ou tem a mesma runa 2 vezes na BibliotecaDeRunas do RuneManager, ou tem 2 runas cujo scriptableObject tem o mesmo nome. "+
                $"A chave que gerou esse erro é {rune.saveLoadId}");
            }
            else{
                auxSaveLoadDictionary.Add(rune.saveLoadId,rune);
            }
        }
    }
    public void GainRune(RuneSO newRune, bool displayNotification)
    {
        if (displayNotification) UIManager.instance?.PlayNotification("Você coletou a runa: " + newRune.Nome);
        runeInventory.Add(newRune);
        DataPersistenceManager.instance?.SaveGame();
    }
    public void EquipRune(int index){//Nossa Ui passa o index do array de runas dela, que é igual ao nosso, por isso usado o ID diretamente
        if(showRuneDebug)Debug.Log($"Vou equipar a runa de {index}, que é a {runeInventory[index]}");
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
    public void UnequipRune(int index){
        if(showRuneDebug)Debug.Log($"Vou desequipar a runa de {index}, que é a {runeInventory[index]}");
        RuneSO rune = runeInventory[index];
        if(equipedRunes[(int)rune.Part]==null){
            if(showRuneDebug)Debug.LogError($"tentaram tirar uma runa e o rune manager n tem uma runa equipada no slot pedido {rune.Part}");
        }
        else{
            equipedRunes[(int)rune.Part]=null;
            unequipedRunes[(int)rune.Part]=runeInventory[index];
        }
        hasChanged[(int)rune.Part]=true;
    }
    public void ApplySelectedRunes(){//Called when we close the UI, applyes the buffs from the selected Runes
        for(int i =0;i<hasChanged.Length;i++){
            if (hasChanged[i])
            {
                //Deactivate a que estava esquipada
                RuneSO rune = unequipedRunes[i];
                if (rune != null)
                {
                    if (showRuneDebug) Debug.Log($"Estou dando deapply na runa {rune}");
                    ActivateDeactivateRune(rune, false);
                    unequipedRunes[i] = null;
                }
                //Activate a nova esquipada
                rune = equipedRunes[i];
                if (rune != null)
                {
                    if (showRuneDebug) Debug.Log($"Estou dando apply na runa {rune}");
                    ActivateDeactivateRune(rune, true);
                }
                hasChanged[i] = false;
                DataPersistenceManager.instance?.SaveGame();
            }
        }
    }
    void ActivateDeactivateRune(RuneSO rune,bool isActivate){
        if(showRuneDebug)Debug.Log($"Vou ativar a runa {rune.name}");
        bool parseResult;
        switch(rune.runeActivationCode){
            case Enums.RuneActivationCode.DamageBuff:
            {
                int dano = 0;
                parseResult = false;
                string[] strings = rune.DescriptionText.Split(" ");
                if(showRuneDebug)Debug.Log(strings.Length);
                foreach(string s in strings){
                    parseResult = int.TryParse(s,NumberStyles.AllowLeadingSign,CultureInfo.CurrentCulture,out dano);
                    if(parseResult)break;
                }
                if(showRuneDebug)Debug.Log($"resultado do parse {parseResult} e dano é {dano}");
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
                if(showRuneDebug)Debug.Log(strings.Length);
                foreach(string s in strings){

                    if(!parseResult)parseResult = int.TryParse(s,NumberStyles.AllowLeadingSign,CultureInfo.CurrentCulture,out amount);
                    if(s.Equals("vitalidade",StringComparison.OrdinalIgnoreCase)){stat ="vitalidade"; }
                    if(s.Equals("destreza",StringComparison.OrdinalIgnoreCase)){stat ="destreza"; }
                    if(s.Equals("força",StringComparison.OrdinalIgnoreCase)){stat ="força"; }
                    if(s.Equals("inteligência",StringComparison.OrdinalIgnoreCase)){stat ="inteligência"; }
                }
                if(!parseResult)if(showRuneDebug)Debug.Log($"O parse da runa não funcionou, não sabemos quanto do stat: {stat} devemos aumentar");
                if(showRuneDebug)Debug.Log($"resultado do parse {parseResult} e dano é {amount}");
                if(parseResult){
                    GameEventsManager.instance.runeEvents.RuneStatBuff(isActivate,stat,amount);
                }
            }
            break;
            case Enums.RuneActivationCode.TradeOff:
            break;
            case Enums.RuneActivationCode.OtherBonus:
            {
                int amount = 0;
                parseResult = false;
                string noPertentage = rune.DescriptionText.Replace('%',' ');
                string[] strings = noPertentage.Split(" ");
                if(showRuneDebug)Debug.Log(strings.Length);
                foreach(string s in strings){
                    //if(!parseResult)parseResult = int.TryParse(s,out amount);
                    if(!parseResult)parseResult = int.TryParse(s,NumberStyles.AllowLeadingSign,CultureInfo.CurrentCulture,out amount);
                }
                if(!parseResult)Debug.Log($"O parse da runa não funcionou, não sabemos quanto critico:");
                if(showRuneDebug)Debug.Log($"resultado do parse {parseResult} e critico é {amount}");
                if(parseResult){
                    GameEventsManager.instance.runeEvents.RuneOtherBuff(isActivate,Enums.RuneOtherCode.Critico,amount);
                }
            }
            break;
        }
    }

    public void LoadData(GameData gameData)
    {
        RuneSO auxRune;
        foreach(string runeName in gameData.runeData.collectedRunes){
            auxSaveLoadDictionary.TryGetValue(runeName,out auxRune);
            if(!runeInventory.Contains(auxRune))
                GainRune(auxRune,false);
        }
        foreach(string runeName in gameData.runeData.equipedRunes){
            auxSaveLoadDictionary.TryGetValue(runeName,out auxRune);
            int index=runeInventory.IndexOf(auxRune);
            if(showRuneDebug)Debug.Log($"index da runa {auxRune} é {index}");
            EquipRune(index);
            ApplySelectedRunes();
            //if(index>=0)RunesUiManager.instance.EquipRune(index);
            
        }
    }

    public void SaveData(GameData gameData)
    {
        RuneData newRuneData = new();
        foreach(RuneSO rune in runeInventory){
            newRuneData.collectedRunes.Add(rune.saveLoadId);
        }
        foreach(RuneSO rune in equipedRunes){
            if(rune!=null)
                newRuneData.equipedRunes.Add(rune.saveLoadId);
        }
        gameData.runeData=newRuneData;
    }
}
