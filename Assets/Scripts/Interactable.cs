using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour
{
    protected SphereCollider sphereCollider;
    [SerializeField] protected UnityEvent myEvent;
    void Start(){
        sphereCollider=GetComponent<SphereCollider>();
        sphereCollider.isTrigger=true;
    }
    
}
