using System.Collections;
using UnityEngine;

public class wpn_KitsuneMagicAttack : EnemyBaseWeapon
{
    [SerializeField] GameObject[] bullets;
    Vector3[] positions;

    void Start()
    {
        positions = new Vector3[bullets.Length];
        for(int i = 0; i < bullets.Length; i++ )
            positions[i] = bullets[i].transform.localPosition;
    }

    protected override void OneExecution()
    {
        
        for(int i = 0; i < bullets.Length; i++){
            bullets[i].transform.SetParent(this.transform);
            bullets[i].SetActive(false);
        }
    }

    protected override IEnumerator MultipleExecution()
    {
        for(int i = 0; i < bullets.Length; i++) {
            yield return new WaitForSeconds(0.1f);
            bullets[i].transform.localPosition = positions[i];
            bullets[i].transform.SetParent(null);
            bullets[i].SetActive(true);

        }

    }

}