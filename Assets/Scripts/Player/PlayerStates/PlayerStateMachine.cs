#region Imports

using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

#endregion

public class PlayerStateMachine : MonoBehaviour, IDataPersistence
{
    #region Singleton
    
    // Singleton publico do PlayerMovement
    public static PlayerStateMachine Instance;
    private void CreateSingleton()
    {
        if (Instance)
            Destroy(this); // Deletar novo objeto caso playerMovement já tenha sido instanciado
        else
            Instance = this; // Instanciar PlayerMovement caso não exista
    }

    #endregion

    #region References
    [Header("Referencias")]
    [HideInInspector] public CinemachineFreeLook cinemachine;
    private Camera mainCam;
    private Animator animator;
    private CharacterController cc;
    [SerializeField] private PlayerWeapon swordWeaponManager;
    private PlayerInput playerInput;
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    #endregion

    #region Private Variables

    private float _gravity, _initialJumpVelocity, _camYSpeed, _camXSpeed, _acceleration;

    private Vector3 _currentMovement, _appliedMovement;
    private Vector2 _currentMovementInput, _currentLookInput;

    private bool
        _hasJumped,
        _isMovementPressed,
        _isSprintPressed,
        _isJumping,
        _isJumpPressed,
        _isDodgePressed,
        _isAttackPressed,
        _isInteractPressed,
        _canInteract = true,
        _isDodging,
        _canDodge = true,
        _canJump = true,
        _canAttack = true,
        _isBetweenAttacks,
        _isClimbing,
        _canMount = true;

    private byte _attackCount;

    #endregion

    #region Public Variables

    public readonly int DodgeCooldownMs = 1500;
    public readonly byte BaseMoveSpeed = 5;
    public readonly float MaxJumpHeight = .75f,
        MaxJumpTime = .6f,
        BaseGravity = -9.8f,
        BaseTurnTime = 0.15f,
        SlowTurnTimeModifier = 1.5f;
    
    [FormerlySerializedAs("ShowStateLogs")] public bool ShowDebugLogs;

    #region AnimatorHashes
    public readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    public readonly int IsRunningHash = Animator.StringToHash("isRunning");
    public readonly int IsGroundedHash = Animator.StringToHash("isGrounded");
    public readonly int Attack1Hash = Animator.StringToHash("Attack1");
    public readonly int Attack2Hash = Animator.StringToHash("Attack2");
    public readonly int Attack3Hash = Animator.StringToHash("Attack3");
    public readonly int IsClimbingHash = Animator.StringToHash("isClimbing");
    public readonly int HasJumpedHash = Animator.StringToHash("hasJumped");
    public readonly int HasDodgedHash = Animator.StringToHash("hasDodged");
    public readonly int HasDiedHash = Animator.StringToHash("hasDied");
    public readonly int HasRespawnedHash = Animator.StringToHash("hasRespawned");
    public readonly int PlayerVelocityHash = Animator.StringToHash("playerVelocity");
    #endregion

    #endregion

    #region Public Getters

    public CharacterController CC => cc;
    public Animator Animator => animator;
    public Camera MainCam => mainCam;
    public Vector3 CurrentMovementInput => _currentMovementInput;
    public bool IsInteractPressed => _isInteractPressed && _canInteract;
    public bool IsMovementPressed => _isMovementPressed;
    public bool IsJumpPressed => _isJumpPressed && _canJump;
    public bool IsDodgePressed => _isDodgePressed && _canDodge;
    public bool IsAttackPressed => _isAttackPressed && _canAttack;
    public bool IsSprintPressed => _isSprintPressed;
    public bool IsClimbing => _isClimbing;
    public int AttackCount => _attackCount;
    public float InitialJumpVelocity => _initialJumpVelocity;
    

    #endregion

    #region Public Setters

    public float Gravity
    {
        get => _gravity;
        set => _gravity = value;
    }

    public bool IsDodging
    {
        set => _isDodging = value;
    }

    public PlayerBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    public bool CanMount
    {
        get => _canMount;
        set => _canMount = value;
    }

    public bool CanJump
    {
        get => _canJump;
        set => _canJump = value;
    }

    public bool CanDodge
    {
        get => _canDodge;
        set => _canDodge = value;
    }

    public bool CanAttack
    {
        get => _canAttack;
        set => _canAttack = value;
    }

    public bool CanInteract
    {
        get => _canInteract;
        set => _canInteract = value;
    }

    public Vector3 CurrentMovement
    {
        get => _currentMovement;
        set => _currentMovement = value;
    }

    public float CurrentMovementY
    {
        get => _currentMovement.y;
        set => _currentMovement.y = value;
    }

    public Vector3 AppliedMovement
    {
        get => _appliedMovement;
        set => _appliedMovement = value;
    }
    
    public float AppliedMovementX
    {
        get => _appliedMovement.x;
        set => _appliedMovement.x = value;
    }
    
    public float AppliedMovementY
    {
        get => _appliedMovement.y;
        set => _appliedMovement.y = value;
    }
    public float AppliedMovementZ
    {
        get => _appliedMovement.z;
        set => _appliedMovement.z = value;
    }

