using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RunePickupable : MonoBehaviour
{
    SphereCollider sphereCollider;
    [SerializeField]RuneSO rune;
    void Start(){
        sphereCollider=GetComponent<SphereCollider>();
        sphereCollider.isTrigger=true;
    }
    void OnTriggerEnter(Collider outro){
        if(outro.CompareTag("Player")){
            RuneManager.instance?.GainRune(rune);
            Destroy(gameObject);
        }
    }
}
