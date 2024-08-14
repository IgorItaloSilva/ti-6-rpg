using UnityEngine;
using UnityEngine.Events;



[DefaultExecutionOrder(-99)]
public class GameManager : MonoBehaviour
{
    [Header("Referências: ")]
    public static GameManager instance;
    public AudioManager audioManager;
    public CameraMovement camMove;
    public Transform camTarget;

    [Header("Parâmetros Player: ")]
    [SerializeField] int playerLife;

    [Header("Variáveis de controle")]
    [SerializeField] bool inGame;


    // Teste
    public UnityEvent shakeEffect;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void PlayerDamage(int value) // Jogador perde vida passando valor do dano como parâmetro
    {
        playerLife -= value;
        if(value < 0)
        {
            GameOver();
        }
    }

    public void GameOver() // Vida do jogador zerou
    {

    }


}