using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestEnemyController : MonoBehaviour 
{
    [SerializeField] protected EntityParameters parameters;
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;


    [SerializeField] protected List<TestEnemyActions> movementActions = new List<TestEnemyActions>(); // 0 - idle | 1 - walk | 2 - Run | 3 - Turn Around
    [SerializeField] protected List<TestEnemyActions> attackActions = new List<TestEnemyActions>(); // Lista com todos os ataques
    [SerializeField] protected Queue<TestEnemyActions> queueAttacks = new Queue<TestEnemyActions>(); // Fila com ordem de ataque do inimigo aleat�ria
    [SerializeField] protected TestEnemyActions currentAction; // A��o atual do inimigo

    [SerializeField] protected Transform target; // Jogador
    private float bkRestTime = 2f;


    void Start() { 
        animator = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        StartEnemy(); 
    }

    private void FixedUpdate() {
        currentAction?.UpdateAction(); 
    }
    protected abstract void StartEnemy(); // Come�o do inimigo
        // Template:
        // movementActions.Add();
        // attackActions.Add();
        // ShuffleAttacks();
        // CurrentAction = movementActions[0];
        // bkRestTime = queueAttacks[0].restTime;
        // CurrentAction.StartAction(this, rb,);


    public void EnemyAttacked() // Ativado quando a a��o do inimigo � um ataque
    {
        queueAttacks.Dequeue(); // Retira a a��o realizada
        Debug.Log("Antes " + queueAttacks.Count);
        if (queueAttacks.Count == 0) // Checa se ja utilizou todos os ataques randomizados
            ShuffleAttacks(); // Randomiza os pr�ximos ataques
        Debug.Log("Depois " + queueAttacks.Count);
    }


    protected void ShuffleAttacks()// Randomiza os pr�ximos ataques
    {
        List<TestEnemyActions> shuffleList = new();
        shuffleList.AddRange(attackActions); // Lista para randomizar a fila de a��es
        while(shuffleList.Count > 0)
        {
            int value = Random.Range(0, shuffleList.Count);
            queueAttacks.Enqueue(shuffleList[value]);
            shuffleList.RemoveAt(value);
        }
    }


    // -----  Getters  ----- \\
    public EntityParameters GetParameters() { return parameters; }
    public Rigidbody GetRigidbody() { return rb; }
    public Animator GetAnimator() { return animator; }
    public float GetRestTime() { return bkRestTime; }
    public float GetNextMinRange() { return queueAttacks.Peek().GetMinRange(); } // pega o minimo de range do pr�ximo ataque do inimigo
    public TestEnemyActions GetMoveActions(int index) { return movementActions[index]; }
    public TestEnemyActions GetAttackActions() { return queueAttacks.Peek(); }
    public Transform GetTarget() { return target; }
    public void SetMovement(TestEnemyActions moveActions) { 
        currentAction = moveActions;
        currentAction.StartAction(this);
    }

    public void SetAttack()
    {
        currentAction = queueAttacks.Peek();
        currentAction.StartAction(this);
    }

}




//// Parado:
// - no momento que ir para o idle, checar se consegue olhar para o jogador, caso n�o consiga diminuir o tempo de espera pela metade e chamar a��o de se virar e diminuir o tempo novamente pela metade

//// End Parado


//// Movimento:

// -Adicionar aleatoriamente um movimento de acordo com a a��o, Exe: chance de 7 walk, 3 run
// -Caso rest da a��o chegue a 0 e n�o conseguiu chegar no minimo da pr�xima a��o, se a a��o atual � caminhada, ele passa para a de corrida

//// End Movimento