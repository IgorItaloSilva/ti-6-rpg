using UnityEngine;

public abstract class EnemyBaseState
{
    #region Start Variables
    protected EnemyBehaviour enemyBehave;
    protected CharacterController charControl;
    protected Animator animator;
    protected float speed;
    #endregion


    protected float minDistPlayer = 3f; // Distancia minima do jogador
    float verticalVel = 0; // Variável de gravidade
    protected float steeringForce; // Força de Rotação
    protected float lookTime; // Temporizador da rotação
    protected float restTime; // Tempo de descanço máximo
    public bool isRange { get; }


    // Escolher skill -> Checar se skill e range ? Usar : Aproximar -> Esperar -> Reinicia


    public virtual void StateStart(EnemyBehaviour enemyBehave) {
        this.enemyBehave = enemyBehave;
        this.charControl = enemyBehave.GetCharControl();
        this.animator = enemyBehave.GetAnimator();
        this.speed = enemyBehave.GetSpeed();
        restTime = 0;
        lookTime = 0;
        OneExecution();
    }

    public virtual void StateAttackEnd(){
    }

    protected virtual void OneExecution() {  }

    public virtual void StateUpdate() {  }

    public virtual void StateFixedUpdate() {  }
    
    protected virtual void StateExit() { enemyBehave.currentState.StateStart(enemyBehave); }

    protected float GetPlayerDistance(){
        
        if(enemyBehave.GetTarget()){
            Transform newTarget = enemyBehave.GetTarget().transform;
            return Vector3.Distance(newTarget.position, charControl.transform.position);

        }else return 0f;

    }

    protected float GetTargetAngle(Transform from, Transform to){
        Vector3 desiredDir = (to.position - from.position).normalized;
        return Vector3.Angle(from.forward, desiredDir);
    }

    protected float ApplyGravity() {
        if(charControl.isGrounded){
            verticalVel = -1f;
        }else{
            verticalVel -= 9.81f * Time.deltaTime;
        }
        return verticalVel;
    }

}