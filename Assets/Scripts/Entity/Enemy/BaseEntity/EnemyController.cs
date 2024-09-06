using UnityEngine;


public class EnemyController : MonoBehaviour//, IEnemyBehave
{
 
    Rigidbody rb;
    Animator animator;
    AEnemyBehave enemyBehave; // Comportamento do personagem
    [HideInInspector] public AEnemyAction currentAction; // Ação atual do personagem
    [SerializeField] Transform target;
    [SerializeField] float meleeDistance;// Distância máxima que é considerado melee


    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemyBehave = GetComponent<AEnemyBehave>();
        enemyBehave.StartBehave(this);
        SetAction(enemyBehave.GetAction(0));
        
    }

    void FixedUpdate()
    {
        currentAction?.UpdateAction();
        enemyBehave.CoolDownTimer();
    }

    public bool CheckDistance() // Checa distância entre o alvo o este personagem de acordo com a varável meleeDistance
    {
        throw new System.NotImplementedException();
    }

    // ----- ACTION e BEHAVIOUR

    public void SetAction(AEnemyAction action)
    {
        currentAction = action;
        currentAction.StartAction(this);
    }

    public void SetBoolAnimation(string _name, bool _value) // Tocar animação de acordo com o nome
    {
        animator.SetBool(_name, _value);
    }

    public void ActionWithAnimator()
    {
        currentAction.ActionWithAnimator();
    }

    public void EndAnimation()
    {
        currentAction.EndAnimation();
    }

    public Transform GetTarget() { return target; }
    public Rigidbody GetRB() { return GetComponent<Rigidbody>(); }
}