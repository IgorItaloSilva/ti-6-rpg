using UnityEngine;


public class EnemyBehaviour : MonoBehaviour, IEnemyBehave
{
    AEnemyAction[] action; // Todas ações possiveis do personagem
    [SerializeField] float[] coolDownActions; // Cooldown de cada skill
    [SerializeField] float[] actionsReady; // Quando zerado, habilidade pronta para usar
    AEnemyAction currentAction; // Ação atual do personagem
    [SerializeField] Rigidbody rb; 
    [SerializeField] Animator animator;

    int index = 0;

    

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        currentAction?.UpdateAction();
    }

    public bool CheckDistance()
    {
        throw new System.NotImplementedException();
    }

    public void IsClose()
    {
        throw new System.NotImplementedException();
    }

    public void IsDistant()
    {
        throw new System.NotImplementedException();
    }

    public void ChangeActionStage()
    {
        currentAction?.ChangeStage();
    }

    public Rigidbody GetRB()
    {
        return GetComponent<Rigidbody>();
    }

    public void SetActions(AEnemyAction[] enemyActions) // Setar as possiveis ações do personagem (DEV: Tentar incorporar usando um abstract por causa do uso de variável)
    { 
        action = enemyActions;
        currentAction = action[0];
    }



     // ----- EDITOR
#if UNITY_EDITOR
    [SerializeField] int whichAction;
    public void DebugAction()
    {
        currentAction = action[whichAction];
        currentAction.StartAction(this);
    }
#endif

}