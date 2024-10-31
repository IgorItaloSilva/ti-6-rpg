using UnityEngine;


public class OldEnemyController : MonoBehaviour//, IEnemyBehave
{
    Rigidbody rb;
    Animator animator;
    AEnemyBehave enemyBehave; // Comportamento do personagem
    [HideInInspector] public AEnemyAction currentAction; // A��o atual do personagem
    [SerializeField] Transform target; // posi��o do alvo


    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        enemyBehave = GetComponent<AEnemyBehave>(); // Declara o "C�rebro" do persoangem

        enemyBehave.StartBehave(this, out currentAction); // Come�a o "C�rebro" do personagem e recebe a primeira a��o
        currentAction.StartAction(this); // Come�a a a��o atual do personagem
    }

    void FixedUpdate()
    {
        currentAction?.UpdateAction();
    }


    // ----- ACTION e BEHAVIOUR

    public void ChangeAction(bool haveToRest) // Muda a a��o atual o personagem
    {
        enemyBehave.Think(out currentAction, haveToRest); // 
        currentAction.StartAction(this);
    }

    public void SetBoolAnimation(string _name, bool _value) // Tocar anima��o de acordo com o nome
    {
        animator.SetBool(_name, _value);
    }

    public void SetBlendTree(string _name, float _value)
    {
        animator.SetFloat(_name, _value);
    }

    public void ActionWithAnimator() // A��o junto com a anima��o do personagem
    {
        currentAction.ActionWithAnimator();
    }

    public void EndAnimation() // Fim da a��o pela anima��o
    {
        currentAction.ExitAction();
    }


    // ----- GETTERS
    public Transform GetTarget() { return target; }
    public Rigidbody GetRB() { return GetComponent<Rigidbody>(); }
}