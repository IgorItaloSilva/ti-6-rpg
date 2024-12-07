using System;
using UnityEngine;

[CreateAssetMenu(fileName = "newRune",menuName = "ScriptableObjects/Rune",order =3)]
public class RuneSO : ScriptableObject{
[field:SerializeField]public int ID{get;private set;}
[field:SerializeField]public string Nome{get;private set;}
[field:SerializeField]public string DescriptionText{get;private set;}
[field:SerializeField]public Sprite Sprite{get;private set;}
[field:SerializeField]public Enums.KatanaPart Part{get;private set;}
[field:SerializeField]public Enums.ItemQuality Quality{get;private set;}
[field:SerializeField]public Enums.RuneActivationCode runeActivationCode{get;private set;}
}
