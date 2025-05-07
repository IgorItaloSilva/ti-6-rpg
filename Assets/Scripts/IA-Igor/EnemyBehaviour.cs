using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField] ASkills ia;
    [SerializeField] CharacterController charControl;
    [SerializeField] Animator animator;
    [SerializeField] float hp;
    [SerializeField] float poise;
    float currentPoise;
    [SerializeField] float speed;

    float poiseValue;


    public EnemyBaseState currentState;
    public EnemyBaseState attackState;
    Transform target;
    float restTimer;

    [SerializeField] HealthBar healthBar;
    [SerializeField] WeaponManager weapon;



    void Start()
    {
        healthBar.SettupBarMax(hp, poise);
        currentPoise = poise;
        charControl = GetComponent<CharacterController>();

        attackState = ia.ChoseSkill();

        currentState = new NewKitsuneIdle();
        currentState.StateStart(this);
    }

    void Update() { currentState.StateUpdate(); }

    void FixedUpdate() { currentState.StateFixedUpdate(); }

    #region Rest e ia 
    public void SetRest(float value) { restTimer = value; }
    public bool isResting(){
        restTimer -= Time.deltaTime;
        return restTimer > 0;
    }

    public void ChoseSkill(){ 
        attackState = ia.ChoseSkill();
    }

    public bool IsRangeSkill() { return ia.IsRangeSkill();}

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
        if(currentPoise <= 0 && !(currentState is NewKitsuneStuned)){
            currentState = new NewKitsuneStuned();
            currentState.StateStart(this);
        }
        
    }

    public void Die()
    {
        currentState = null;
        animator.Play("Death", -1, 0.0f);
        healthBar.OnDeath();
        Destroy(this);
    }
    #endregion

    #region Weapon
    public void EnableWeapon() { weapon.EnableCollider(); }
    public void DisableWeapon() { weapon.DisableCollider(); }
    public void UseWeapon(){ ia.UseWeapon(target); }
    #endregion

}