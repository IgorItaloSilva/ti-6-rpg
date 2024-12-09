using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RunePickupable : Interactable
{
    [SerializeField]RuneSO rune;
    
    void OnTriggerEnter(Collider outro){
        if(outro.CompareTag("Player")){
            RuneManager.instance?.GainRune(rune);
            Destroy(gameObject);
        }
    }
}
