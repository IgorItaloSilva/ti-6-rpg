using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemColetavel : ColliderActivateEvent
{
    [SerializeField]int itemID;
    protected override void OnTriggerEnter(Collider collider)
    {
        GameEventsManager.instance.mapaTiagoEvents.CollectedItem(itemID);
        Destroy(gameObject);
    }

}
