using System.Collections;
using UnityEngine;


enum CamEffect
{
    Follow,
    Shake,
    Zoom
}


public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float posSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private Transform target;
    private float posY;
    [SerializeField] CamEffect effect;

    // Variaveis de controle
    Vector3 shakePos;
    float shakeSpeed = 1;
    float cdEffect = 0;
    bool inEffect = false;
    Coroutine coroutineRun;



    private void Start()
    {
        if (!GameManager.instance.camMove)
        {
            GameManager.instance.camMove = this;
            target = GameManager.instance.camTarget;
            GameManager.instance.shakeEffect.AddListener(ShakeCam);
        }else
            Destroy(gameObject);
        
    }

    void ShakeCam()
    {
        if (!inEffect)
        {
            inEffect = true;
            effect = CamEffect.Shake;
            coroutineRun = StartCoroutine("CDEffect", 0.5f);
            Shaking();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (GameManager.instance.camTarget)
            if(effect == CamEffect.Shake)
                Shaking();
            Following();
    }

    private void Following()
    {
        transform.position = Vector3.Slerp(transform.position, target.position + shakePos, posSpeed * shakeSpeed); // Atualiza a posição da camera de acordo com o alvo(target).
        transform.localRotation = Quaternion.Slerp(transform.rotation, target.rotation, rotSpeed); // Atualiza a rotação da camera de acordo com o alvo(target).
    }

    private void Shaking() // Sistema de tremer camera
    {
        if (cdEffect > 0.045f)
        {
            shakePos = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));
            shakeSpeed = 1.5f;
            cdEffect = 0;
        }
        cdEffect += Time.fixedDeltaTime;
    }

    private IEnumerator CDEffect(float value) // CountDown do sistema em andamento
    {
        yield return new WaitForSeconds(value);
        effect = CamEffect.Follow;
        shakePos = Vector3.zero;
        shakeSpeed = 1;
        inEffect = false;
    }


}
