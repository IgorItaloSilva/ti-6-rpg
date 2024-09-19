using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]private Slider lifeSlider;
    [SerializeField]private Image saveIcon;
    void OnEnable(){
        GameEventsManager.instance.uiEvents.onUpdateSliders+=UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange+=UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame+=FeedBackSave;
    }
    void OnDisable(){
        GameEventsManager.instance.uiEvents.onUpdateSliders-=UpdateSliders;
        GameEventsManager.instance.uiEvents.onLifeChange-=UpdateHealth;
        GameEventsManager.instance.uiEvents.onSavedGame-=FeedBackSave;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateHealth(float vidaAtual){
        if(lifeSlider!=null){
            lifeSlider.value=vidaAtual;
        }
    }
    private void UpdateSliders(int id,float minValue,float maxValue){
        switch(id){
            case 1:
                if(lifeSlider!=null){
                    lifeSlider.minValue=minValue;
                    lifeSlider.maxValue=maxValue;
                }
            break;
            default: return;
        }
    }
    private void FeedBackSave(){
        if(saveIcon!=null){
            saveIcon.gameObject.SetActive(true);
            StartCoroutine(SpinSaveIcon());
        }
    }
    IEnumerator SpinSaveIcon(){
        float timerTotal=5f;
        while(timerTotal>0){
            timerTotal-=Time.unscaledDeltaTime;
            saveIcon.rectTransform.Rotate(Vector3.forward,-5);
            yield return null;
        }
        saveIcon.gameObject.SetActive(false);
    }
}
