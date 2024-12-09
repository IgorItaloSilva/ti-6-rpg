using UnityEngine;


public class KitsuneRangeAttack : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform[] bulletPos;
    [SerializeField] GameObject rangeBullet;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0;  i < bulletPos.Length; i++)
        {
            GameObject clone = Instantiate(rangeBullet, bulletPos[i].position, bulletPos[i].rotation);
            //clone.GetComponent<KitsuneBullet>().SetPlayer(player);


        }
    }
}