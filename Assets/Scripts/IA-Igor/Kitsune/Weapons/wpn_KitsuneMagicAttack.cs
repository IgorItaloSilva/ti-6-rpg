using System.Collections;
using UnityEngine;

public class wpn_KitsuneMagicAttack : EnemyBaseWeapon
{
    [SerializeField] GameObject[] bullets;
    Vector3[] positions;

    bool skillUsed;

    void Awake()
    {
        positions = new Vector3[bullets.Length];
        for (int i = 0; i < bullets.Length; i++) {
            positions[i] = bullets[i].transform.localPosition;
            bullets[i].SetActive(false);
        }
    }

    protected override void OneExecution()
    {
        skillUsed = false;
        for(int i = 0; i < bullets.Length; i++) {
            bullets[i].transform.SetParent(this.transform);
        }
        
    }

    protected override IEnumerator MultipleExecution()
    {
        for(int i = 0; i < bullets.Length; i++) {
            yield return new WaitForSeconds(0.2f);
            bullets[i].transform.localPosition = positions[i];
            bullets[i].transform.SetParent(null);
            bullets[i].SetActive(true);
            transform.LookAt(target);

        }
        skillUsed = true;

    }

    void FixedUpdate()
    {
        if(skillUsed){
            bool bulletsUsed = true;
            for(int i = 0; i < bullets.Length; i++)
                bulletsUsed = bullets[i].activeSelf ? false : bulletsUsed;
            
            if(bulletsUsed){
                gameObject.SetActive(false);
                skillUsed = false;
            }
        }

    }

}