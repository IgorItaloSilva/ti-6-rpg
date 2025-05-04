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
        healthBar.SettupBarMax(hp);
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
    public void SetAttack(){ ia.ChoseSkill(); }

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
        if(wasCrit)
            damage *= 2;
        hp -= damage;
        healthBar.SetValue(hp, wasCrit);
        currentPoise -= 1;
        if(currentPoise <= 0 && !(currentState is NewKitsuneStuned)){
            currentState = new NewKitsuneStuned();
            currentState.StateStart(this);
        }
        
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region Weapon
    public void EnableWeapon() { weapon.EnableCollider(); }
    public void DisableWeapon() { weapon.DisableCollider(); }
    #endregion

}