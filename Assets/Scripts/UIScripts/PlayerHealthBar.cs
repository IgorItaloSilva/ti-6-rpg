using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    [SerializeField]float barWidthRatio = .35f;
    [SerializeField] float backgroundWidthRatio = .59f;
    [SerializeField]RectTransform background;
    float maxLife;
    public override void SettupBarMax(float maxLife)
    {
        this.maxLife=maxLife;
        base.SettupBarMax(maxLife);
        float magic = (maxLife - 1000) / 100 * 0.01f;
        background.sizeDelta = new Vector2((backgroundWidthRatio-magic)*maxLife,background.sizeDelta.y);
        rectTransformVerde.sizeDelta = new Vector2(barWidthRatio*maxLife,rectTransformVerde.sizeDelta.y);
        rectTransformColorido.sizeDelta = new Vector2(barWidthRatio*maxLife,rectTransformColorido.sizeDelta.y);
    }
     public void OnValidate()
    {
        background.sizeDelta = new Vector2(backgroundWidthRatio * maxLife, background.sizeDelta.y);
        rectTransformVerde.sizeDelta = new Vector2(barWidthRatio * maxLife, rectTransformVerde.sizeDelta.y);
        
    } 
}
