using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpdateInfoSO", menuName = "ScriptableObjects/UpdateInfoSO")]
public class UpdateInfoSO : ScriptableObject
{
    [Header("Info do update")]
    [field:SerializeField]public String Version{get;private set;}
    [field:SerializeField]public int Dia{get;private set;}
    [field:SerializeField]public int Mes{get;private set;}
    [field:SerializeField]public int Ano{get;private set;}
    [Header("Texto com a contribuição de cada membro")]
    [field:SerializeField]public String ContributionsAlice{get;private set;}
    [field:SerializeField]public String ContributionsArthur{get;private set;}
    [field:SerializeField]public String ContributionsFelipe{get;private set;}
    [field:SerializeField]public String ContributionsGabriel{get;private set;}
    [field:SerializeField]public String ContributionsGustavo{get;private set;}
    [field:SerializeField]public String ContributionsHenrique{get;private set;}
    [field:SerializeField]public String ContributionsIgor{get;private set;}
    [field:SerializeField]public String ContributionsPedro{get;private set;}
    [field:SerializeField]public String ContributionsTiago{get;private set;}
}
