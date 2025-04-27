using UnityEngine;

public class KitsuneDashAttack : EnemyActions
{
    KitsuneController kitsuneController;
    float time;
    float timeWaitDash;
    bool alreadyDashed;
    public override void EnterAction()
    {
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezePosition;
        Debug.Log("Entrei num dash");
        kitsuneController.animator.SetTrigger("isAttacking");
        kitsuneController.isDashing=true;
        time=0;
        alreadyDashed=false;
        kitsuneController.rb.AddRelativeForce(0,0,500,ForceMode.Impulse);
    }

    public override void ExitAction()
    {
        Debug.Log("Sai do dash");
    }

    public override void UpdateAction()
    {
        time+=Time.fixedDeltaTime;
        if(!alreadyDashed){
            Debug.Log(kitsuneController.target.GetPosition());
            kitsuneController.steeringManager.LookAtTargetToAttack(kitsuneController.target.GetPosition());
        }
        if((time>timeWaitDash)&&(!alreadyDashed)){
            Dash();
        }
        if(time>animationDuration)
            kitsuneController.ChangeAction(new nullAction());
    }
    public KitsuneDashAttack(float animationDuration,float waitTime,KitsuneController kitsuneController){
        this.animationDuration=animationDuration;
        this.kitsuneController=kitsuneController;
        timeWaitDash=waitTime;
    }
    private void Dash(){
        alreadyDashed=true;
        kitsuneController.rb.constraints=RigidbodyConstraints.FreezeRotation;
        kitsuneController.rb.AddRelativeForce(0,0,600,ForceMode.Impulse);
    } 

}
