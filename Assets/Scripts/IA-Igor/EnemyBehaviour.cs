using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField] protected ASkills allSkills;
    [SerializeField] CharacterController charControl;
    [SerializeField] Animator animator;
    [SerializeField] float hp;
    [SerializeField] float poise;
    float currentPoise;
    [SerializeField] float speed;



    protected EnemyBaseState idleState; // Estado inicial de idle - Settar no inimigo
    public EnemyBaseState currentState; // Estado atual
    public EnemyBaseState attackState; // Estado de ataque
    Transform target;
    float restTimer;

    [SerializeField] HealthBar healthBar;
    [SerializeField] WeaponManager weapon;



    void Start()
    {
        healthBar.SettupBarMax(hp, poise);
        currentPoise = poise;
        charControl = GetComponent<CharacterController>();
        ChoseSkill();
        OneExecution();
    }

    protected virtual void OneExecution(){  }



    void Update() { currentState.StateUpdate(); }

    void FixedUpdate() { currentState.StateFixedUpdate(); }

    #region Rest e ia 
    public void SetRest(float value) { restTimer = value; }
    public bool isResting(){
        restTimer -= Time.deltaTime;
        return restTimer > 0;
    }

    public void ChoseSkill(){ 
        attackState = allSkills.ChoseSkill();
    }

    public bool IsRangeSkill() { return allSkills.IsRangeSkill();}

    #endregion

    #region Get Variaveis
    public CharacterController GetCharControl(){ return charControl; }
    public Animator GetAnimator(){ return animator; }
    public float GetSpeed(){ return speed; }
    #endregion

    #region Target Get, Set and Clear
    public void SetTarget(Transform target) { this.target = target; }
    public Transform GetTarget() { return target; }
    public void ClearTarget() { target = null; }
    public void IdleState(){ currentState = idleState; currentState.StateStart(this); }

    #endregion

    public void ResetPoise(){ currentPoise = poise; }

    #region IDamagable
    public void TakeDamage(float damage, Enums.DamageType damageType, bool wasCrit)
    {
        hp -= damage;
        currentPoise -= 1;
        healthBar.SetValue(hp, currentPoise, wasCrit);
        if(hp <= 0){
            Die();
            return;
        }
        if(currentPoise <= 0 && !(currentState is StateStuned)){
            currentState = new StateStuned();
            currentState.StateStart(this);
        }
        
    }

    public void Die()
    {
        currentState = null;
        animator.Play("Death", -1, 0.0f);
        healthBar.OnDeath();
        target.gameObject.GetComponentInChildren<EnemyDetection>().ForgetEnemy();
        Destroy(this);
    }

    #endregion

    #region Weapon
    public void EnableWeapon() { weapon.EnableCollider(); }
    public void DisableWeapon() { weapon.DisableCollider(); }
    public void UseWeapon(){ allSkills.UseWeapon(); }
    #endregion

}