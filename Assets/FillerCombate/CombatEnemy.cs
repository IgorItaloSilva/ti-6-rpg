using UnityEngine;

public class CombatEnemy : MonoBehaviour
{
    WeaponManager[] weapons;
    void Start(){
        weapons = GetComponentsInChildren<WeaponManager>(true);
        if(weapons[0]==null)
            Debug.Log("O inimigo n achou as armas dele");
    }
    public void DesligarHitBox(){
        weapons[0]?.DisableCollider();
        //weapons[1]?.DisableCollider();
    }
    public void LigarHitBox(){
        weapons[0]?.EnableCollider();
        //weapons[1]?.EnableCollider();
    }
}
