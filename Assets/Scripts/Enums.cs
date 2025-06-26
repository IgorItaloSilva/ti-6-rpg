

public class Enums {
    //COISAS DE POWER UPS
    public enum PowerUpType{
        Light,
        Dark
    }
    //COISAS DE DANO
    public enum DamageType{
        Regular,
        Magic,
        Parry,
        Poise,
        Bleed,
        Ice,
        SelfDamage
    }
    //COISAS DAS RUNAS
    public enum KatanaPart{
        Blade =0,
        Guard,
        Handle
    }
    public enum ItemQuality{
        Common,
        Rare,
        Epic,
        Legendary
    }
    public enum RuneActivationCode{
        DamageBuff,
        StatsBuff,
        TradeOff,
        OtherBonus
    }
    public enum RuneOtherCode{
        Critico
    }
    public enum AttackType{
        HeavyAttack,
        LightAttack,
        BleedAttack,//Corte de energia
        PoiseAttack,//cone de gelo
        IceAttack,//muralha de gelo
        SelfDamageAttack
    }
    
}
