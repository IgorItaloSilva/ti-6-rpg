using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


[DefaultExecutionOrder(-99)]
public class GameManager : MonoBehaviour
{
    // Singleton do GameManager IMPORTANTE
    public static GameManager gm;
    
    [Header("Refer�ncias:")] 
    public AudioManager audioManager;

    [Header("Par�metros Player: ")] [SerializeField]
    private int playerHP;

    [Header("Vari�veis de controle")] [SerializeField]
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

    public void PlayerDamage(int value) // Jogador perde vida passando valor do dano como par�metro
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

    // M�todo para definir visibilidade do cursor
    public void ToggleCursor(bool show)
    {
        Cursor.visible = show;
        Cursor.lockState = show ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    // M�todo para alternar visibilidade do cursor sem precisar definir.
    public void ToggleCursor()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked
            ? CursorLockMode.Confined
            : CursorLockMode.Locked;
    }
}