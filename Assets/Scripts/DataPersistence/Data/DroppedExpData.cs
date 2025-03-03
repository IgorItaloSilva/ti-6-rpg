using System;
using UnityEngine;

[Serializable]
public class DroppedExpData
{
    public int expAmmount;
    public Vector3 pos;
    public bool isActive;

    public DroppedExpData(){
        expAmmount = 0;
        pos=Vector3.zero;
        isActive=false;
    }
}
