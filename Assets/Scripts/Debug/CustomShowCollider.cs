using UnityEngine;


enum TypeGizmo
{
    OnSelection,
    Anytime
}
public class CustomShowCollider : MonoBehaviour
{
    [SerializeField] Color mainColor = Color.white;
    [SerializeField] TypeGizmo type;


    //Variaveis de controle
    bool touching;


    private void OnDrawGizmos()
    {
        if(type == TypeGizmo.OnSelection) { return; }

        Color currentColor;
        if (touching) currentColor = Color.red;
        else currentColor = mainColor;

        try
        {
            if (GetComponent<SphereCollider>())
            {
                SphereCollider collider = GetComponent<SphereCollider>();
                Gizmos.color = currentColor;
                Gizmos.DrawWireSphere(transform.position + collider.center, collider.radius);
            }
            else
            {
                BoxCollider collider = GetComponent<BoxCollider>();
                Gizmos.color = currentColor;
                Gizmos.DrawWireCube(transform.position + collider.center, GetComponent<BoxCollider>().size);
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

        Color currentColor;
        if (touching) currentColor = Color.red;
        else currentColor = mainColor;

        try
        {
            if (GetComponent<SphereCollider>())
            {
                SphereCollider collider = GetComponent<SphereCollider>();
                Gizmos.color = currentColor;
                Gizmos.DrawWireSphere(collider.center, collider.radius);
            }
            else
            {
                BoxCollider collider = GetComponent<BoxCollider>();
                Gizmos.color = currentColor;
                Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
            }
        }
        catch
        {
            Debug.Log("Collider not found!");
        }
    }

    public void IsTouching(bool value)
    {
        touching = value;
    }

}
