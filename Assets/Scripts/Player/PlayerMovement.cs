using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Singleton publico do PlayerMovement
    public static PlayerMovement instance;

    [Header("Referências: ")] [SerializeField]
    private Rigidbody rb;

    private CinemachineFreeLook cinemachine;
    private Camera mainCam;
    private Animator animator;
    [SerializeField] private MeshRenderer playerMesh;
    [SerializeField] private CharacterController cc;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private PhysicMaterial physicMat;

    [Header("Input: ")] [SerializeField] private PlayerInput playerInput;
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction dodgeAction;
    public InputAction sprintAction;


    [Header("Parametros: ")] private float jumpForce = 6000f;
    private float moveSpeed, turnTime;
    private const float dodgeDuration = 0.15f;
    private float turnSmoothSpeed, turnOrientation, smoothedTurnOrientation;
    private Vector3 moveDir;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;
    private bool isGrounded { get; set; }
    private bool hasJumped;
    public bool IsGrounded => isGrounded;

    [SerializeField] private LayerMask groundLayers;

    #region MoveStates

    public enum moveTypes
    {
        idle,
        walking,
        sprinting,
        dodging,
        inAir,
        landing,
        attacking,
    }

    private moveTypes activeMoveType = moveTypes.walking;

    private readonly PlayerState walkState = new WalkState();
    private readonly PlayerState sprintState = new SprintState();
    private readonly PlayerState dodgeState = new DodgeState();
    private readonly PlayerState inAirState = new InAirState();
    private readonly PlayerState landState = new LandState();
    private readonly PlayerState attackState = new AttackState();

    private PlayerState activePlayerState;

    private void EnterPlayerState(PlayerState newState)
    {
        Debug.Log("\nExiting: " + activeMoveType + "\nEntering: " + newState.moveType);
        activePlayerState = newState;
        activeMoveType = newState.moveType;
        moveSpeed = newState.MoveSpeed;
        turnTime = newState.TurnTime;
        activePlayerState.Enter();
    }

    #endregion

    private void CreateSingleton()
    {
        if (instance)
            Destroy(this); // Deletar novo objeto caso playerMovement já tenha sido instanciado
        else
            instance = this; // Instanciar PlayerMovement caso não exista
    }


    private void HandleActions()
    {
        // Definir input do player:
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        dodgeAction = playerInput.actions.FindAction("Dodge");
        sprintAction = playerInput.actions.FindAction("Sprint");
    }

    private void Awake()
    {
        EnterPlayerState(walkState);

#if UNITY_EDITOR
        if (!UIManager.instance)
            SceneManager.LoadSceneAsync("Hud", LoadSceneMode.Additive);
#endif

        CreateSingleton();

        mainCam = Camera.main;
        cinemachine = mainCam?.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();

        HandleActions();

        Physics.gravity *= 2.5f;

        if (Debug.isDebugBuild)
            Debug.developerConsoleVisible = true;
    }

    private void Update()
    {
        // Ler valores de movimento do InputActionReference 
        moveInput = moveAction.ReadValue<Vector2>().normalized;

#if UNITY_EDITOR || DEBUG
        if (Keyboard.current.lKey.wasPressedThisFrame)
            GameEventsManager.instance.playerEvents.PlayerDied();
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            DataPersistenceManager.instance.SaveGame();
            GameEventsManager.instance.uiEvents.SavedGame();
        }
#endif
        // Raycast para baixo para checar se o player está no chão
        isGrounded = Physics.Raycast(transform.position, Vector3.down, cc.height / 2 + cc.stepOffset + 0.05f,
            groundLayers);

        if (!isGrounded && !hasJumped)
        {
            SwitchMovements(toCC: false);
            EnterPlayerState(inAirState);
        }

        activePlayerState.Update();
    }

    private void FixedUpdate()
    {
        activePlayerState.FixedUpdate();

        cinemachine.m_RecenterToTargetHeading.m_enabled = moveInput is not { x: 0f, y: < 0f };
    }

    public void Move()
    {
        // Calcular direção resultante do input do player e rotacionar ele na direção para onde está indo.
        turnOrientation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        smoothedTurnOrientation =
            Mathf.SmoothDampAngle(transform.eulerAngles.y, turnOrientation, ref turnSmoothSpeed, turnTime);

        // Aplicar movimentação multiplicando pela velocidade do player:
        moveDir = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;

        // Rotacionar a direção do player
        transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);

        // Mover com character controller quando estiver no chão e com o rigidbody quando estiver no ar
        if (isGrounded && cc.enabled)
        {
            cc.Move(moveDir * (moveSpeed * Time.fixedDeltaTime));
        }
        else
        {
            rb.AddForce(moveDir * (moveSpeed * Time.fixedDeltaTime));
        }
    }

    public void Jump()
    {
        Debug.Log("Jumping");
        SwitchMovements(toCC: false);
        EnterPlayerState(inAirState);
        rb.AddForce(0f, jumpForce, 0f);
        hasJumped = true;
    }

    public async Task LandAsync()
    {
        Debug.Log("Landing started!");
        await Task.Delay(100);
        while (!isGrounded) await Task.Yield();

        EnterPlayerState(landState);
        await Task.Delay(150);
        EnterPlayerState(sprintAction.inProgress ? sprintState : walkState);
        hasJumped = false;
    }

    public async Task SprintAsync()
    {
        Debug.Log("Sprint started!");
        EnterPlayerState(sprintState);
        while (sprintAction.inProgress)
            await Task.Yield();
        EnterPlayerState(walkState);
    }

    public async Task DodgeAsync()
    {
        Debug.Log("Dodge started!");
        EnterPlayerState(dodgeState);
        Debug.Log("Dodge");
        await Task.Delay((int)(dodgeDuration * 1000f));
        EnterPlayerState(walkState);
    }

    public void SwitchMovements(bool toCC)
    {
        if (toCC)
        {
            if (playerMesh) playerMesh.material.color = Color.red;
            cc.enabled = true;
            rb.isKinematic = true;
            col.enabled = false;
        }
        else
        {
            if (playerMesh) playerMesh.material.color = Color.blue;
            cc.enabled = false;
            rb.isKinematic = false;
            rb.velocity = new Vector3(cc.velocity.x, rb.velocity.y, cc.velocity.z);
            col.enabled = true;
        }
    }

    //Chamado após a cena ser carregada
    public void LoadData(GameData gameData)
    {
        transform.position = gameData.pos;
        Physics.SyncTransforms();
    }

    //Chamado manualmente para salvar o jogo
    public void SaveData(GameData gameData)
    {
        gameData.pos = transform.position;
    }
}