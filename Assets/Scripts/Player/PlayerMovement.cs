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
    private float _turnSmoothSpeed, _gravity, _initialJumpVelocity;
    private const float MaxJumpHeight = 0.6f, MaxJumpTime = .65f, MoveSpeed = 10f, SprintSpeedModifier = 2f, GroundedGravity = -0.05f, TurnTime = 0.1f;
    private Vector3 _currentMovement;
    private Vector2 _currentMovementInput;
    private bool _hasJumped, _isMovementPressed, _isSprintPressed, _isJumpPressed, _isJumping;

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
                _isJumping = true;
                _currentMovement.y += _initialJumpVelocity * 0.5f;
                break;
            case false when _isJumping && cc.isGrounded:
                _isJumping = false;
                break;
        }
    }

    private void FixedUpdate()
    {
        cinemachine.m_RecenterToTargetHeading.m_enabled = _currentMovementInput is { x: not 0, y: > 0f };
    }

    private void HandleMove()
    {
        if (_isSprintPressed && _isMovementPressed)
        {
            _currentMovement.x = transform.forward.x * SprintSpeedModifier;
            _currentMovement.z = transform.forward.z * SprintSpeedModifier;
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
        var smoothedTurnOrientation = Mathf.SmoothDampAngle(transform.eulerAngles.y, turnOrientation, ref _turnSmoothSpeed, TurnTime);

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