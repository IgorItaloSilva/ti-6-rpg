using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class EnemyBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField] protected ASkills allSkills;
    [SerializeField] CharacterController charControl;
    [SerializeField] Animator animator;

    [Header("VFX")]
    [SerializeField] public ParticleSystem _dashVFX;
    [SerializeField] public VisualEffect _headbuttVFX;
    [SerializeField] private VisualEffect _bloodVFX;
    [SerializeField] public ParticleSystem _uncorruptedVFX;
    [SerializeField] public VisualEffect _corruptedVFX;

    [FormerlySerializedAs("_enemySounds")]
    [Header("Audio")]
    public EnemySounds enemySounds;
    [FormerlySerializedAs("_soundSource")] public AudioSource soundSource;

    [Header("Valores")]
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
    [SerializeField] Vector3 initialPos;
    [SerializeField] float maxDistSpawn;

    //Coisas de boss
    [Header("Coisas de boss")]
    [SerializeField] bool isBoss;
    [SerializeField] string Nome;
    DialogChoiceAux dialogChoiceAux;
    [SerializeField] bool hasDialogChoice;

    //Coisas do level loading manager (reload e spawn)
    [Header("Coisas do LevelLoadingManager")]
    public bool IsDead { get; private set; }
    [SerializeField] bool ignoreSaveLoad;
    [SerializeField] private readonly bool _showDebugLogs;
    [field: SerializeField] public string SaveId { get; private set; }
    Vector3 startingPos;
    public bool neverDied { get; private set; }

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
    byte damageCounter;

    [SerializeField] HealthBar healthBar;
    [SerializeField] WeaponManager weapon;



    void Awake()
    {
        GameEventsManager.instance.playerEvents.onPlayerRespawned += OnPlayerDied;
        if (!ignoreSaveLoad)
        {
            if (LevelLoadingManager.instance == null)
            {
                if (_showDebugLogs) Debug.LogWarning($"O inimigo {gameObject.name} está tentando se adicionar na lista de inimigos, mas não temos um LevelLoadingManger na cena");
            }
            LevelLoadingManager.instance.enemiesIgor.Add(this);
            if (SaveId == "")
            {
                SaveId = gameObject.name;
            }
            startingPos = transform.position;
        }
        Hp = maxHp;
        neverDied = true;
        if (hasDialogChoice) dialogChoiceAux = gameObject.GetComponent<DialogChoiceAux>();
        initialPos = transform.position;
    }

    void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onPlayerRespawned -= OnPlayerDied;
    }

    void Start()
    {
        healthBar?.SettupBarMax(maxHp, poise);
        if (healthBar) healthBar.SetValue(Hp, false);
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
    public float GetDistSpawn() { return Vector3.Distance(transform.position, initialPos); }
    public bool IsDistFromSpawn() { return GetDistSpawn() > maxDistSpawn && maxDistSpawn > 0; }
    public Vector3 GetInitialPos() { return initialPos; }

    #endregion

    #region Target Get, Set and Clear

    public void SetTarget(Transform target) { this.target = target; }
    public Transform GetTarget() { return target; }
    public bool HasTarget() { return target != null ? true : false; }
    public void ClearTarget() { target = null; }
    public float GetMeleeDist() { return meleeDist; }

    #endregion

    public void ResetPoise() { currentPoise = poise; }

    #region IDamagable

    public void TakeDamage(float damage, Enums.DamageType damageType, bool wasCrit)
    {
        Hp -= damage;
        currentPoise -= damageType switch
        {
            Enums.DamageType.Poise => 7,
            Enums.DamageType.Magic or Enums.DamageType.Bleed => 0,
            _ => 0.5f
        };

        if (damageType == Enums.DamageType.Bleed)
        {
            //Debug.LogError("Bleed activated");
            damageCounter = 0;
            StartCoroutine(Bleed());
        }

        if (healthBar) healthBar.SetValue(Hp, currentPoise, wasCrit);
        if (target && damageType != Enums.DamageType.Poise && _bloodVFX)
        {
            _bloodVFX.gameObject.transform.LookAt(target.position);
            _bloodVFX.Play();
        }
        if (isBoss) UIManager.instance?.UpdateBossLife(Hp, currentPoise, wasCrit);
        if (currentState?.GetType() == typeof(StateIdle))
        {
            animator.Play("Damage", 0, 0);
            if(enemySounds) enemySounds.PlaySound(EnemySounds.SoundType.Damage, soundSource);
        }
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
        PlayerStateMachine.Instance.EnemyDetector.ForgetEnemy();
        if (healthBar)
        {
            healthBar.gameObject.SetActive(false);
        }
        IsDead = true;
        if (isBoss)
        {
            HideBossInfo();
            if (hasDialogChoice && neverDied)
            {
                Invoke(nameof(OpenDialog), timeToDie);
            }
            else
            {
                Invoke(nameof(ActualDeath), timeToDie);
            }
        }
        else
        {
            if (hasDialogChoice && neverDied)
            {
                Invoke(nameof(OpenDialog), timeToDie);
            }
            else
            {
                Invoke(nameof(ActualDeath), timeToDie);
            }
        }
        Save();
    }

    public IEnumerator Bleed()
    {
        float damage = maxHp / 40;
        while (damageCounter < 5 && !IsDead)
        {
            TakeDamage(damage, Enums.DamageType.Magic, false);
            damageCounter++;
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

    void OnPlayerDied()
    {
        allSkills.DisableWeapon();
        Hp = maxHp;
        if (healthBar) healthBar.SetValue(Hp, false);
        transform.position = startingPos;

    }

    public void ActualDeath()
    {

        
        if (allSkills is MagoSkills)
        {
            GameManager.instance.EndGame();
        }
        //fazer o bicho sumir
        GameEventsManager.instance.playerEvents.PlayerGainExp(expGain);
        GameEventsManager.instance.levelEvents.EnemyDied((int)enemyType);
        Invoke(nameof(DisableKitsune), 5f);
        
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
        attackState = allSkills.ChoseSkill();
    }

    private void DisableKitsune()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Weapon

    public void EnableWeapon()
    {
        weapon.EnableCollider();
    }
    public void DisableWeapon() { weapon.DisableCollider(); }
    public void UseWeapon() { allSkills.UseWeapon(); }

    #endregion
    #region Save e Load
    public void Save()
    {
        if (ignoreSaveLoad) return;
        if (LevelLoadingManager.instance == null)
        {
            if (_showDebugLogs) Debug.Log($"O inimigo {SaveId} está tentando se salvar, mas não temos um LevelLoadingManger na cena");
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
        neverDied = enemyData.neverDied;
        if (healthBar) healthBar.SetValue(Hp, false);
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
            healthBar.SettupBarMax(maxHp, poise);
        }
        StartIdle();
        
        if (hasDialogChoice && dialogChoiceAux.IsDialogAnserInteractableActiveInHierarchy())
        {
            dialogChoiceAux.Deactivate();
            neverDied = true;
        }
        Save();

    }
    #endregion
    public void DisplayBossInfoIfBoss()
    {
        if (isBoss)
        {
            UIManager.instance?.BossLifeSettup(Hp, maxHp, currentPoise, poise, Nome);
            //UIManager.instance?.BossPoiseSettup(Hp, maxHp, Nome);
        }
    }
    public void HideBossInfo()
    {
        UIManager.instance?.HideBossLife();
    }
    void OpenDialog()
    {
        if (dialogChoiceAux)
        {
            dialogChoiceAux.Activate();
            neverDied = false;
        }
    }
    


}