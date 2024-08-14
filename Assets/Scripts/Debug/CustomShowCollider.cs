using System.Runtime.CompilerServices;
using UnityEngine;


enum TypeGizmo
{
    OnSelection,
    Anytime
}
public class CustomShowCollider : MonoBehaviour
{
    [SerializeField] Color color;
    [SerializeField] TypeGizmo type;


    private void OnDrawGizmos()
    {
        if(type == TypeGizmo.OnSelection) { return; }
        try
        {
            if (GetComponent<SphereCollider>())
            {
                SphereCollider collider = GetComponent<SphereCollider>();
                Gizmos.color = color;
                Gizmos.DrawSphere(collider.center, collider.radius);
            }
            else
            {
                BoxCollider collider = GetComponent<BoxCollider>();
                Gizmos.color = color;
                Gizmos.DrawCube(collider.center, GetComponent<BoxCollider>().size);
            }
        }
        catch
        {
            Debug.Log("Collider not found!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (type == TypeGizmo.Anytime) { return; }
        try
        {
            if (GetComponent<SphereCollider>())
            {
                SphereCollider collider = GetComponent<SphereCollider>();
                Gizmos.color = color;
                Gizmos.DrawSphere(collider.center, collider.radius);
            }
            else
            {
                BoxCollider collider = GetComponent<BoxCollider>();
                Gizmos.color = color;
                Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
            }
        }
        catch
        {
            Debug.Log("Collider not found!");
        }
    }

}
