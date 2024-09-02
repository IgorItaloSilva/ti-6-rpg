using UnityEngine;


public class EntityParameters : MonoBehaviour
{
    [SerializeField] SO_Entity entity;

    [SerializeField] int currentHp;
    [SerializeField] int currentSpeed;


    private void Start()
    {
        if (entity)
        {
            currentHp = entity.GetmaxHp();
            currentSpeed = entity.GetmaxSpeed();
        }
    }
}