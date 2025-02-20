using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillTreeEvents {
    public event Action<int> onUnlockBuy;
    public void UnlockBuy(int id){
        if(onUnlockBuy!=null){
            onUnlockBuy(id);
        }
    }
    public event Action<int> onActivatePowerUp;
    public void ActivatePowerUp(int id){
        if(onActivatePowerUp!=null){
            onActivatePowerUp(id);
        }
    }
    public event Action<float>onLifeStealHit;
    public void LifeStealHit(float heal){
        if(onLifeStealHit!=null){
            onLifeStealHit(heal);
        }
    }
    //MUDADO PARA SINGLETON
    /* public event Action<int> onPlayerGetsPowerUpMoney;
    public void PlayerGetsPowerUpMoney(int powerUpType){
        if(onPlayerGetsPowerUpMoney!=null){
            onPlayerGetsPowerUpMoney(powerUpType);
        }
    } */
}
