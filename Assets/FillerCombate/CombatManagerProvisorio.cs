using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManagerProvisorio : MonoBehaviour
{
    PlayerInput inputSystem;
    Animation anim;
    WeaponManager weapon;
    PlayerStats playerStats;
    float damage;
    public void Start(){
        anim=GetComponent<Animation>();
        weapon=GetComponentInChildren<WeaponManager>(true);
        playerStats=GetComponent<PlayerStats>();
        inputSystem = new PlayerInput();
        inputSystem.Gameplay.Enable();
        inputSystem.Gameplay.Attack.started += Atacar;
        if(weapon!=null){
            if(playerStats!=null){
                //weapon.SetDamage(playerStats.Str,playerStats.Dex);
            }
        }
    }
    void Update(){
        if(Keyboard.current.lKey.wasPressedThisFrame){
            DataPersistenceManager.instance.LoadGame();
        }
        if(Keyboard.current.kKey.wasPressedThisFrame){
            GameEventsManager.instance.uiEvents.SavedGame();
            DataPersistenceManager.instance.SaveGame();
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
        inputSystem.Gameplay.Attack.started -= Atacar;
    }
}
