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
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dodgeAction;
    private InputAction sprintAction;


    [Header("Parametros: ")] private float jumpForce = 7500f;
    private float moveSpeed, turnTime;
    private const float dodgeDuration = 0.15f;
    private float turnSmoothSpeed, turnOrientation, smoothedTurnOrientation;
    private Vector3 moveDir;
    private Vector2 moveInput;
    private bool isGrounded, hasJumped;
    [SerializeField] private LayerMask groundLayers;

    private enum movementSystems
    {
        cc,
        rb
    }

    public enum moveStateTypes
    {
        idle,
        walking,
        sprinting,
        dodging,
        inAir,
        landing,
        attacking,
    }

    private moveStateTypes activeMoveState = moveStateTypes.walking;

    private readonly PlayerState walkState = new WalkState();
    private readonly PlayerState sprintState = new SprintState();
    private readonly PlayerState dodgeState = new DodgeState();
    private readonly PlayerState inAirState = new InAirState();
    private readonly PlayerState landState = new LandState();
    private readonly PlayerState attackState = new AttackState();

    private PlayerState activePlayerState;

    private void EnterPlayerState(PlayerState newState)
    {
        Debug.Log("Exiting: " + activeMoveState + ", entering: " + newState.MoveState);
        activePlayerState = newState;
        activeMoveState = newState.MoveState;
        moveSpeed = newState.MoveSpeed;
        turnTime = newState.TurnTime;
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

        if (isGrounded)
        {
            if (sprintAction.triggered) SprintAsync();
            if (jumpAction.triggered) Jump();
            if (dodgeAction.triggered) DodgeAsync();
        }
        
        if (!isGrounded && !hasJumped && activeMoveState != moveStateTypes.inAir)
        {
            SwitchMovements(movementSystems.rb);
            LandAsync();
        }
    }

    private void FixedUpdate()
    {
        if ((moveInput.magnitude > 0.01f)) Move();

        cinemachine.m_RecenterToTargetHeading.m_enabled = moveInput is not { x: 0f, y: < 0f };
    }

    private void Move()
    {
        // Calcular direção resultante do input do player e rotacionar ele na direção para onde está indo.
        turnOrientation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        smoothedTurnOrientation = Mathf.SmoothDampAngle(transform.eulerAngles.y, turnOrientation, ref turnSmoothSpeed, turnTime);

        // Aplicar movimentação multiplicando pela velocidade do player:
        moveDir = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;

        // Rotacionar a direção do player
        transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);

        // Mover com character controller quando estiver no chão e com o rigidbody quando estiver no ar
        if (isGrounded && cc.enabled)
        {
            
            cc.Move(transform.forward * (moveSpeed * Time.deltaTime));
        }
        else
        {
            rb.AddForce(moveDir * moveSpeed);
        }
    }

    private void Jump()
    {
        Debug.Log("Jumping");
        hasJumped = true;
        SwitchMovements(movementSystems.rb);
        rb.AddForce(moveDir * moveSpeed + Vector3.up * jumpForce);
        LandAsync();
    }

    private async Task LandAsync()
    {
        EnterPlayerState(inAirState);
        await Task.Delay(100);
        while (!isGrounded)
            await Task.Yield();
        SwitchMovements(movementSystems.cc);
        EnterPlayerState(landState);
        await Task.Delay(250);
        EnterPlayerState(sprintAction.inProgress ? sprintState : walkState);
        hasJumped = false;
    }

    private async Task SprintAsync()
    {
        EnterPlayerState(sprintState);
        while (sprintAction.inProgress)
            await Task.Yield();
        EnterPlayerState(walkState);
    }

    private async Task DodgeAsync()
    {
        EnterPlayerState(dodgeState);
        Debug.Log("Dodge");
        await Task.Delay((int)(dodgeDuration * 1000f));
        EnterPlayerState(walkState);
    }

    private void SwitchMovements(movementSystems ms)
    {
        switch (ms)
        {
            case movementSystems.cc when !cc.enabled:
                if (playerMesh) playerMesh.material.color = Color.red;
                cc.enabled = true;
                rb.isKinematic = true;
                col.enabled = false;
                break;
            case movementSystems.rb when rb.isKinematic:
                if (playerMesh) playerMesh.material.color = Color.blue;
                rb.isKinematic = false;
                rb.velocity = cc.velocity;
                cc.enabled = false;
                col.enabled = true;
                break;
            default:
                Debug.LogWarning(ms + " is already enabled!");
                return;
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