using System;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(SphereCollider))]
public abstract class ColliderActivateEvent : MonoBehaviour
{
    [SerializeField]protected UnityEvent myEvent;

    protected abstract void OnTriggerEnter(Collider collider);
}
