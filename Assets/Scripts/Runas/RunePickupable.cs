using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RunePickupable : Interactable
{
    [SerializeField]RuneSO rune;

    override protected void OnTriggerEnter(Collider outro){
        if(outro.CompareTag("Player")){
            RuneManager.instance?.GainRune(rune,true);
            AlreadyInterated=true;
            Active=false;
            Save();
            gameObject.SetActive(false);
        }
    }
    public override void Load(InteractableData interactableData)
    {
        base.Load(interactableData);
        //Debug.Log("Passmaos da chamada de base maas to aqui ainda");
        if(Active==false)gameObject.SetActive(false);
    }
}
