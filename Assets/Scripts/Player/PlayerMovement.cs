using System;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Singleton publico do PlayerMovement
    public static PlayerMovement Instance;

    [Header("Referências: ")] 
    
    [HideInInspector] public CinemachineFreeLook cinemachine;
    private Camera mainCam;

    private Animator animator;
    private CharacterController cc;

    [Header("Input: ")] 
    
    private PlayerInput playerInput;
    private float _turnSmoothSpeed, _gravity, _initialJumpVelocity, _turnTime = TurnTime;
    private const float MaxJumpHeight = .6f, MaxJumpTime = .75f, MoveSpeed = 10f, SprintSpeedModifier = 2f, DodgeSpeedMultiplier = 4f, GroundedGravity = -0.05f, TurnTime = 0.15f, SprintTurnTimeModifier = 3f;
    private Vector3 _currentMovement;
    private Vector2 _currentMovementInput;
    private bool _hasJumped, _isMovementPressed, _isSprintPressed, _isJumpPressed, _isJumping, _isDodgePressed, _isDodging;

    [SerializeField] private LayerMask groundLayers;


    #region InputSystemSetup

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
    }

    private void OnMovementPressed(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput is not { x: 0f, y: 0f };
    }

    private void OnEnable()
    {
        playerInput.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerInput.Gameplay.Disable();
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
    
    #endregion

    #region Awake

    private void CreateSingleton()
    {
        if (Instance)
            Destroy(this); // Deletar novo objeto caso playerMovement já tenha sido instanciado
        else
            Instance = this; // Instanciar PlayerMovement caso não exista
    }

    private void PrepareJumpVariables()
    {
        var _timeToApex = MaxJumpTime / 2;
        _gravity = (-2 * MaxJumpHeight) / Mathf.Pow(_timeToApex, 2);
        _initialJumpVelocity = (2 * MaxJumpHeight) / _timeToApex;
    }
    
    private void Awake()
    {
        CreateSingleton();
        playerInput = new PlayerInput();
        mainCam = Camera.main;
        
        SetupInputCallbackContext();

        cinemachine = mainCam?.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();
        cc = GetComponent<CharacterController>();

        PrepareJumpVariables();
        
#if UNITY_EDITOR
        if (!UIManager.instance)
            SceneManager.LoadSceneAsync("Hud", LoadSceneMode.Additive);
#endif
    }

    #endregion

    
    
    #region Update
    private void HandleJump()
    {
        switch (_isJumpPressed)
        {
            case true when !_isJumping && cc.isGrounded:
                _turnTime = TurnTime * SprintTurnTimeModifier;
                _isJumping = true;
                _currentMovement.y += _initialJumpVelocity * 0.5f;
                break;
            case false when _isJumping && cc.isGrounded:
                _turnTime = TurnTime;
                _isJumping = false;
                break;
        }
    }

    private async void ResetDodge(int ms = 150)
    {
        await Task.Delay(ms);
        _isDodgePressed = false;
        _isDodging = false;
    }
    
    private void HandleDodge()
    {
        if (_isDodgePressed && (!_isSprintPressed && !_isDodging && !_isJumping && cc.isGrounded))
        {
            _isDodging = true;
            ResetDodge();
        }
    }

    private void FixedUpdate()
    {
        cinemachine.m_RecenterToTargetHeading.m_enabled = _currentMovementInput is { x: not 0, y: > 0f };
    }

    private void HandleMove()
    {
        if ((_isSprintPressed || !cc.isGrounded) && _isMovementPressed)
        {
            _currentMovement.x = _isSprintPressed ? transform.forward.x * SprintSpeedModifier : transform.forward.x;
            _currentMovement.z = _isSprintPressed ? transform.forward.z * SprintSpeedModifier : transform.forward.z;
            _turnTime = TurnTime * SprintTurnTimeModifier;
        }
        else if (_isDodging)
        {
            _currentMovement.x *= DodgeSpeedMultiplier;
            _currentMovement.z *= DodgeSpeedMultiplier;
        }
        else
        {
            _turnTime = TurnTime;
        }
        // Aplicar direção e rotação só caso o player esteja se movendo
        cc.Move(_currentMovement * (MoveSpeed * Time.deltaTime));
    }

    private void HandleGravity()
    {
        if (cc.isGrounded) _currentMovement.y = GroundedGravity;
        else
        {
            var previousYVelocity = _currentMovement.y;
            var newYVelocity = _currentMovement.y + (_gravity * Time.deltaTime);
            var nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            _currentMovement.y = nextYVelocity;
        }
    }

    private void HandleRotation()
    {
        // Calcular direção resultante do input do player e rotacionar ele na direção para onde está indo.
        var turnOrientation = Mathf.Atan2(_currentMovementInput.x, _currentMovementInput.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        var smoothedTurnOrientation = Mathf.SmoothDampAngle(transform.eulerAngles.y, turnOrientation, ref _turnSmoothSpeed, _turnTime);

        if (!_isMovementPressed) return;
        // Aplicar movimentação multiplicando pela velocidade do player:
        var _targetMovement = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;

        _currentMovement = new Vector3(_targetMovement.x, _currentMovement.y, _targetMovement.z);

        // Rotacionar a direção do player
        transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);
    }

    private void Update()
    {
        HandleRotation();
        HandleDodge();
        HandleMove();
        HandleGravity();
        HandleJump();
    }
    
    #endregion

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