using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class SkillTreeUIManager : MonoBehaviour
{
    public static SkillTreeUIManager instance;
    [Header("Painel Skill Tree")]
    [SerializeField]private GameObject painelSkillTree;
    [SerializeField] private Color corCinza;
    [Header("Texto Das Moedas")]
    [SerializeField]String textCoinHonor;
    [SerializeField]String textCoinCorruption;
    [SerializeField]private TextMeshProUGUI tmpCoinsHonor;
    [SerializeField]private TextMeshProUGUI tmpCoinsCorruption;
    [Header("Caixinha Descricao e Nome")]
    [SerializeField]GameObject powerUpUIBox;
    [SerializeField]TextMeshProUGUI powerUpDescriptionText;
    [SerializeField]TextMeshProUGUI powerUpNameText;    
    [SerializeField]GameObject puBoxMoedaHonra;
    [SerializeField]GameObject puBoxMoedaTrevas;
    [SerializeField]float pixelOffset = 10;
    [SerializeField]SkillNodeUI[] powerUpNodes; //O NODE PRECISA SER FEITO EM ORDER PELO ID, não precisaria ser o node, mas caso precise já esta aqui
    bool powerUpBoxIsOpen = false;
    bool painelSkillTreeIsOpen;
    private bool[]boughtPowerUps;
    private bool[]buyablePowerUps;
    private int[]currentMoney;

    private void Start(){
        if(instance==null){
            instance=this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        AjustUiOnStart();
        int quantidadePowerUps = powerUpNodes.Count();
        /* ESSES MANOS VÃO SER CRIADOS NO LOAD DO SKILL TREE, QUE OCORRE ANTES DO START
        buyablePowerUps = new bool[quantidadePowerUps];
        boughtPowerUps = new bool[quantidadePowerUps];
        currentMoney = new int[Enum.GetNames(typeof(Enums.PowerUpType)).Length]; */
    }
    public void OnEnable(){
        GameEventsManager.instance.skillTreeEvents.onUnlockBuy+=AjustBuyable;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp+=AjustBuy;
        GameEventsManager.instance.uiEvents.onSkillTreeMoneyChange+=ChangeMoney;
    }
    void OnDisable(){
        GameEventsManager.instance.skillTreeEvents.onUnlockBuy-=AjustBuyable;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp-=AjustBuy;
        GameEventsManager.instance.uiEvents.onSkillTreeMoneyChange-=ChangeMoney;
    }
    public void ActivatePowerUpDescriptionBox(int id)
    {
        if (id >= 0)
        {
            if (id == 9)
            {
                if (!boughtPowerUps[9])
                {
                    powerUpUIBox.SetActive(true);
                    Vector2 pos = Mouse.current.position.ReadValue() + new Vector2(pixelOffset, pixelOffset);
                    powerUpUIBox.GetComponent<RectTransform>().SetPositionAndRotation(pos, Quaternion.identity);
                    powerUpNameText.text = "Magia de Fogo";
                    powerUpDescriptionText.text = "Esse poder é desbloqueado de outra forma.";
                    puBoxMoedaTrevas.SetActive(false);
                    puBoxMoedaHonra.SetActive(false);
                    powerUpBoxIsOpen = true;
                }
                else
                {
                    SkillNodeUI node = powerUpNodes[id];
                    powerUpUIBox.SetActive(true);
                    Vector2 pos = Mouse.current.position.ReadValue() + new Vector2(pixelOffset, pixelOffset);
                    powerUpUIBox.GetComponent<RectTransform>().SetPositionAndRotation(pos, Quaternion.identity);
                    powerUpNameText.text = node.powerUp.Name;
                    powerUpDescriptionText.text = node.powerUp.UiDescription;
                    powerUpBoxIsOpen = true;
                    puBoxMoedaHonra.SetActive(false);
                    puBoxMoedaTrevas.SetActive(false);
                }
            }else
            if (!powerUpBoxIsOpen)
            {
                SkillNodeUI node = powerUpNodes[id];
                powerUpUIBox.SetActive(true);
                Vector2 pos = Mouse.current.position.ReadValue() + new Vector2(pixelOffset, pixelOffset);
                powerUpUIBox.GetComponent<RectTransform>().SetPositionAndRotation(pos, Quaternion.identity);
                powerUpNameText.text = node.powerUp.Name;
                powerUpDescriptionText.text = node.powerUp.UiDescription;
                powerUpBoxIsOpen = true;
                if (node.powerUp.PUType == Enums.PowerUpType.Light)
                {
                    puBoxMoedaHonra.SetActive(true);
                    puBoxMoedaTrevas.SetActive(false);
                }
                else
                {
                    puBoxMoedaHonra.SetActive(false);
                    puBoxMoedaTrevas.SetActive(true);
                }
            }
        }
        else//MIGUE PRA TER TOOLTIP NAS MOEDAS
        {
            if (id == -1)//HONRA
            {
                powerUpUIBox.SetActive(true);
                Vector2 pos = Mouse.current.position.ReadValue() + new Vector2(pixelOffset, pixelOffset);
                powerUpUIBox.GetComponent<RectTransform>().SetPositionAndRotation(pos, Quaternion.identity);
                powerUpNameText.text = "Moeda Meyio";
                powerUpDescriptionText.text = "Usada para comprar talentos. Recebida por fazer ações boas.";
                puBoxMoedaHonra.SetActive(true);
                puBoxMoedaTrevas.SetActive(false);
                powerUpBoxIsOpen = true;
            }
            if (id == -2)//Corrupção
            {
                powerUpUIBox.SetActive(true);
                Vector2 pos = Mouse.current.position.ReadValue() + new Vector2(pixelOffset, pixelOffset);
                powerUpUIBox.GetComponent<RectTransform>().SetPositionAndRotation(pos - new Vector2(400, 0), Quaternion.identity);
                powerUpNameText.text = "Moeda Fuhai";
                powerUpDescriptionText.text = "Usada para comprar talentos. Recebida por fazer ações más.";
                puBoxMoedaTrevas.SetActive(true);
                puBoxMoedaHonra.SetActive(false);
                powerUpBoxIsOpen = true;
            }
        }
    }
    public void DeactivatePowerUpDescriptionBox(){
        powerUpUIBox.SetActive(false);
        powerUpBoxIsOpen=false;
    }
    public void AjustUiOnStart(){
    if(!powerUpUIBox){
            Debug.LogWarning("O nosso SkilltreeManagerUi não tem referencia da caixa do powerUp");
        }
        else{
            DeactivatePowerUpDescriptionBox();
        }
        if(!painelSkillTree){
            Debug.LogWarning("O nosso SkilltreeManagerUi não tem referencia ao painel de skill");
        }
        else{
            painelSkillTree.SetActive(false);
            painelSkillTreeIsOpen=false;
        }
    }
    public void AlternarPainelSkillTree(){//Abre e fecha o painel
        if(painelSkillTreeIsOpen){
            painelSkillTree.SetActive(false);
            painelSkillTreeIsOpen=false;
            DeactivatePowerUpDescriptionBox();
        }
        else{
            AjustButtonsSprites();
            AjustText();
            painelSkillTree.SetActive(true);
            painelSkillTreeIsOpen=true;
        }
    }
    public void AjustButtonsSprites(){
        foreach(SkillNodeUI skillNode in powerUpNodes){
            int id = skillNode.powerUp.Id;
            //Debug.Log($"id do power up id é {id}");
            if(buyablePowerUps==null)Debug.LogWarning("o buyablePowerUps não existe");
            if (buyablePowerUps[id])
            {
                skillNode.button.interactable = true;
                skillNode.powerUpLockedGO.SetActive(false);
                skillNode.powerUpCanBuyCover.SetActive(true);
            }
            else
            {
                skillNode.button.interactable = false;
                skillNode.powerUpLockedGO.SetActive(true);
                skillNode.powerUpCanBuyCover.SetActive(true);
            }
            if (boughtPowerUps[id])
            {
                powerUpNodes[id].button.interactable = true;
                skillNode.powerUpBoughtGO.SetActive(true);
                skillNode.powerUpBaseOutline.SetActive(false);
                skillNode.powerUpCanBuyCover.SetActive(false);
                skillNode.powerUpLockedGO.SetActive(false);
            }
            else
            {
                skillNode.powerUpBoughtGO.SetActive(false);
                skillNode.powerUpBaseOutline.SetActive(true);
                skillNode.powerUpCanBuyCover.SetActive(true);
            }
        }
    }
    public void AjustBuy(int id)
    {
        if (boughtPowerUps == null)
        {
            boughtPowerUps = new bool[powerUpNodes.Count()];
        }
        boughtPowerUps[id] = true;
        powerUpNodes[id].button.interactable = true;
        powerUpNodes[id].powerUpBaseOutline.SetActive(false);
        powerUpNodes[id].powerUpBoughtGO.SetActive(true);
        powerUpNodes[id].powerUpLockedGO.SetActive(false);
        powerUpNodes[id].powerUpCanBuyCover.SetActive(false);
    }
    public void AjustBuyable(int id)
    {
        if (buyablePowerUps == null)
        {
            buyablePowerUps = new bool[powerUpNodes.Count()];
        }
        buyablePowerUps[id] = true;
        powerUpNodes[id].button.interactable = true;
        powerUpNodes[id].powerUpLockedGO.SetActive(false);
    }
    
    public void AjustText(){
        tmpCoinsHonor.text = textCoinHonor + " " + currentMoney[0].ToString();
        tmpCoinsCorruption.text = textCoinCorruption + " " + currentMoney[1].ToString();
    }
    private void ChangeMoney(int index,int value){
        if(currentMoney==null){
            currentMoney=new int[Enum.GetNames(typeof(Enums.PowerUpType)).Length];
        }
        currentMoney[index]=value;
        AjustText();
    } 
}
