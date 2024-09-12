using UnityEngine;


public class CheckCollision : MonoBehaviour
{
#if UNITY_EDITOR //maluco eu movi isso pro inicio do script pra poder criar uma build
    private void Start()
    {
        if(showCollider!=null){
        showCollider = GetComponent<CustomShowCollider>();
        }
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

    CustomShowCollider showCollider;
    private void Colliding(bool value)
    {
        showCollider.IsTouching(value);
    }
#endif
}
