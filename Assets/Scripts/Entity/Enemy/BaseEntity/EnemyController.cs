using UnityEngine;


public class EnemyController : MonoBehaviour//, IEnemyBehave
{
 
    Rigidbody rb;
    Animator animator;
    AEnemyBehave enemyBehave; // Comportamento do personagem
    [HideInInspector] public AEnemyAction currentAction; // A��o atual do personagem
    [SerializeField] Transform target;
    [SerializeField] float meleeDistance;// Dist�ncia m�xima que � considerado melee


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

    public bool CheckDistance() // Checa dist�ncia entre o alvo o este personagem de acordo com a var�vel meleeDistance
    {
        throw new System.NotImplementedException();
    }

    // ----- ACTION e BEHAVIOUR

    public void SetAction(AEnemyAction action)
    {
        currentAction = action;
        currentAction.StartAction(this);
    }

    public void SetBoolAnimation(string _name, bool _value) // Tocar anima��o de acordo com o nome
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