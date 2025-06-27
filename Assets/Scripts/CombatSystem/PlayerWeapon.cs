using UnityEngine;

public class PlayerWeapon : WeaponManager
{
    [SerializeField] private float critRate = 5;
    private float _heavyAttackDamage;
    private float _lightAttackDamage;
    private float _strBonusDamage; //received from playerStats
    private float _dexBonusDamage; //received from playerStats

    private float _critRateBonus; //received from runeManager

    //Weapon pinduricalhos
    private float _runeBonusDamage;

    //Skill tree powerUps
    private float _doubleDamageMultiplier = 1;
    private bool _lifeStealPuActive;
    [SerializeField] private readonly bool _showDebugLogs;

    private void OnEnable()
    {
        GameEventsManager.instance.runeEvents.onRuneDamageBuff += RuneDamageBuff;
        GameEventsManager.instance.runeEvents.onRuneOtherBuff += RuneOtherBuff;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp += ActivatePowerUps;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.runeEvents.onRuneDamageBuff -= RuneDamageBuff;
        GameEventsManager.instance.runeEvents.onRuneOtherBuff -= RuneOtherBuff;
        GameEventsManager.instance.skillTreeEvents.onActivatePowerUp -= ActivatePowerUps;
    }

    protected override void Start()
    {
        damageCollider = GetComponent<Collider>();
        if (damageCollider == null)
        {
            if(_showDebugLogs) Debug.LogWarning($"O weapon manager do {name} não achou o collider dela");
        }

        damage = baseDamage;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(_showDebugLogs) Debug.Log($"A arma colidiu com um {other.name}");
        if (!other.gameObject.CompareTag("EnemyDetection"))
        {
            IDamagable alvoAtacado = other.gameObject.GetComponentInParent<IDamagable>();
            if(_showDebugLogs) Debug.Log($"A interface Idamageble que eu peguei foi {alvoAtacado}");
            if (alvoAtacado != null && alvoAtacado.GetType() != typeof(PlayerStats))
            {
                DealDamage(alvoAtacado, damage);
                DisableCollider();
                AudioPlayer.instance.PlaySFX("Stab");
                
                if(!PlayerStateMachine.Instance.TestHitFeedback)
                {
                    PlayerStateMachine.Instance.CameraShake();
                    PlayerStateMachine.Instance.HitAnimatorPause();
                }

            }
        }
    }

    protected override void DealDamage(IDamagable alvo, float dano)
    {
        float damageDealt;
        bool crited = false;
        if (damagedTargets.Contains(alvo))
        {
            return;
        }

        damagedTargets.Add(alvo);
        //CritLogic
        if (Random.Range(0f, 100f) <= critRate + _critRateBonus)
        {
            damageDealt = damage * 2 * _doubleDamageMultiplier;
            crited = true;
        }
        else
        {
            damageDealt = damage * _doubleDamageMultiplier;
        }

        if (_lifeStealPuActive)
        {
            GameEventsManager.instance.skillTreeEvents.LifeStealHit(damageDealt / 2);
            if (showDebug) Debug.Log($"Curando o jogador com lifeSteal de {damageDealt / 2}");
        }
        
        alvo.TakeDamage(damageDealt, damageType, crited);
        if (showDebug) Debug.Log($"Enviei {damageDealt} de dano para ser tomado para {alvo}");
    }

    public void SetDamageAndValues(float strongAttackBonus, float fastAttackBonus)
    {
        _strBonusDamage = strongAttackBonus;
        _dexBonusDamage = fastAttackBonus;
        SetDamage();
    }

    private void SetDamage()
    {
        _heavyAttackDamage = baseDamage + _strBonusDamage + _runeBonusDamage;
        _lightAttackDamage = baseDamage + _dexBonusDamage + _runeBonusDamage;
    }

    public void SetDamageType(Enums.AttackType attackType)
    {
        //Chamado pelo playerStateMachine
        switch (attackType)
        {
            case Enums.AttackType.LightAttack:
                damage = _lightAttackDamage;
                damageType = Enums.DamageType.Regular;
                break;
            case Enums.AttackType.HeavyAttack:
                damage = _heavyAttackDamage;
                damageType = Enums.DamageType.Regular;
                break;
            case Enums.AttackType.BleedAttack:
                damage = _heavyAttackDamage;
                damageType = Enums.DamageType.Bleed;
                break;
            case Enums.AttackType.PoiseAttack:
                damage = 0f;
                damageType = Enums.DamageType.Poise;
                break;
            case Enums.AttackType.JumpAttack:
                damage = _heavyAttackDamage;
                damageType = Enums.DamageType.Ice;
                break;
            case Enums.AttackType.SelfDamageAttack:
                damage = _heavyAttackDamage * 6;
                damageType = Enums.DamageType.SelfDamage;
                break;
        }
    }

    public void RuneDamageBuff(bool isActivate, int value)
    {
        if (isActivate)
        {
            _runeBonusDamage = value;
        }
        else
        {
            _runeBonusDamage = 0;
            ;
        }

        SetDamage();
    }

    private void ActivatePowerUps(int id)
    {
        switch (id)
        {
            //DoubleDamage
            case 8:
                _doubleDamageMultiplier = 1.5f;
                break;
            //LifeSteal
            case 7:
                _lifeStealPuActive = true;
                break;
        }
    }

    public void RuneOtherBuff(bool isActivate, Enums.RuneOtherCode code, int amount)
    {
        if (isActivate)
        {
            switch (code)
            {
                case Enums.RuneOtherCode.Critico:
                    _critRateBonus = amount;
                    break;
            }
        }
        else
        {
            switch (code)
            {
                case Enums.RuneOtherCode.Critico:
                    _critRateBonus = 0;
                    break;
            }
        }
    }
}