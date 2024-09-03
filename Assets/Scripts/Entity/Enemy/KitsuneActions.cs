using UnityEngine;


public class KitsuneActions : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        AEnemyAction[] allAction = new AEnemyAction[]
        {
            new KitsuneDash(),
            new KitsuneAttack()

        };
        GetComponent<EnemyBehaviour>().SetActions(allAction);
        Destroy(this);
    }
}
