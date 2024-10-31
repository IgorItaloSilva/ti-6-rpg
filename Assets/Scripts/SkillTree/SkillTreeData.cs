using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SkillTreeData 
{
    private const int NPOWERUPS = 15; //PRECISA SER AJUSTADO MANUALMENTE
    public bool[] boughtPowerUps;
    public int[] currentMoney;
    public int[] totalMoneyGotten;
    public SkillTreeData(){
        int tamanho = Enum.GetNames(typeof(Enums.PowerUpType)).Length;
        boughtPowerUps = new bool[NPOWERUPS];//inicializar arrays já coloca 0, que é o default
        boughtPowerUps[0]=true;
        currentMoney = new int[tamanho];
        totalMoneyGotten = new int[tamanho];
    }
}
