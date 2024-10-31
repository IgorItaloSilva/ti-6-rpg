using UnityEngine;


public class OldEnemyController : MonoBehaviour//, IEnemyBehave
{
    Rigidbody rb;
    Animator animator;
    AEnemyBehave enemyBehave; // Comportamento do personagem
    [HideInInspector] public AEnemyAction currentAction; // Ação atual do personagem
    [SerializeField] Transform target; // posição do alvo


    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        enemyBehave = GetComponent<AEnemyBehave>(); // Declara o "Cérebro" do persoangem

        enemyBehave.StartBehave(this, out currentAction); // Começa o "Cérebro" do personagem e recebe a primeira ação
        currentAction.StartAction(this); // Começa a ação atual do personagem
    }

    void FixedUpdate()
    {
        currentAction?.UpdateAction();
    }


    // ----- ACTION e BEHAVIOUR

    public void ChangeAction(bool haveToRest) // Muda a ação atual o personagem
    {
        enemyBehave.Think(out currentAction, haveToRest); // 
        currentAction.StartAction(this);
    }

    public void SetBoolAnimation(string _name, bool _value) // Tocar animação de acordo com o nome
    {
        animator.SetBool(_name, _value);
    }

    public void SetBlendTree(string _name, float _value)
    {
        animator.SetFloat(_name, _value);
    }

    public void ActionWithAnimator() // Ação junto com a animação do personagem
    {
        currentAction.ActionWithAnimator();
    }

    public void EndAnimation() // Fim da ação pela animação
    {
        currentAction.ExitAction();
    }


    // ----- GETTERS
    public Transform GetTarget() { return target; }
    public Rigidbody GetRB() { return GetComponent<Rigidbody>(); }
}