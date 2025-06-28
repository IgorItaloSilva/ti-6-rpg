using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillNodeUI : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    [field:SerializeField]public PowerUpSO powerUp {get; private set;}
    [field:SerializeField]public Button button{get; private set;}
    [field:SerializeField]public GameObject powerUpLockedGO{get; private set;}
    [field:SerializeField]public GameObject powerUpBoughtGO{get; private set;}
    [field:SerializeField]public GameObject powerUpCanBuyCover{get; private set;}
    [field:SerializeField]public GameObject powerUpBaseOutline{get; private set;}

    public void OnPointerEnter(PointerEventData eventData)
    {
        SkillTreeUIManager.instance.ActivatePowerUpDescriptionBox(powerUp.Id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SkillTreeUIManager.instance.DeactivatePowerUpDescriptionBox();
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
}
