using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillNodeUI : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [field:SerializeField]public PowerUpSO powerUp {get; private set;}
    [SerializeField]Button button;
    [SerializeField]Sprite powerUpBoughtSprite;
    bool powerUpWasBought;
    bool powerUpCanBeBought;
    void Start()
    {
        button = GetComponent<Button>();
        button.interactable=false;
        if(powerUpCanBeBought){
            button.interactable=true;
        }
        if(powerUpWasBought){
            button.interactable=true;
            button.image.sprite = powerUpBoughtSprite;
        }
    }
    void OnEnable(){
        GameEventsManager.instance.skillTreeEvents.onUnlockBuy+=UnlockBuy;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp+=ActivatePowerUp;
    }
    void OnDisable(){
        GameEventsManager.instance.skillTreeEvents.onUnlockBuy-=UnlockBuy;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp-=ActivatePowerUp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.instance.ActivatePowerUpDescriptionBox(powerUp.Id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.DeactivatePowerUpDescriptionBox();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //feedback click
        if(SkillTree.instance.BuyPowerUp(powerUp.Id)){
            //feedback sucesso 
        }
        else{
            //feedback falha
        }
    }
    public void ActivatePowerUp(int id){
        if(id == powerUp.Id){
            if(!button){
                powerUpWasBought=true;
            }
            else{
                button.interactable=true;
                button.image.sprite = powerUpBoughtSprite;
            }
        }
    }

    public void UnlockBuy(int id){
        if(id == powerUp.Id){
            if(!button)
                powerUpCanBeBought=true;
            else
                button.interactable=true;
        }
    }
}
