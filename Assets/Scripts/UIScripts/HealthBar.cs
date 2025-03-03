using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    protected RectTransform rectTransformVerde;
    protected RectTransform rectTransformColorido;
    [SerializeField]protected Slider sliderColorido;
    [SerializeField]protected Slider sliderVerde;
    [SerializeField]protected Image coloredSliderImage;
    [SerializeField]protected float timeToAjust = .5f;
    bool coroutineIsRunning;
    Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        rectTransformColorido=sliderColorido.GetComponent<RectTransform>();
        rectTransformVerde=sliderVerde.GetComponent<RectTransform>();
    }
    void Update()
    {

    }
    virtual public void SettupBarMax(float maxLife){
        sliderVerde.maxValue = maxLife;
        sliderColorido.maxValue = maxLife;
    }
    public void SetValue(float currentValue,bool wasCrit){
        if(currentValue<sliderVerde.value){
            sliderVerde.value=currentValue;
            if(coroutineIsRunning){
                if(coroutine!=null)StopCoroutine(coroutine);
            }
            coroutine=StartCoroutine(AjustColoredValue(wasCrit));
        }
        else{
            sliderVerde.value=currentValue;
            sliderColorido.value=currentValue;
        }
    }

    IEnumerator AjustColoredValue(bool wasCrit){
        coroutineIsRunning=true;
        float time = 0f;
        float startValue = sliderColorido.value;
        coloredSliderImage.color = Color.red;
        if(wasCrit){
            coloredSliderImage.color = Color.yellow;
        }
        while(time <= timeToAjust){
            time += Time.deltaTime;

            sliderColorido.value=Mathf.Lerp(startValue,sliderVerde.value,time/timeToAjust);
            yield return null;
        }
        coroutineIsRunning=false;
    }
}
