using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


[DefaultExecutionOrder(-99)]
public class GameManager : MonoBehaviour
{
    // Singleton do GameManager IMPORTANTE
    public static GameManager gm;
    
    [Header("Referências:")] 
    public AudioManager audioManager;

    [Header("Parâmetros Player: ")] [SerializeField]
    private int playerHP;

    [Header("Variáveis de controle")] [SerializeField]
    private bool inGame;

    // Start is called before the first frame update
    private void Start()
    {
        ToggleCursor(false);
        if (gm)
            Destroy(gameObject);
        else
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayerDamage(int value) // Jogador perde vida passando valor do dano como parâmetro
    {
        playerHP -= value;
        if (playerHP <= 0)
        {
            GameOver();
        }
    }

    public void GameOver() // Vida do jogador zerou
    {
    }

    // Método para definir visibilidade do cursor
    public void ToggleCursor(bool show)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    // Método para alternar visibilidade do cursor sem precisar definir.
    public void ToggleCursor()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
            ? CursorLockMode.Confined
            : CursorLockMode.Locked;
    }
}