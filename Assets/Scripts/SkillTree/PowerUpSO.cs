using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PowerUp",menuName = "ScriptableObjects/PowerUp",order = 3)]
public class PowerUpSO : ScriptableObject
{
    [field:SerializeField]
    public int Id {get; private set;}
    [field:SerializeField]
    public string Name {get; private set;}
    [field:SerializeField]
    public Enums.PowerUpType PUType {get; private set;}
    [field:SerializeField]
    public string UiDescription {get; private set;}
    [field:SerializeField]
    private string DescricaoInterna; // Só pra eu poder implementar depois
    [field:SerializeField]
    public List<PowerUpSO> children {get; private set;}
    
}

