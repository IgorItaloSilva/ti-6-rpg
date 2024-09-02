using UnityEngine;


public class KitsuneActions : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        IEnemyAction[] allAction = new IEnemyAction[]
        {
            gameObject.AddComponent<KitsuneIdle>(),
            gameObject.AddComponent<KitsuneAttack>(),
            gameObject.AddComponent<KitsuneDash>()

        };
        GetComponent<EnemyBehaviour>().action = allAction;
        Destroy(this);
    }
}
