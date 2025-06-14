using System;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField] protected ASkills allSkills;
    [SerializeField] CharacterController charControl;
    [SerializeField] Animator animator;
    [SerializeField] public ParticleSystem _dashVFX;
    [SerializeField] public VisualEffect _headbuttVFX;
    [SerializeField] private GameObject _bloodGameObject;
    [SerializeField] private VisualEffect _bloodVFX;
    [SerializeField] public float Hp { get; private set; }
    [SerializeField] float maxHp;
    [SerializeField] float poise;
    [SerializeField] float timeToDie;
    float currentPoise;
    [SerializeField] float speed;
    [SerializeField] float knockbackDuration = 1f;
    [SerializeField] float meleeDist = 1.5f;
    //Coisas dos outros sistemas do jogo
    [Header("Coisas dos outros Sistemas")]
    [SerializeField] int expGain;
    [SerializeField] EnemyType enemyType;
    //Coisas de boss
    [Header("Coisas de boss")]
    [SerializeField] bool isBoss;
    [SerializeField] string Nome;
    [SerializeField] KitsuneBossDeathAux kitsuneBossDeathAux;
    //Coisas do level loading manager (reload e spawn)
    [Header("Coisas do LevelLoadingManager")]
    public bool IsDead { get; private set; }
    [SerializeField] bool ignoreSaveLoad;
    [field: SerializeField] public string SaveId { get; private set; }
    Vector3 startingPos;

    public enum EnemyType
    {
        kitsune = 0,
        kasa,
        chiyo,
        yuki,
        mago,
    };

    //protected EnemyBaseState idleState; // Estado inicial de idle - Settar no inimigo
    public EnemyBaseState currentState; // Estado atual
    public EnemyBaseState attackState; // Estado de ataque
    Transform target;
    float restTimer;

    [SerializeField] HealthBar healthBar;
    [SerializeField] WeaponManager weapon;
    void Awake()
    {
        if (!ignoreSaveLoad)
        {
            if (LevelLoadingManager.instance == null)
            {
                Debug.LogWarning($"O inimigo {gameObject.name} está tentando se adicionar na lista de inimigos, mas não temos um LevelLoadingManger na cena");
            }
            LevelLoadingManager.instance.enemiesIgor.Add(this);
            if (SaveId == "")
            {
                SaveId = gameObject.name;
            }
            startingPos = transform.position;
        }
        Hp = maxHp;
    }
    void Start()
    {
        healthBar?.SettupBarMax(Hp, poise);
        currentPoise = poise;
        charControl = GetComponent<CharacterController>();
        ChoseSkill();
        currentState = new StateIdle();
        currentState.StateStart(this);
    }

    void Update() { currentState?.StateUpdate(); }

    void FixedUpdate() { currentState?.StateFixedUpdate(); }

    #region Rest e ia 
    public void SetRest(float value)
    {
        restTimer = value;
        currentState = new StateIdle();
        currentState.StateStart(this);
    }

    public bool isResting()
    {
        restTimer -= Time.deltaTime;
        return restTimer > 0;
    }

    public void ChoseSkill() { attackState = allSkills.ChoseSkill(); }

    public bool IsRangeSkill() { return allSkills.IsRangeSkill(); }

    public void StartIdle()
    {
        currentState = new StateIdle();
        currentState.StateStart(this);
    }

    #endregion

    #region Get Variaveis

    public CharacterController GetCharControl() { return charControl; }
    public Animator GetAnimator() { return animator; }
    public float GetSpeed() { return speed; }

    #endregion

    #region Target Get, Set and Clear

    public void SetTarget(Transform target) { this.target = target; }
    public Transform GetTarget() { return target; }
    public void ClearTarget() { target = null; }
    public float GetMeleeDist() { return meleeDist; }

    #endregion

    public void ResetPoise() { currentPoise = poise; }

    #region IDamagable

    public void TakeDamage(float damage, Enums.DamageType damageType, bool wasCrit)
    {
        Hp -= damage;
        currentPoise -= 1;
        healthBar?.SetValue(Hp, currentPoise, wasCrit);
        _bloodGameObject?.transform.LookAt(target.position);
        _bloodVFX?.Play();
        if(isBoss)UIManager.instance?.UpdateBossLife(Hp,wasCrit);
        if (Hp <= 0)
        {
            allSkills.DisableWeapon();
            Die();
            return;
        }
        if (currentPoise <= 0 && !(currentState is StateStuned))
        {
            allSkills.DisableWeapon();
            currentState = new StateStuned();
            currentState.StateStart(this);
        }
    }

    public void Die()
    {
        
        currentState = null;
        animator.Play("Death", -1, 0.0f);
        charControl.enabled = false;
        target.gameObject.GetComponentInChildren<EnemyDetection>().ForgetEnemy();
        if (healthBar)
        {
            healthBar.gameObject.SetActive(false);
        }
        IsDead = true;
        Save();
        if (isBoss)
        {
            HideBossInfo();
            kitsuneBossDeathAux?.Activate(this);
        }
        else
        {
            Invoke("ActualDeath", timeToDie);
        }
    }
    public void ActualDeath()
    {
        //fazer o bicho sumir
        GameEventsManager.instance.playerEvents.PlayerGainExp(expGain);
        GameEventsManager.instance.levelEvents.EnemyDied((int)enemyType);
        gameObject.SetActive(false);
    }

    public void WasParried()
    {
        currentPoise -= 4;
        healthBar?.SetValue(Hp, currentPoise, false);
        if (currentPoise <= 0 && !(currentState is StateStuned))
        {
            currentState = new StateStuned();
            currentState.StateStart(this);
            return;
        }
        SetRest(knockbackDuration);
        currentState = new StateDamage();
        currentState.StateStart(this);
    }

    #endregion

    #region Weapon

    public void EnableWeapon() { weapon.EnableCollider(); }
    public void DisableWeapon() { weapon.DisableCollider(); }
    public void UseWeapon() { allSkills.UseWeapon(); }

    #endregion
    #region Save e Load
    public void Save()
    {
        if (ignoreSaveLoad) return;
        if (LevelLoadingManager.instance == null)
        {
            Debug.Log($"O inimigo {SaveId} está tentando se salvar, mas não temos um LevelLoadingManger na cena");
        }
        //Debug.Log(LevelLoadingManager.instance.CurrentLevelData);
        //see if we have this data in dictionary        
        if (LevelLoadingManager.instance.CurrentLevelData.enemiesData.ContainsKey(SaveId))
        {
            //if so change it
            EnemyData newData = new EnemyData(this);
            LevelLoadingManager.instance.CurrentLevelData.enemiesData[SaveId] = newData;
        }
        else
        {
            //if not add it
            EnemyData newData = new EnemyData(this);
            LevelLoadingManager.instance.CurrentLevelData.enemiesData.Add(SaveId, newData);
        }
    }
    public void Load(EnemyData enemyData)
    {
        if (ignoreSaveLoad) return;
        IsDead = enemyData.isDead;
        Hp = enemyData.currentLife;
        transform.position = enemyData.lastPosition;
        Physics.SyncTransforms();
        //initiationThroughLoad=true;
        if (IsDead) gameObject.SetActive(false);
    }
    public void Respawn()
    {
        if (isBoss) return;
        charControl.enabled = true;
        currentPoise = poise;
        Hp = maxHp;
        IsDead = false;
        transform.position = startingPos;
        if (healthBar)
        {
            healthBar.gameObject.SetActive(true);
            healthBar.SettupBarMax(Hp, poise);
        }
        StartIdle();
        Save();
    }
    #endregion
    public void DisplayBossInfoIfBoss()
    {
        if(isBoss)UIManager.instance?.BossLifeSettup(Hp, maxHp, Nome);
    }
    public void HideBossInfo()
    {
        UIManager.instance?.HideBossLife();
    }

}