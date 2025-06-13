using UnityEngine;

public abstract class EnemyBaseState
{
    #region Start Variables
    protected EnemyBehaviour enemyBehave;
    protected CharacterController charControl;
    protected Animator animator;
    public float speed;
    #endregion


    #region Apply Rotation Variables
    float newSteering;
    Vector3 directionToPlayer;
    Quaternion rotationDesired;
    #endregion



    protected float minDistPlayer = 3f; // Distancia minima do jogador
    float verticalVel = 0; // Variável de gravidade
    protected float steeringForce; // Força de Rotação
    protected float lookTime; // Temporizador da rotação
    protected float restTime; // Tempo de descanço máximo
    protected Vector3 _knockbackDir, _appliedMovement;
    protected float _acceleration = 1.5f;


    public virtual void StateStart(EnemyBehaviour enemyBehave)
    {
        this.enemyBehave = enemyBehave;
        this.charControl = enemyBehave.GetCharControl();
        this.animator = enemyBehave.GetAnimator();
        this.speed = enemyBehave.GetSpeed();
        restTime = 0;
        lookTime = 0;
        OneExecution();
    }


    protected virtual void OneExecution() { }

    public virtual void StateUpdate() { }

    public virtual void StateFixedUpdate() { }

    protected virtual void StateExit() { enemyBehave.currentState.StateStart(enemyBehave); }

    protected float GetPlayerDistance() {

        if (enemyBehave.GetTarget())
        {
            Transform newTarget = enemyBehave.GetTarget().transform;
            return Vector3.Distance(newTarget.position, charControl.transform.position);

        }
        else return 0f;

    }

    protected float GetTargetAngle(Transform from, Transform to) {
        Vector3 desiredDir = (to.position - from.position).normalized;
        return Vector3.Angle(from.forward, desiredDir);
    }

    protected float ApplyGravity() {
        if (charControl.isGrounded)
        {
            verticalVel = -1f;
        }
        else
        {
            verticalVel -= 9.81f * Time.deltaTime;
        }
        return verticalVel;
    }


    protected Quaternion ApplyRotation()
    {
        newSteering = steeringForce;
        // Rotation
        directionToPlayer = (enemyBehave.GetTarget().position - charControl.transform.position).normalized;
        directionToPlayer.y = 0;
        rotationDesired = Quaternion.LookRotation(directionToPlayer);
        if(lookTime < 1)
                lookTime += newSteering * Time.fixedDeltaTime;
        return Quaternion.Slerp(charControl.transform.rotation, rotationDesired, lookTime);
    }
    
    protected void Knockback()
    {
        if (_acceleration <= 0)
        {
            _acceleration = 0;
            return;
        }
        
        _appliedMovement.x = _knockbackDir.x * 5 * _acceleration;
        _appliedMovement.z = _knockbackDir.z * 5 * _acceleration;

        charControl.Move(_appliedMovement * Time.fixedDeltaTime);
        
        
        _acceleration -= Time.fixedDeltaTime * 3;

    }
}