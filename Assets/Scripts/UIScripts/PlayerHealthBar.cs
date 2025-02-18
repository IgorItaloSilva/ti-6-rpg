using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : HealthBar
{
    [SerializeField]float barWidthRatio = .35f;
    public override void SettupBarMax(float maxLife)
    {
        base.SettupBarMax(maxLife);
        rectTransformVerde.sizeDelta = new Vector2(barWidthRatio*maxLife,rectTransformVerde.sizeDelta.y);
        rectTransformColorido.sizeDelta = new Vector2(barWidthRatio*maxLife,rectTransformColorido.sizeDelta.y);
    }
}
