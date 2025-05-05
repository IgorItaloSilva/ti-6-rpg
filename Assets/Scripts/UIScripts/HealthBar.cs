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
    [SerializeField] Slider sliderPoise;
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

    #region Setup health Bar
    virtual public void SettupBarMax(float maxLife, float maxPoise){

        sliderVerde.value = sliderVerde.maxValue = maxLife;
        sliderColorido.value = sliderColorido.maxValue = maxLife;
        sliderPoise.value = sliderPoise.maxValue = maxPoise;
    }

    virtual public void SettupBarMax(float maxLife){
        sliderVerde.value = sliderVerde.maxValue = maxLife;
        sliderColorido.value = sliderColorido.maxValue = maxLife;
    }
    #endregion

    #region Set Value
    public void SetValue(float currentValue, float currentPoise, bool wasCrit){
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
        sliderPoise.value = currentPoise;
    }

    public void SetValue(float currentValue, bool wasCrit){
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
    #endregion

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

    public void OnDeath(){
        Destroy(gameObject);
    }
}