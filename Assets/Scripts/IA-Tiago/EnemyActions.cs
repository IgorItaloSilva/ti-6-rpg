using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyActions
{
    protected float animationDuration;
    protected float distToAttack;
    protected ActualEnemyController actualEnemyController;

    public abstract void UpdateAction();
    public abstract void EnterAction();
    public abstract void ExitAction();
    /***EXEMPLOS DE CONSTRUTORES QUE VAMOS PRECISAR AO IMPLEMENTAR UMA ACTION*******

        BASIC ATTACK
    public EnemyActions(float distToAttack,ActualEnemyController actualEnemyController){
        this.distToAttack=distToAttack;
        this.actualEnemyController=actualEnemyController;
    }
        NULL
    public EnemyActions(){
        
    }
        REST/IDLE

    ********************************************/
}
