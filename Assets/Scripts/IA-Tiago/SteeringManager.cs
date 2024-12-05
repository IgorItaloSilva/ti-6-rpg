using UnityEngine;

/*  ************************
    COMO USAR ESSA CLASSE: Na classe de quem for usar isso aqui vc precisa ISteeringAgent
    e implementar os metodos que ela pede, criando as variaveis que serão usadas pelos metodos.
    Ai vc precisa criar um steeringManager e no fixed update do ser chamar os steeringBehaviours que vc quer, e ai o
    update do steering behaviour. Tem um exemplo no final do arquivo
    ***********************  */
public class SteeringManager{
    public Vector3 steering;
    public ISteeringAgent steeringAgent;
    public Rigidbody rb;
    const float MaxRaycastDist = 2;
    private float wanderRate = 5f;  //era.4
    private float wanderOffset = 1.5f; //quanto a dir muda de frame pra frame era 1.5
    private float wanderRadius = 2f;//era 4  
    private float wanderOrientation = 0f;
    protected bool shouldWander;
    // The constructor
    public SteeringManager(ISteeringAgent agent,Rigidbody rb) {
    	this.steeringAgent	= agent;
    	this.steering 	= Vector3.zero;
        this.rb = rb;
    }
    // The public API (one method for each behavior)
    public void Seek(Vector3 target, float slowingRadius = 0) {
        steering += DoSeek(target, slowingRadius);
    }
    public void Flee(Vector3 target) {
        steering += DoFlee(target);
    }
    public void AvoidObstacle() {
        steering += DoAvoidObstacle();
    }
    public void Wander(){
        steering += DoWander();
    }
    public void Evade(ISteeringAgent targetAgent){
        steering+=DoEvade(targetAgent);
    }
    public void Pursuit(ISteeringAgent targetAgent){
        steering+=DoPursuit(targetAgent);
    }
    // The update method.
    // Should be called after all behaviors have been invoked
    public void Update(){
        steering.y=0f;
        steering = steering.normalized * steeringAgent.GetMaxForce();
        rb.gameObject.transform.LookAt(steering);
        rb.gameObject.transform.eulerAngles=new Vector3(0,rb.gameObject.transform.eulerAngles.y,0);
        rb.AddForce(steering);
        //Debug.DrawRay(rb.position,steering,Color.red);
        
    }
    // Reset the internal steering force.
    public void Reset(){
        steering = Vector3.zero;
    }
    // The internal API
    private Vector3 DoSeek(Vector3 target, float slowingRadius = 0) {
        Vector3 steeringForce = Vector3.zero;
        float maxVelocity = steeringAgent.GetMaxVelocity();
        Vector3 desiredVelocity = target - steeringAgent.GetPosition();
        float distanceToTarget = desiredVelocity.magnitude;
        desiredVelocity.Normalize();
        if(distanceToTarget<=slowingRadius){
            desiredVelocity = desiredVelocity * maxVelocity * (distanceToTarget/slowingRadius);
        }
        else{
            desiredVelocity = desiredVelocity * maxVelocity;
        }
        steeringForce = desiredVelocity - steeringAgent.GetVelocity();
        return steeringForce*2;
    }
    private Vector3 DoFlee(Vector3 target){
        Vector3 steeringForce = Vector3.zero;
        Vector3 desiredVelocity = (steeringAgent.GetPosition()-target).normalized * steeringAgent.GetMaxVelocity();
        steeringForce = desiredVelocity - steeringAgent.GetVelocity();
        return steeringForce;
    }
	private Vector3 DoWander(){
        Vector3 steering;
        wanderOrientation += RandomBinomial() * wanderRate;  
        float characterOrientation = rb.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;  
        float targetOrientation = wanderOrientation +  characterOrientation;  
        Vector3 targetPosition = rb.transform.position + (wanderOffset * OrientationToVector(characterOrientation));  
        targetPosition += wanderRadius * OrientationToVector(targetOrientation);  
        steering = targetPosition - rb.transform.position;  
        steering.Normalize();  
        steering *= steeringAgent.GetMaxVelocity()/10;  
        return steering;  
    }
    private float RandomBinomial(){//random entre -1 e 1
        return Random.value - Random.value;
    }
    private Vector3 OrientationToVector(float orientation){  
        return new Vector3(Mathf.Cos(orientation), 0,Mathf.Sin(orientation));  
    }
    private Vector3 DoAvoidObstacle(){
        Vector3 steeringForce;
        float sphereRadius = steeringAgent.GetSphereRadius();
        float charHeight = steeringAgent.GetCharHeight();
        RaycastHit hit;
        LayerMask obstaclesLayerMask = steeringAgent.GetObstaclesLayerMask();
        Vector3 sphereCastOrigin = rb.transform.position + new Vector3(0,charHeight/2,0);
        Vector3 rightVel = Quaternion.Euler(0,30,0)*rb.transform.forward;
        Vector3 leftVel = Quaternion.Euler(0,-30,0)*rb.transform.forward;
        Vector3 desiredVel = Vector3.zero;
        float raycastDist = MaxRaycastDist*sphereRadius/2;
        Debug.DrawRay(rb.transform.position,rightVel*raycastDist,Color.yellow);
        Debug.DrawRay(rb.transform.position,rb.transform.forward*raycastDist,Color.blue);
        Debug.DrawRay(rb.transform.position,leftVel*raycastDist,Color.green);
        //if(Physics.CheckSphere(sphereCastOrigin,sphereRadius,obstaclesLayerMask)){
            //Debug.Log("O checkSphere deu positivo");
            Collider[] overlapColliders = new Collider[3];
            int numOverlap;
            if((numOverlap=Physics.OverlapSphereNonAlloc(sphereCastOrigin,sphereRadius,overlapColliders,obstaclesLayerMask))>0){
                for(int i =0;i<numOverlap;i++){
                    Debug.Log("Algo dentro de mim");
                    Vector3 colliderPos = new Vector3(overlapColliders[i].transform.position.x,rb.transform.position.y+charHeight/2,overlapColliders[i].transform.position.z);
                    Vector3 myTranform = new Vector3(rb.transform.position.x,rb.transform.position.y+charHeight/2,rb.transform.position.z);
                    Vector3 touchDir=myTranform-colliderPos;
                    Vector3 target = colliderPos+ touchDir.normalized*(sphereRadius*1.5f);
                    Debug.DrawLine(colliderPos,target);
                    desiredVel += target - steeringAgent.GetPosition();
                }
            }
        //}
        else{
            if(Physics.SphereCast(sphereCastOrigin,sphereRadius,rb.transform.forward,out hit,raycastDist,obstaclesLayerMask)){
                //Debug.Log("Açgp na minha frent");
                Vector3 target = hit.point + hit.normal*(sphereRadius+1);
                Debug.DrawLine(hit.point,target);
                desiredVel += target - steeringAgent.GetPosition();
            }
            if(Physics.SphereCast(sphereCastOrigin,sphereRadius,rightVel,out hit,raycastDist,obstaclesLayerMask)){
                //Debug.Log("Açgp na minha dir");
                Vector3 target = hit.point + hit.normal*(sphereRadius+1);
                Debug.DrawLine(hit.point,target);
                desiredVel += target - steeringAgent.GetPosition();
            }
            if(Physics.SphereCast(sphereCastOrigin,sphereRadius,leftVel,out hit,raycastDist,obstaclesLayerMask)){
                //Debug.Log("Açgp na minha esq");
                Vector3 target = hit.point + hit.normal*(sphereRadius+1);
                Debug.DrawLine(hit.point,target);
                desiredVel += target - steeringAgent.GetPosition();
            }
        }
        desiredVel=desiredVel.normalized*steeringAgent.GetMaxVelocity();
        steeringForce = desiredVel*10 - steeringAgent.GetVelocity();
        return steeringForce;
    }
	private Vector3 DoEvade(ISteeringAgent targetAgent){
        Vector3 distance = targetAgent.GetPosition() - steeringAgent.GetPosition();
        int updatesAhead = (int) (distance.magnitude / steeringAgent.GetMaxVelocity());
        Vector3 futurePosition = targetAgent.GetPosition() + targetAgent.GetVelocity() * updatesAhead;
        return DoFlee(futurePosition);
    }
	private Vector3 DoPursuit(ISteeringAgent targetAgent){
        Vector3 distance = targetAgent.GetPosition() - steeringAgent.GetPosition();
        int updatesAhead = (int) (distance.magnitude / steeringAgent.GetMaxVelocity());
        Vector3 futurePosition = targetAgent.GetPosition() + targetAgent.GetVelocity() * updatesAhead;
        return DoSeek(futurePosition);
    }

}
/* EXEMPLO DE COMO USAR
public class Enemy : MonoBehaviour, ISteeringAgent
{
    [SerializeField] float maxVelocity;
    [SerializeField] float maxForce;
    [SerializeField]GameObject seekTarget;
    [SerializeField]GameObject fleeTarget;
    [SerializeField]GameObject pursuitTarget;
    [SerializeField]GameObject evadeTarget;
    [SerializeField]float minSlowDist;
    ISteeringAgent pursuitTargetAgent;
    ISteeringAgent evadeTargetAgent;
    SteeringManager steeringManager;
    [Header("Raycast Info")]
    [SerializeField] float charHeight;
    [SerializeField] float charWidth;
    [SerializeField]LayerMask obstaclesLayerMask;
    Rigidbody rb;

    public float GetMaxVelocity()
    {
        return maxVelocity;
    }

    public Vector3 GetPosition()
    {
       return transform.position;
    }

    public Vector3 GetVelocity()
    {
        return new Vector3(rb.velocity.x,0,rb.velocity.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(!rb)Debug.LogWarning("O inimigo da classe Enemy não tem o rigidbody dele");
        steeringManager = new SteeringManager(this,rb);
        if(evadeTarget!=null)evadeTargetAgent=evadeTarget.GetComponent<ISteeringAgent>();
        if(pursuitTarget!=null)pursuitTargetAgent=pursuitTarget.GetComponent<ISteeringAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(seekTarget!=null)steeringManager.Seek(seekTarget.transform.position,minSlowDist);
        if(fleeTarget!=null)steeringManager.Flee(fleeTarget.transform.position);
        if(evadeTarget!=null)steeringManager.Evade(evadeTargetAgent);
        if(pursuitTarget!=null)steeringManager.Pursuit(pursuitTargetAgent);
        steeringManager.AvoidObstacle();
        steeringManager.Update();
    }

    public float GetMaxForce()
    {
        return maxForce;
    }

    public float GetSphereRadius(){
        return Mathf.Max(charHeight,charWidth)/2;
    }
    public float GetCharHeight(){
        return charHeight;
    }
    public LayerMask GetObstaclesLayerMask(){
        return obstaclesLayerMask;
    }
}
*/