using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{
    // Singleton publico do PlayerMovement
    public static PlayerStateMachine Instance;

    [Header("Referências: ")] [HideInInspector]
    public CinemachineFreeLook cinemachine;

    private Camera mainCam;
    private Animator animator;
    private CharacterController cc;

    [Header("Input: ")] private PlayerInput playerInput;
    private float _turnSmoothSpeed, _gravity, _turnTime, _initialJumpVelocity;

    public readonly float MaxJumpHeight = .15f,
        MaxJumpTime = .6f,
        BaseGravity = -0.05f,
        BaseTurnTime = 0.15f,
        SlowTurnTimeModifier = 2f;

    public readonly int DodgeCooldownMs = 600;

    private Vector3 _currentMovement, _appliedMovement;
    private Vector2 _currentMovementInput, _currentLookInput;

    private bool
        _hasJumped,
        _isMovementPressed,
        _isSprintPressed,
        _isJumping,
        _isJumpPressed,
        _isDodgePressed,
        _isDodging,
        _canDodge = true;

    // variáveis dos estados:
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    // GETTERS E SETTERS:
    public CharacterController CC => cc;

    public PlayerBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    public bool IsJumpPressed => _isJumpPressed;
    public bool IsDodgePressed => _isDodgePressed;
    public bool IsSprintPressed => _isSprintPressed;

    public bool CanDodge
    {
        get => _canDodge;
        set => _canDodge = value;
    }

    public bool IsJumping { get; set; }

    public float TurnTime
    {
        set => _turnTime = value;
    }

    public Vector3 CurrentMovement => _currentMovement;

    public Vector3 AppliedMovement
    {
        get => _appliedMovement;
        set => _appliedMovement = value;
    }

    public float CurrentMovementY
    {
        get => _currentMovement.y;
        set => _currentMovement.y = value;
    }

    public float AppliedMovementY
    {
        get => _appliedMovement.y;
        set => _appliedMovement.y = value;
    }

    public float InitialJumpVelocity => _initialJumpVelocity;

    public float Gravity => _gravity;

    private void CreateSingleton()
    {
        if (Instance)
            Destroy(this); // Deletar novo objeto caso playerMovement já tenha sido instanciado
        else
            Instance = this; // Instanciar PlayerMovement caso não exista
    }

    private void OnEnable()
    {
        playerInput.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
    }

    private void SetupInputCallbackContext()
    {
        playerInput.Gameplay.Move.started += OnMovementPressed;
        playerInput.Gameplay.Move.canceled += OnMovementPressed;
        playerInput.Gameplay.Move.performed += OnMovementPressed;
        playerInput.Gameplay.Sprint.started += Sprint;
        playerInput.Gameplay.Sprint.canceled += Sprint;
        playerInput.Gameplay.Jump.started += Jump;
        playerInput.Gameplay.Jump.canceled += Jump;
        playerInput.Gameplay.Dodge.started += Dodge;
        playerInput.Gameplay.Dodge.canceled += Dodge;
    }

    private void OnMovementPressed(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput is not { x: 0f, y: 0f };
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        _isSprintPressed = context.ReadValueAsButton();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }

    private void Dodge(InputAction.CallbackContext context)
    {
        _isDodgePressed = context.ReadValueAsButton();
    }

    private void SetupJumpVariables()
    {
        var timeToApex = MaxJumpTime / 2;
        _gravity = (-2 * MaxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * MaxJumpHeight) / timeToApex;
        Debug.Log(_initialJumpVelocity);
    }

    private void Awake()
    {
        CreateSingleton();
        playerInput = new PlayerInput();
        mainCam = Camera.main;
        cinemachine = mainCam?.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();
        cc = GetComponent<CharacterController>();

        SetupPlayerStates();

        SetupInputCallbackContext();

        SetupJumpVariables();

        SceneManager.LoadSceneAsync("Hud", LoadSceneMode.Additive);
    }

    private void SetupPlayerStates()
    {
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    private void HandleRotation()
    {
        // Calcular direção resultante do input do player e rotacionar ele na direção para onde está indo.
        var turnOrientation = Mathf.Atan2(_currentMovementInput.x, _currentMovementInput.y) * Mathf.Rad2Deg +
                              mainCam.transform.eulerAngles.y;
        var smoothedTurnOrientation =
            Mathf.SmoothDampAngle(transform.eulerAngles.y, turnOrientation, ref _turnSmoothSpeed, _turnTime);

        if (!_isMovementPressed) return;
        // Aplicar movimentação multiplicando pela velocidade do player:
        var groundedMovement = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;

        _currentMovement = new Vector3(groundedMovement.x, _currentMovement.y, groundedMovement.z);

        // Rotacionar a direção do player
        transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);
    }

    // PRECISA SAIR DAQUI E IR PARA O MOVESTATE!!!!
    private void FixedUpdate()
    {
        cinemachine.m_RecenterToTargetHeading.m_enabled = _currentMovementInput is { x: not 0, y: > 0f };
    }

    private void Update()
    {
        HandleRotation();
        _currentState.UpdateState();
    }

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