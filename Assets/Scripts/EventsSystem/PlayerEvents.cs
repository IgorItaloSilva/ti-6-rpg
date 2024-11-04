using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerEvents 
{
    /****************************************
        COMO USAR AS CLASSES DE EVENTOS
        1-declarar um event Action da forma "public event Action onNameEvent;",
        2-depois declarar uma função publica que será chamada para executar esse evento, ela deve ter a forma 
            "public NameEvent(){
              if(onNameEvent != null){ //não podemos chamar um event vazio
                onNameEvent();
              }
            }"
        3- Para usar um evento, chamar ele pelo singleton da classe mãe, isso pode ser feito de QUALQUER lugar
            exp GameEventsManager.instance.PlayerEvents.PlayerDied()
        4- Para esse evento ter algum efeito, outros scripts precisam ter se inscrito nele,
            isso dever ser feito no OnEnable, da forma
                private OnEnable(){
                    GameEventsManager.instance.classeExemploEvents.onActionDeExemple += sua função aqui
                }
        5- Precisamos remover as funções dos events para eles não serem chamado sem ninguem inscrito neles,
            isso deve ser feito no OnDisable, da forma
                private OnEnable(){
                    GameEventsManager.instance.classeExemploEvents.onActionDeExemple -= sua função aqui
                }
    *****************************************/
    public event Action onPlayerDied;
    public void PlayerDied(){
        if(onPlayerDied!=null){
            onPlayerDied();
        }
    }
    public event Action onPlayerRespawned;
    public void PlayerRespawn(){
        if(onPlayerRespawned!=null){
            onPlayerRespawned();
        }
    }
    /* Um Evento com parametros tem a forma
    public event Action<int,float> onEventoOcorreu;
    public void EventoOcorreu(int inteiro, float flutuante){
        if(onEventoOcorreu!=null){
            onEventoOcorreu(inteiro,flutuante);
        }
    } */
    
}