    public float Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }

    #endregion

    #region Initializers

    #region Input Initializers

    private void OnEnable()
    {
        playerInput.Gameplay.Enable();
        GameEventsManager.instance.playerEvents.onPlayerDied += PlayerDied;
        GameEventsManager.instance.playerEvents.onPlayerRespawned += PlayerRespawned;
        GameEventsManager.instance.uiEvents.onPauseGame += LockCam;
        GameEventsManager.instance.uiEvents.onUnpauseGame += UnlockCam;
    }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
        GameEventsManager.instance.playerEvents.onPlayerDied -= PlayerDied;
        GameEventsManager.instance.playerEvents.onPlayerRespawned -= PlayerRespawned;
        GameEventsManager.instance.uiEvents.onPauseGame -= LockCam;
        GameEventsManager.instance.uiEvents.onUnpauseGame -= UnlockCam;
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
        playerInput.Gameplay.Attack.started += Attack;
        playerInput.Gameplay.Attack.canceled += Attack;
        playerInput.Gameplay.Interact.started += OnInteractPressed;
        playerInput.Gameplay.Interact.canceled += OnInteractPressed;
    }

    private void OnMovementPressed(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput is not { x: 0f, y: 0f };
        Animator.SetBool(IsWalkingHash, IsMovementPressed);
    }


    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        _isInteractPressed = context.ReadValueAsButton();
        _canInteract = true;
    }

    private void Attack(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
        _canAttack = true;
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        _isSprintPressed = context.ReadValueAsButton();
        Animator.SetBool(IsRunningHash, IsSprintPressed);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _canJump = true;
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
    }

    #endregion
    
    private void InitializeReferences()
    {
        playerInput = new PlayerInput();
        mainCam = Camera.main;
        cinemachine = mainCam?.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void InitializePlayerStates()
    {
        _states = new PlayerStateFactory(this);
        InitializeGroundedState();
    }

    private void InitializeGroundedState()
    {
        // Set player state to Grounded by default
        _currentState = _states.Grounded();
        _currentState.EnterState();
    }

    #endregion
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CreateSingleton();
        InitializeReferences();
        InitializePlayerStates();
        SetupInputCallbackContext();
        SetupJumpVariables();
        _camXSpeed = cinemachine.m_XAxis.m_MaxSpeed;
        _camYSpeed = cinemachine.m_YAxis.m_MaxSpeed;
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateState();
        cinemachine.m_RecenterToTargetHeading.m_enabled = _currentMovementInput is { x: not 0, y: > 0f };
    }

    private void Update()
    {
        _currentState.UpdateState();
    }

    #region Collisions / Triggers
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable") && _canMount)
        {
            if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.forward, 1.5f)) return;
            
            transform.rotation = other.gameObject.transform.rotation;
            CC.Move(-transform.forward * 0.1f);
            _isClimbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbable"))
        {
            _canMount = true;
            _isClimbing = false;
        }
    }
    
    #endregion

    #region Attacks / Weapons

    public void HandleAttack()
    {
        _canAttack = false;

        switch (_attackCount)
        {
            case 0:
                _attackCount = 1;
                animator.SetBool(Attack1Hash, true);
                break;
            case 1:
                _attackCount = 2;
                animator.SetBool(Attack2Hash, true);
                break;
            case 2:
                _attackCount = 3;
                animator.SetBool(Attack3Hash, true);
                break;
        }
    }

    public void ResetAttacks()
    {
        animator.SetBool(Attack1Hash, false);
        animator.SetBool(Attack2Hash, false);
        animator.SetBool(Attack3Hash, false);
        _attackCount = 0;
        
        if(ShowDebugLogs) Debug.LogWarning("RESET ATTACKS");
    }

    private void EnableSwordCollider()
    {
        swordWeaponManager.EnableCollider();
    }

    private void DisableSwordCollider()
    {
        swordWeaponManager.DisableCollider();
    }

    public void SetWeaponDamageType(int value)
    {
        swordWeaponManager.SetDamageType(value);
    }

    #endregion

    #region Savegame
    
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
    
    #endregion

    #region Player Death / Respawn

    private void PlayerDied()
    {
        _currentState.ExitState();
        _currentState = _states.Dead();
        _currentState.EnterState();
    }

    private void PlayerRespawned()
    {
        Debug.Log("Player respawnou");
        animator.ResetTrigger(HasRespawnedHash);
        animator.SetTrigger(HasRespawnedHash);
        InitializeGroundedState();
    }

    #endregion

    #region Camera
    
    private void LockCam()
    {
        cinemachine.m_YAxis.m_MaxSpeed = 0f;
        cinemachine.m_XAxis.m_MaxSpeed = 0f;
    }

    private void UnlockCam()
    {
        cinemachine.m_YAxis.m_MaxSpeed = _camYSpeed;
        cinemachine.m_XAxis.m_MaxSpeed = _camXSpeed;
    }
    
    #endregion
    
}