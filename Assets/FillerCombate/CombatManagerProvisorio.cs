using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManagerProvisorio : MonoBehaviour
{
    PlayerActions inputSystem;
    Animation anim;
    WeaponManager weapon;
    PlayerStats playerStats;
    float damage;
    public void Start(){
        anim=GetComponent<Animation>();
        weapon=GetComponentInChildren<WeaponManager>(true);
        playerStats=GetComponent<PlayerStats>();
        inputSystem = new PlayerActions();
        inputSystem.Gameplay.Enable();
        inputSystem.Gameplay.Fire.started += Atacar;
        if(weapon!=null){
            if(playerStats!=null){
                weapon.SetDamage(playerStats.Str,playerStats.Dex);
            }
        }
    }

    private void Atacar(InputAction.CallbackContext callbackContext){
        if(anim!=null)anim?.Play();
    }
    public void DesligarHitBox(){
        weapon?.DisableCollider();
    }
    public void LigarHitBox(){
        weapon?.EnableCollider();
    }
    public void OnDisable(){
        inputSystem.Gameplay.Fire.started -= Atacar;
    }
}
