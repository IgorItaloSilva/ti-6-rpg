#region Imports

using System;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.VFX;

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

    public CinemachineFreeLook playerCamera;
    public CinemachineVirtualCamera targetCamera;
    private CinemachineTargetGroup _camTargetGroup;
    private CinemachineBasicMultiChannelPerlin[] _camNoises = new CinemachineBasicMultiChannelPerlin[2];
    public GameObject enemyDetectionObject;
    private EnemyDetection enemyDetector;
    private Camera _mainCam;
    private Animator _animator;
    private CharacterController _cc;
    [SerializeField] private PlayerWeapon _swordWeaponManager, _magicWeaponManager;
    private PlayerInput _playerInput;
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    [FormerlySerializedAs("_swordMainTrail")] [SerializeField] private TrailRenderer _swordTrail;
    [SerializeField] private VisualEffect _swordSlash;
    [SerializeField] private VisualEffect _parryVfx;

    #endregion

    #region Private Variables

    private float _gravity, _initialJumpVelocity, _camYSpeed, _camXSpeed, _acceleration, _maxAcceleration;

    private Vector3 _currentMovement, _appliedMovement;
    private Vector2 _currentMovementInput, _currentLookInput;

    private bool
        _playerStaggered = false,
        _hasJumped,
        _isMovementPressed,
        _isSprintPressed,
        _isJumping,
        _isBlocking,
        _canInteract = true,
        _isJumpPressed,
        _isDodgePressed,
        _isAttackPressed,
        _isSpecial1Pressed,
        _isSpecial2Pressed,
        _isSpecial3Pressed,
        _isSpecial4Pressed,
        _isMagicPressed,
        _isBlockPressed,
        _isPotionPressed,
        _isInteractPressed,
        _isDodging,
        _canDodge = true,
        _canJump = true,
        _canAttack = true,
        _canBlock = true,
        _canHeal = true,
        _canEnterCombat = true,
        _canMount = true,
        _canCastMagic = true,
        _canSpecial1 = true,
        _canSpecial2 = true,
        _canSpecial3 = true,
        _canSpecial4 = true,
        _isBetweenAttacks,
        _isClimbing,
        _isTargetPressed,
        _canTarget,
        _isOnTarget,
        _isLocked,
        _inCombat,
        _shouldParry;

    private byte _attackCount;

    #endregion

    #region Public Variables

    public readonly byte BaseMoveSpeed = 5;

    public readonly float MaxJumpHeight = 1f,
        MaxJumpTime = .75f,
        BaseGravity = -9.8f,
        BaseTurnTime = 0.2f,
        SlowTurnTimeModifier = 1.5f;

    public bool ShowDebugLogs, TestHitFeedback;

    #endregion

    #region AnimatorHashes

    public readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    public readonly int IsBlockingHash = Animator.StringToHash("isBlocking");
    public readonly int IsGroundedHash = Animator.StringToHash("isGrounded");
    public readonly int IsCastingMagicHash = Animator.StringToHash("isCastingMagic");
    public readonly int Attack1Hash = Animator.StringToHash("Attack1");
    public readonly int Attack2Hash = Animator.StringToHash("Attack2");
    public readonly int Attack3Hash = Animator.StringToHash("Attack3");
    public readonly int Attack4Hash = Animator.StringToHash("Attack4");
    public readonly int Special1Hash = Animator.StringToHash("Special1");
    public readonly int Special2Hash = Animator.StringToHash("Special2");
    public readonly int Special3Hash = Animator.StringToHash("Special3");
    public readonly int Special4Hash = Animator.StringToHash("Special4");
    public readonly int IsClimbingHash = Animator.StringToHash("isClimbing");
    public readonly int HasJumpedHash = Animator.StringToHash("hasJumped");
    public readonly int HasDodgedHash = Animator.StringToHash("hasDodged");
    public readonly int HasHealedHash = Animator.StringToHash("hasHealed");
    public readonly int TookHitHash = Animator.StringToHash("tookHit");
    public readonly int HasParried = Animator.StringToHash("hasParried");
    public readonly int HasDiedHash = Animator.StringToHash("hasDied");
    public readonly int HasPrayedHash = Animator.StringToHash("hasPrayed");
    public readonly int HasRespawnedHash = Animator.StringToHash("hasRespawned");
    public readonly int PlayerVelocityXHash = Animator.StringToHash("playerVelocityX");
    public readonly int PlayerVelocityYHash = Animator.StringToHash("playerVelocityY");
    public readonly int InCombatHash = Animator.StringToHash("inCombat");

    #endregion

    #region Coisas skill tree

    public bool IsSpecial1Unlocked { get; private set; }
    public bool IsSpecial2Unlocked { get; private set; }
    public bool IsSpecial3Unlocked { get; private set; }
    public bool IsSpecial4Unlocked { get; private set; }
    public bool IsSpecial1OnCooldown { get; private set; }
    public bool IsSpecial2OnCooldown { get; private set; }
    public bool IsSpecial3OnCooldown { get; private set; }
    public bool IsSpecial4OnCooldown { get; private set; }

    public void UnlockSpecial(int id)
    {
        switch (id) //sim essa merda é hardcoded  ¯\_(ツ)_/¯
        {
            case 1: //tier 1 (primeiro a ser comprado) skill tree do bem
                IsSpecial1Unlocked = true;
                break;
            case 5: //tier 3 (2 pré requisitos) skill tree do bem
                IsSpecial2Unlocked = true;
                break;
            case 10: //tier 2 (1 pré requisito) skill tree do mal
                IsSpecial3Unlocked = true;
                break;
            case 9: //tier 3 (2 pré requisitos) skill tree do mal (sim o 9 vem depois do 10)
                IsSpecial4Unlocked = true;
                break;
            default:
                Debug.LogWarning("Tentamos ativar um power up que a State machine não reconhece");
                break;
        }
    }

    public void StartSpecialCooldown(int special)
    {
        switch (special)
        {
            case 1:
                IsSpecial1OnCooldown = true;
                break;
            case 2:
                IsSpecial2OnCooldown = true;
                break;
            case 3:
                IsSpecial3OnCooldown = true;
                break;
            case 4:
                IsSpecial4OnCooldown = true;
                break;
            default:
                Debug.LogWarning("Tentamos ativar o cooldown de uma skill que não existe");
                break;
        }
        SpecialAttackUIManager.instance
            ?.StartCooldown(
                special); //Essa função vai cuidar do cooldown e nos avisar quando ele tiver acabado, o tempo de cooldown será definido la
    }

    public void EndSpecialCooldown(int special) //chamdo pela UI quando o cooldown acabar
    {
        switch (special)
        {
            case 1:
                IsSpecial1OnCooldown = false;
                break;
            case 2:
                IsSpecial2OnCooldown = false;
                break;
            case 3:
                IsSpecial3OnCooldown = false;
                break;
            case 4:
                IsSpecial4OnCooldown = false;
                break;
            default:
                Debug.LogWarning("Tentamos desativar o cooldown de uma skill que não existe");
                break;
        }
    }

    #endregion

    #region Public Getters

    public CharacterController CC => _cc;
    public Animator Animator => _animator;
    public Camera MainCam => _mainCam;
    public CinemachineTargetGroup CamTargetGroup => _camTargetGroup;
    public EnemyDetection EnemyDetector => enemyDetector;
    public TrailRenderer SwordTrail => _swordTrail;
    public PlayerWeapon SwordWeaponManager => _swordWeaponManager;
    public PlayerWeapon MagicWeaponManager => _magicWeaponManager;
    public Vector3 CurrentMovementInput => _currentMovementInput;
    public float Gravity => _gravity;
    public byte AttackCount => _attackCount;
    public bool IsInteractPressed => _isInteractPressed && _canInteract;
    public bool IsMovementPressed => _isMovementPressed;
    public bool IsJumpPressed => _isJumpPressed && _canJump;
    public bool IsDodgePressed => _isDodgePressed && _canDodge;
    public bool IsAttackPressed => _isAttackPressed && _canAttack;
    public bool IsPotionPressed => _isPotionPressed && _canHeal;
    public bool IsBlockPressed => _isBlockPressed;
    public bool IsSprintPressed => _isSprintPressed && _isMovementPressed;
    public bool IsSpecial1Pressed => _isSpecial1Pressed && _canSpecial1 && IsSpecial1Unlocked && !IsSpecial1OnCooldown;
    public bool IsSpecial2Pressed => _isSpecial2Pressed && _canSpecial2 && IsSpecial2Unlocked && !IsSpecial2OnCooldown;
    public bool IsSpecial3Pressed => _isSpecial3Pressed && _canSpecial3 && IsSpecial3Unlocked && !IsSpecial3OnCooldown;
    public bool IsSpecial4Pressed => _isSpecial4Pressed && _canSpecial4 && IsSpecial4Unlocked && !IsSpecial4OnCooldown;
    public bool IsMagicPressed => _isMagicPressed;
    public bool IsCastingMagic => _isMagicPressed && _canCastMagic;
    public bool IsClimbing => _isClimbing;
    public bool IsBlocking => _isBlocking && _inCombat;
    public float InitialJumpVelocity => _initialJumpVelocity;

    #endregion

    #region Public Setters

    public bool IsDodging
    {
        get => _isDodging;
        set => _isDodging = value;
    }

    public bool ShouldParry
    {
        get => _shouldParry;
        set => _shouldParry = value;
    }

    public bool IsLocked
    {
        get => _isLocked;
        set => _isLocked = value;
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

    public bool CanCastMagic
    {
        get => _canCastMagic;
        set => _canCastMagic = value;
    }

    public bool CanEnterCombat
    {
        set => _canEnterCombat = value;
    }

    public bool CanJump
    {
        set => _canJump = value;
    }

    public bool CanDodge
    {
        set => _canDodge = value;
    }

    public bool CanHeal
    {
        set => _canHeal = value;
    }

    public bool CanInteract
    {
        set => _canInteract = value;
    }

    public bool CanSpecial1
    {
        set => _canSpecial1 = value;
    }

    public bool CanSpecial2
    {
        set => _canSpecial2 = value;
    }

    public bool CanSpecial3
    {
        set => _canSpecial3 = value;
    }

    public bool CanSpecial4
    {
        set => _canSpecial4 = value;
    }

    public bool InCombat
    {
        get => _inCombat && _canEnterCombat;
        set => _inCombat = value;
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

    public float CurrentMovementZ
    {
        set => _currentMovement.z = value;
    }

    public Vector3 AppliedMovement
    {
        get => _appliedMovement;
        set => _appliedMovement = value;
    }

    public float AppliedMovementX
    {
        set => _appliedMovement.x = value;
    }

    public float AppliedMovementY
    {
        set => _appliedMovement.y = value;
    }

    public float AppliedMovementZ
    {
        set => _appliedMovement.z = value;
    }

    public float Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }

    #endregion

    #region Input Initializers

    private void OnEnable()
    {
        _playerInput.Gameplay.Enable();
        GameEventsManager.instance.playerEvents.onPlayerDied += PlayerDied;
        GameEventsManager.instance.playerEvents.onPlayerRespawned += PlayerRespawned;
        GameEventsManager.instance.uiEvents.onPauseGame += StopCameraInput;
        GameEventsManager.instance.uiEvents.onUnpauseGame += ResumeCameraInput;
    }

    private void OnDisable()
    {
        _playerInput.Gameplay.Disable();
        GameEventsManager.instance.playerEvents.onPlayerDied -= PlayerDied;
        GameEventsManager.instance.playerEvents.onPlayerRespawned -= PlayerRespawned;
        GameEventsManager.instance.uiEvents.onPauseGame -= StopCameraInput;
        GameEventsManager.instance.uiEvents.onUnpauseGame -= ResumeCameraInput;
    }

    private void SetupInputCallbackContext()
    {
        _playerInput.Gameplay.Move.started += OnMovementPressed;
        _playerInput.Gameplay.Move.canceled += OnMovementPressed;
        _playerInput.Gameplay.Move.performed += OnMovementPressed;
        _playerInput.Gameplay.Sprint.started += OnSprintPressed;
        _playerInput.Gameplay.Sprint.canceled += OnSprintPressed;
        _playerInput.Gameplay.Jump.started += OnJumpPressed;
        _playerInput.Gameplay.Jump.canceled += OnJumpPressed;
        _playerInput.Gameplay.Dodge.started += OnDodgePressed;
        _playerInput.Gameplay.Dodge.canceled += OnDodgePressed;
        _playerInput.Gameplay.Attack.started += OnAttackPressed;
        _playerInput.Gameplay.Attack.canceled += OnAttackPressed;
        _playerInput.Gameplay.Interact.started += OnInteractPressed;
        _playerInput.Gameplay.Interact.canceled += OnInteractPressed;
        _playerInput.Gameplay.Target.started += OnTargetPressed;
        _playerInput.Gameplay.Target.started += OnTargetPressed;
        _playerInput.Gameplay.Potion.started += OnPotionPressed;
        _playerInput.Gameplay.Potion.canceled += OnPotionPressed;
        _playerInput.Gameplay.Block.started += OnBlockPressed;
        _playerInput.Gameplay.Block.canceled += OnBlockPressed;
        _playerInput.Gameplay.Special1.started += OnSpecial1Pressed;
        _playerInput.Gameplay.Special1.canceled += OnSpecial1Pressed;
        _playerInput.Gameplay.Special2.started += OnSpecial2Pressed;
        _playerInput.Gameplay.Special2.canceled += OnSpecial2Pressed;
        _playerInput.Gameplay.Special3.started += OnSpecial3Pressed;
        _playerInput.Gameplay.Special3.canceled += OnSpecial3Pressed;
        _playerInput.Gameplay.Special4.started += OnSpecial4Pressed;
        _playerInput.Gameplay.Special4.canceled += OnSpecial4Pressed;
        _playerInput.Gameplay.Magic.started += OnMagicPressed;
        _playerInput.Gameplay.Magic.canceled += OnMagicPressed;
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

    private void OnAttackPressed(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
        _canAttack = true;
    }

    private void OnPotionPressed(InputAction.CallbackContext context)
    {
        _isPotionPressed = context.ReadValueAsButton();
        _canHeal = true;
    }

    private void OnSprintPressed(InputAction.CallbackContext context)
    {
        _isSprintPressed = context.ReadValueAsButton();
    }

    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _canJump = true;
    }

    private void OnBlockPressed(InputAction.CallbackContext context)
    {
        _isBlockPressed = context.ReadValueAsButton();
        _isBlocking = _isBlockPressed && _canBlock;
        _canBlock = true;
    }

    private void OnDodgePressed(InputAction.CallbackContext context)
    {
        _isDodgePressed = context.ReadValueAsButton();
    }

    private void OnMagicPressed(InputAction.CallbackContext context)
    {
        _isMagicPressed = context.ReadValueAsButton();
    }

    private void OnTargetPressed(InputAction.CallbackContext context)
    {
        _isTargetPressed = context.ReadValueAsButton();
        _canTarget = true;
    }

    private void OnSpecial1Pressed(InputAction.CallbackContext context)
    {
        _isSpecial1Pressed = context.ReadValueAsButton();
        _canSpecial1 = true;
    }

    private void OnSpecial2Pressed(InputAction.CallbackContext context)
    {
        _isSpecial2Pressed = context.ReadValueAsButton();
        _canSpecial2 = true;
    }

    private void OnSpecial3Pressed(InputAction.CallbackContext context)
    {
        _isSpecial3Pressed = context.ReadValueAsButton();
        _canSpecial3 = true;
    }

    private void OnSpecial4Pressed(InputAction.CallbackContext context)
    {
        _isSpecial4Pressed = context.ReadValueAsButton();
        _canSpecial4 = true;
    }

    private void SetupJumpVariables()
    {
        var timeToApex = MaxJumpTime / 2;
        _gravity = (-2 * MaxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * MaxJumpHeight) / timeToApex;
    }

    #endregion

    #region Initializers

    private void InitializeReferences()
    {
        _playerInput = new PlayerInput();
        _mainCam = Camera.main;
        _camTargetGroup = targetCamera?.GetComponentInChildren<CinemachineTargetGroup>();
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        enemyDetector = enemyDetectionObject.GetComponent<EnemyDetection>();
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
        _camXSpeed = playerCamera.m_XAxis.m_MaxSpeed;
        _camYSpeed = playerCamera.m_YAxis.m_MaxSpeed;
        _swordSlash.playRate = 0.6f;
        _swordSlash.Stop();
    }

    private void Start()
    {
        _camNoises[0] = targetCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _camNoises[1] = playerCamera.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateState();
        playerCamera.m_RecenterToTargetHeading.m_enabled = _currentMovementInput is { x: not 0, y: > 0f };
    }

    private void Update()
    {
        _currentState.UpdateState();

        if (_isTargetPressed && _canTarget)
            HandleTarget();
    }

    #region Collisions / Triggers

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbable") && _canMount)
        {
            if (!Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
                    transform.forward, 1.5f)) return;

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

    public void HandleAttack(bool dodgeAttack = false)
    {
        _canAttack = false;

        switch (_attackCount)
        {
            case 0:
                _attackCount = 1;
                _animator.SetBool(Attack1Hash, true);
                break;
            case 1:
                if (!_animator.GetCurrentAnimatorStateInfo(1).IsName("Attack1")) return;
                _attackCount = 2;
                _animator.SetBool(Attack2Hash, true);
                break;
            case 2:
                if (!_animator.GetCurrentAnimatorStateInfo(1).IsName("Attack2")) return;
                _attackCount = 3;
                _animator.SetBool(Attack3Hash, true);
                break;
            case 3:
                if (!_animator.GetCurrentAnimatorStateInfo(1).IsName("Attack3")) return;
                _attackCount = 4;
                _animator.SetBool(Attack4Hash, true);
                break;
            case 4:
                if (!_animator.GetCurrentAnimatorStateInfo(1).IsName("Attack4")) return;
                _attackCount = 1;
                _animator.SetBool(Attack1Hash, true);
                _animator.SetBool(Attack2Hash, false);
                _animator.SetBool(Attack3Hash, false);
                _animator.SetBool(Attack4Hash, false);
                break;
        }
    }

    public void ResetAttacks()
    {
        _animator.SetBool(Attack1Hash, false);
        _animator.SetBool(Attack2Hash, false);
        _animator.SetBool(Attack3Hash, false);
        _animator.SetBool(Attack4Hash, false);
        _attackCount = 0;

        DisableSwordCollider();

        if (ShowDebugLogs) Debug.LogWarning("RESET ATTACKS");
    }

    private void EnableSwordCollider()
    {
        if (_isDodging) return;
        _swordWeaponManager.SetDamageType(Enums.AttackType.LightAttack);
        _swordTrail.emitting = true;
        _swordWeaponManager.EnableCollider();
        AudioPlayer.instance.PlaySFX("AirSlash");
        if((enemyDetector.targetEnemy &&
            Vector3.Distance(transform.position, enemyDetector.targetEnemy.transform.position) > 2f) || !_inCombat)
            _acceleration = 2f;
        if (TestHitFeedback)
        {
            HitAnimatorPause();
            CameraShake();
            AudioPlayer.instance.PlaySFX("Stab");
        }
    }

    private void EnableSwordColliderAttack4()
    {
        if (_isDodging) return;
        _swordWeaponManager.SetDamageType(Enums.AttackType.HeavyAttack);
        _swordSlash.Play();
        _swordWeaponManager.EnableCollider();
        AudioPlayer.instance.PlaySFX("AirSlash");
        AudioPlayer.instance.PlaySFX("SwordSlash");
        if((enemyDetector.targetEnemy &&
            Vector3.Distance(transform.position, enemyDetector.targetEnemy.transform.position) > 2f) || !_inCombat)
            _acceleration = 2.5f;
    }

    public void StaggerPlayer()
    {
        _currentState.StaggerPlayer();
    }

    private void DisableSwordCollider()
    {
        _swordTrail.emitting = false;
        _swordWeaponManager.DisableCollider();
    }

    public void LockPlayer()
    {
        if (_isLocked) return;
        _isLocked = true;
        _currentState.LockPlayer();
    }

    // ReSharper disable once UnusedMember.Global
    public void UnlockPlayer()
    {
        if (ShowDebugLogs) Debug.Log("Unlocked player");
        _isLocked = false;
        _currentState.UnlockPlayer();
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
        _animator.ResetTrigger(HasRespawnedHash);
        _animator.SetTrigger(HasRespawnedHash);
        InitializeGroundedState();
    }

    #endregion

    #region Camera

    private void HandleTarget()
    {
        _canTarget = false;
        if (!_isOnTarget)
        {
            if (enemyDetector.targetEnemy)
                CameraTargetLock(enemyDetector.targetEnemy.transform);
        }
        else
        {
            CameraTargetUnlock();
        }
    }

    private void StopCameraInput()
    {
        playerCamera.m_YAxis.m_MaxSpeed = 0f;
        playerCamera.m_XAxis.m_MaxSpeed = 0f;
    }

    private void ResumeCameraInput()
    {
        playerCamera.m_YAxis.m_MaxSpeed = _camYSpeed;
        playerCamera.m_XAxis.m_MaxSpeed = _camXSpeed;
    }

    public void CameraTargetLock(Transform newTarget)
    {
        _isOnTarget = true;
        _camTargetGroup.m_Targets[0].target = newTarget;
        playerCamera.enabled = false;
    }

    public void CameraTargetUnlock(bool shouldForgetTarget = false)
    {
        if (shouldForgetTarget) enemyDetector.targetEnemy = null;
        _camTargetGroup.m_Targets[0].target = null;
        playerCamera.enabled = true;
        _isOnTarget = false;
    }

    public async void CameraShake(float strength = 1f, float duration = .3f)
    {
        await Task.Delay(25);
        foreach (var camNoise in _camNoises)
        {
            if (camNoise != null)
                camNoise.m_AmplitudeGain = strength;
        }

        var elapsed = 0f;
        while (elapsed < duration)
        {
            await Task.Delay(10);
            elapsed += 0.01f;
            foreach (var camNoise in _camNoises)
            {
                if (camNoise != null)
                    camNoise.m_AmplitudeGain = Mathf.Lerp(strength, 0f, elapsed / duration);
            }
        }

        foreach (var camNoise in _camNoises)
        {
            if (camNoise != null)
                camNoise.m_AmplitudeGain = 0f;
        }
    }


    public async void HitAnimatorPause()
    {
        await Task.Delay(25);
        if (ShowDebugLogs) Debug.Log("Paused animation");
        _animator.speed = 0f;
        await Task.Delay(125);
        _animator.speed = 1f;
    }

    #endregion

    public void AddActionToInteract(Action<InputAction.CallbackContext> action)
    {
        _playerInput.Gameplay.Interact.started += action;
    }

    public void RemoveActionFromInteract(Action<InputAction.CallbackContext> action)
    {
        _playerInput.Gameplay.Interact.started -= action;
    }

    public void PlayParryVFX()
    {
        _parryVfx.Play();
    }
}