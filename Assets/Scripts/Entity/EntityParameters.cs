using UnityEngine;
using UnityEngine.UI;


public class EntityParameters : MonoBehaviour,IDamagable
{
    [SerializeField] SO_Entity entity;

    [SerializeField] float currentHp;
    [SerializeField] int currentSpeed;
    [SerializeField] Slider sliderVida;

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        sliderVida.value = currentHp;
        if(currentHp<=0){
            Morrer();
        }
    }

    private void Start()
    {
        sliderVida = GetComponentInChildren<Slider>(true);
        if (entity)
        {
            currentHp = entity.cons;
            currentSpeed = entity.dext;
            sliderVida.maxValue=currentHp;
            sliderVida.value=currentHp;
        }
    }
    private void Morrer(){
        Destroy(gameObject);
    }

}