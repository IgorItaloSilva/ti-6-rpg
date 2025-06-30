using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SkillTree : MonoBehaviour,IDataPersistence
{
    public static SkillTree instance;
    private const int NPOWERUPS = 10; //PRECISA SER AJUSTADO MANUALMENTE
    [SerializeField] private List<PowerUpSO> powerUps;
    private int[] currentMoney;//o indice é o enum do tipo de powerUp
    public int[] totalMoneyGotten;//o indice é o enum do tipo de powerUp
    private bool[] boughtPowerUps; //o indice do vetor precisa ser igual o Id do power up
    private bool[]buyablePowerUps;
    private SkillTreeData dataLoadada;
    int tamanhoTiposPU;

    
    public void Start(){
        if(!instance)
            instance=this;
        else
            Destroy(gameObject);
        boughtPowerUps=new bool[NPOWERUPS];
        buyablePowerUps=new bool[NPOWERUPS];
        tamanhoTiposPU = Enum.GetNames(typeof(Enums.PowerUpType)).Length;
        currentMoney = new int[tamanhoTiposPU];
        totalMoneyGotten = new int[tamanhoTiposPU];
        ActivatePowerUp(9);
        LoadData();
    }
    void Update()
    {
        if (!GameManager.instance.cheatsEnabled) return;
        
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            GainMoney(0);
        }
        if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            GainMoney(1);
        }
        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            ActivatePowerUp(9);
        }
    }
    private void ActivatePowerUps(){
        for(int i=0; i < powerUps.Count; i++){
            if(boughtPowerUps[i]){
                ActivatePowerUp(i);
            }
        }
    }
    private void ActivatePowerUp(int id)
    {
        GameEventsManager.instance.skillTreeEvents.ActivatePowerUp(id);
        PlayerStateMachine.Instance?.UnlockSpecial(id);
    }
    public bool BuyPowerUp(int id){//Vai ser chamado por uma função da UI, que vai responder um click de um botão
        if (id == 9) return false;
        //Debug.Log($"Tentaram comprar o powerUp de Id {id}");
        Enums.PowerUpType powerUpType = powerUps[id].PUType;
        if(currentMoney[(int)powerUpType]<=0){
            //Debug.Log($"Sem dinheiro do tipo {powerUpType}");
            return false;
        }
        if(buyablePowerUps[id]&&!boughtPowerUps[id]){
            boughtPowerUps[id]=true;
            ActivatePowerUp(id);
            AjustBuyablePowerUps();
            currentMoney[(int)powerUpType]--;
            //Debug.Log($"Comprei o power up de Id {id}");
            GameEventsManager.instance.uiEvents.SkillTreeMoneyChange((int)powerUpType,currentMoney[(int)powerUpType]);
            DataPersistenceManager.instance?.SaveGame();
            return true;
        }
        return false;

    }
    public void AjustBuyablePowerUps(){
        for(int i=0; i < powerUps.Count; i++){
            if(boughtPowerUps[i]){
                foreach(PowerUpSO powerUpfilho in powerUps[i].children){
                    buyablePowerUps[powerUpfilho.Id]=true;
                    GameEventsManager.instance.skillTreeEvents.UnlockBuy(powerUpfilho.Id);
                }
            }
        }
    }
    public void GainMoney(int powerUpType){
        if(powerUpType>=0&&powerUpType<Enum.GetNames(typeof(Enums.PowerUpType)).Length){
            //Debug.Log($"Ganhei dinheiro do tipo {powerUpType}");
            currentMoney[powerUpType]++;
            totalMoneyGotten[powerUpType]++;
            if((Enums.PowerUpType)powerUpType==Enums.PowerUpType.Light)UIManager.instance?.PlayNotification("Sua honra lhe concedeu uma moeda Meiyo.");
            else UIManager.instance?.PlayNotification("Sua ambição lhe concedeu uma moeda Fuhai.");
            GameEventsManager.instance.uiEvents.SkillTreeMoneyChange(powerUpType,currentMoney[powerUpType]);
        }
    }

    public void LoadData(GameData gameData)
    {
        dataLoadada = gameData.skillTreeData;
        
    }
    public void LoadData(){
        for(int i=0;i<tamanhoTiposPU;i++){
            currentMoney[i]=dataLoadada.currentMoney[i];
            totalMoneyGotten[i] = dataLoadada.totalMoneyGotten[i];
        }
        for(int j=0;j<powerUps.Count;j++){
            boughtPowerUps[j]=dataLoadada.boughtPowerUps[j];
        }
        GameEventsManager.instance.uiEvents.SkillTreeMoneyChange(0,currentMoney[0]);
        GameEventsManager.instance.uiEvents.SkillTreeMoneyChange(1,currentMoney[1]);
        AjustBuyablePowerUps();
        ActivatePowerUps();
    }

    public void SaveData(GameData gameData)
    {
        for(int i=0;i<tamanhoTiposPU;i++){
            gameData.skillTreeData.currentMoney[i] =currentMoney[i];
            gameData.skillTreeData.totalMoneyGotten[i] = totalMoneyGotten[i];
        }
        for(int i=0;i<powerUps.Count;i++){
            gameData.skillTreeData.boughtPowerUps[i]=boughtPowerUps[i];
        }
    }
}
