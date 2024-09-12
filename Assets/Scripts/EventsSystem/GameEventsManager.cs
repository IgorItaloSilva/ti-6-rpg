using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameEventsManager : MonoBehaviour
{
    /****************************************
                COMO USAR  ESSE GAME EVENTS MANAGER
        Declarar classes novas que definem os eventos possiveis, e inicializar elas 
        no awake, elas devem ser publicas
        
        !!!!Tutorial de como declarar classes e usar os eventos no script PlayerEvents!!!!
    *****************************************/
    public static GameEventsManager instance {get; private set;}
    public PlayerEvents playerEvents;
    //EXEMPLO DE NOVA CLASSE
    //public NovaClasseEvents novaClasseEvents;
    private void Awake(){
        if(instance!=null){
            Debug.LogError("Mais de um Game Events Manager existe");
        }
        instance = this;
        playerEvents = new PlayerEvents();
        //novaClasseEvents = new NovaClasseEvents();
    }

}
