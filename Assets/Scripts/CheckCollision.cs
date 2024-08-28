using UnityEngine;


public class CheckCollision : MonoBehaviour
{
    private void Start()
    {
        showCollider = GetComponent<CustomShowCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Colliding(true);
    }

    private void OnTriggerStay(Collider other)
    {
        Colliding(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Colliding(false);
    }

#if UNITY_EDITOR
    CustomShowCollider showCollider;
    private void Colliding(bool value)
    {
        showCollider.IsTouching(value);
    }
#endif
}
