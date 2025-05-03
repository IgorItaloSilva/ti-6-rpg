using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamagable
{
    [SerializeField] CharacterController charControl;
    [SerializeField] Animator animator;
    [SerializeField] float hp;
    [SerializeField] float speed;

    float poiseValue;


    public EnemyBaseState currentState;
    [SerializeField] Transform target;
    [SerializeField] HealthBar healthBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar.SettupBarMax(hp);
        charControl = GetComponent<CharacterController>();
        currentState = new NewKitsuneMoving();
        currentState.StateStart(this);
    }

    void Update() { currentState.StateUpdate(); }

    void FixedUpdate() { currentState.StateFixedUpdate(); }

    #region Get Variables
    public CharacterController GetCharControl(){ return charControl; }
    public Animator GetAnimator(){ return animator; }
    public float GetSpeed(){ return speed; }

    #endregion

    #region Target Get, Set and Clear
    public void SetTarget(Transform target) { this.target = target; }
    public Transform GetTarget() { return target; }

    public void ClearTarget() { target = null; }
    #endregion

    #region IDamagable
    public void TakeDamage(float damage, Enums.DamageType damageType, bool wasCrit)
    {
        if(wasCrit)
            damage *= 2;
        hp -= damage;
        healthBar.SetValue(hp, wasCrit);
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
    #endregion

}