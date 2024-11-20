using System;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerStateMachine : MonoBehaviour
{
    // Singleton publico do PlayerMovement
    public static PlayerStateMachine Instance;

    [HideInInspector] public CinemachineFreeLook cinemachine;

    private Camera mainCam;
    private Animator animator;
    private CharacterController cc;
    [SerializeField] private WeaponManager swordWeaponManager;
    private PlayerInput playerInput;
    private float _turnSmoothSpeed, _gravity, _turnTime, _initialJumpVelocity;

    public readonly float MaxJumpHeight = .25f,
        MaxJumpTime = .8f,
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
        _isAttackPressed,
        _isAttacking,
        _isDodging,
        _canDodge = true,
        _canJump = true,
        _canAttack = true,
        _isClimbing,
        _canMount = true;

    private byte _attackCount, _currentAttack;

    // variáveis dos estados:
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    public readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    public readonly int IsRunningHash = Animator.StringToHash("isRunning");
    public readonly int IsGroundedHash = Animator.StringToHash("isGrounded");
    public readonly int AttackCountHash = Animator.StringToHash("AttackCount");
    public readonly int IsClimbingHash = Animator.StringToHash("isClimbing");
    public readonly int HasJumpedHash = Animator.StringToHash("hasJumped");
    public readonly int PlayerVelocity = Animator.StringToHash("playerVelocity");

    // GETTERS E SETTERS:
    public CharacterController CC => cc;

    public Animator Animator => animator;

    public PlayerBaseState CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }
    public bool IsMovementPressed => _isMovementPressed;
    public bool IsJumpPressed => _isJumpPressed && _canJump;
    public bool IsDodgePressed => _isDodgePressed && _canDodge;
    public bool IsAttackPressed => _isAttackPressed && _canAttack;
    public bool IsSprintPressed => _isSprintPressed;
    public bool IsClimbing => _isClimbing;
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

    public float TurnTime
    {
        set => _turnTime = value;
    }

    public Vector3 CurrentMovement => _currentMovement;

    public Vector3 CurrentMovementInput => _currentMovementInput;

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

    public int AttackCount
    {
        get => _attackCount;
    }

    public int CurrentAttack => _currentAttack;

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
        playerInput.Gameplay.Attack.started += Attack;
        playerInput.Gameplay.Attack.canceled += Attack;
    }

    private void OnMovementPressed(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput is not { x: 0f, y: 0f };
        HandleMovementAnimatorParameters();
    }
    
    private void HandleMovementAnimatorParameters()
    {
        Animator.SetBool(IsWalkingHash, IsMovementPressed);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
        _canAttack = true;
    }

    private void Sprint(InputAction.CallbackContext context)
    {
        _isSprintPressed = context.ReadValueAsButton();
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
        Debug.Log(_initialJumpVelocity);
    }

    private void Awake()
    {
        CreateSingleton();
        SetupReferences();
        SetupPlayerStates();
        SetupInputCallbackContext();
        SetupJumpVariables();
        SceneManager.LoadSceneAsync("Hud", LoadSceneMode.Additive);
    }

    private void SetupReferences()
    {
        playerInput = new PlayerInput();
        mainCam = Camera.main;
        cinemachine = mainCam?.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();
        cc = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
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
        var turnOrientation = Mathf.Atan2(_currentMovementInput.x, _currentMovementInput.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        var smoothedTurnOrientation = Mathf.SmoothDampAngle(transform.eulerAngles.y, turnOrientation, ref _turnSmoothSpeed, _turnTime);
        
        if (!_isMovementPressed) return;
        
        // Aplicar movimentação baseado na rotação do 
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
        
        
        if (!_isClimbing) HandleRotation();
        _currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable") && _canMount && other.transform.position.y > transform.position.y)
        {
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

    public void HandleAttack()
    {
        if (!_canAttack) return;

        _isAttacking = true;

        _currentMovement = Vector3.zero;

        _canAttack = false;

        if (_attackCount == 3 && _currentAttack == 3) return;

        if (_attackCount == _currentAttack)
            _attackCount++;
        ApplyAttackCount();
    }

    public void ResetAttacks()
    {
        if (_attackCount > _currentAttack) return;
        _isAttacking = false;
        _attackCount = 0;
        _currentAttack = 0;
        ApplyAttackCount();
    }

    private void ApplyAttackCount()
    {
        animator.SetInteger(AttackCountHash, _attackCount);
    }

    public void AttackStarted()
    {
        _currentAttack++;
    }

    private void EnableSwordCollider()
    {
        swordWeaponManager.EnableCollider();
    }

    private void DisableSwordCollider()
    {
        swordWeaponManager.DisableCollider();
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