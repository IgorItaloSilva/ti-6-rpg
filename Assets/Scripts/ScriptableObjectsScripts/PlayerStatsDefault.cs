using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="DefaultPlayerStats",menuName = "ScriptableObjects/PlayerDefaltStats",order = 10)]
public class PlayerStatsDefaultSO : ScriptableObject
{
    public int baseCon;
    public int baseStr;
    public int baseDex;
    public int baseInt;
    public int baseExp;
    public int baseLevel;
    public float baseLife;
    public float baseMana;
    public float baseMagicDamage;
    public float baseLightAttackDamage;
    public float baseHeavyAttackDamage;
}
