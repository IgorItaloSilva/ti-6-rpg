using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData{
    public bool isDead;
    public float currentLife;
    public Vector3 lastPosition;
    public EnemyData(ActualEnemyController actualEnemyController){
        isDead=actualEnemyController.IsDead;
        currentLife = actualEnemyController.CurrentHp;
        lastPosition=actualEnemyController.transform.position;
    }
    public EnemyData(EnemyBehaviour enemyBehaviour){
        isDead=enemyBehaviour.IsDead;
        currentLife = enemyBehaviour.Hp;
        lastPosition=enemyBehaviour.transform.position;
    }
}
