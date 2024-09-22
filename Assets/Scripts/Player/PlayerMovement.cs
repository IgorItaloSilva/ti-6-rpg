using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDataPersistence
{
    // Singleton publico do PlayerMovement
    public static PlayerMovement playerMovement;

    [Header("Referências: ")] [SerializeField]
    private Rigidbody rb;

    [SerializeField] private CharacterController cc;
    [SerializeField] private CapsuleCollider col;
    [SerializeField] private PhysicMaterial physicMat;

    private CinemachineFreeLook cinemachine;

    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dodgeAction;
    private InputAction sprintAction;

    private Camera mainCam;
    private Animator animator;

    [Header("Parametros: ")] [SerializeField]
    private float jumpForce = 7500f;

    [SerializeField] private LayerMask groundLayers;

    private float moveSpeed, turnTime;

    private const float
        dodgeDuration = 0.2f, // Duration of the dodge speed
        dodgeMoveSpeedModifier = 5f, // Multiplier for player speed when dodging
        walkMoveSpeed = 12f,
        sprintMoveModifier = 1.75f, // Multiplier for player speed when running
        walkTurnTime = 0.05f, // Player default turn speed when walking
        jumpTurnModifier = 10f; // Player turn speed when in air

    private float turnSmoothSpeed;
    private Vector2 moveInput;
    private bool isGrounded, hasJumped, isFalling, isSprinting, isDodging;

    private enum movementSystems
    {
        cc,
        rb
    }

    private void Awake()
    {
        CreateSingleton();

        mainCam = Camera.main;
        cinemachine = mainCam.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();

        HandleActions();

        moveSpeed = walkMoveSpeed;
        turnTime = walkTurnTime;

        Physics.gravity *= 2;
    }

    private void CreateSingleton()
    {
        if (playerMovement)
            Destroy(this); // Deletar novo objeto caso playerMovement já tenha sido instanciado
        else
            playerMovement = this; // Instanciar PlayerMovement caso não exista
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
        /*  ISSO TINHA SIDO REMOVIDO PELO FELIPE ANTES DO MERGE DOS 3 CODIGOS, E VOLTOU PRA CA VINDO DO SAVESYSTEM
            // Retornar drag para o padrão quando o player cair no chão
            if (isGrounded)
            {
                rb.drag = groundDrag;
                //Debug.Log("Grounded");
        } */
        
        // Checar magnitude do input para aplicar movimentação
        if ((moveInput.magnitude > 0.1))
            Move();
        
        // Pular quando o player apertar o botão e estiver no chão
        if (jumpAction.triggered && isGrounded && moveInput is not {x: 0f, y: 0f})
            Jump();
        if(Keyboard.current.lKey.wasPressedThisFrame)
            GameEventsManager.instance.playerEvents.PlayerDied();
        if(Keyboard.current.kKey.wasPressedThisFrame){
            DataPersistenceManager.instance.SaveGame();
            GameEventsManager.instance.uiEvents.SavedGame();
        }

        // Raycast para baixo para checar se o player está no chão
        isGrounded = Physics.Raycast(transform.position, Vector3.down, cc.height / 2 + cc.stepOffset + 0.05f,
            groundLayers);

        switch (isGrounded)
        {
            // Pular quando o player apertar o botão e estiver no chão
            case true:
            {
                if (jumpAction.triggered && moveInput is not { x: 0f, y: 0f } && !isDodging)
                    StartCoroutine(nameof(Jump));
                if (dodgeAction.triggered)
                    StartCoroutine(nameof(Dodge));
                break;
            }
            case false when !hasJumped && !isFalling:
                isFalling = true;
                SwitchMovements(movementSystems.rb);
                StartCoroutine(nameof(Land));
                break;
        }

#if UNITY_EDITOR
        if (Keyboard.current.cKey.wasPressedThisFrame)
            GameManager.gm.ToggleCursor();
#endif
    }

    private void FixedUpdate()
    {
        cinemachine.m_RecenterToTargetHeading.m_enabled = moveInput is not { x: 0f, y: < 0f };
    }

    private void Move()
    {
        // Calcular direção resultante do input do player e rotacionar ele na direção para onde está indo.
        var turnOrientation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + mainCam.transform.eulerAngles.y;
        var smoothedTurnOrientation =
            Mathf.SmoothDampAngle(transform.eulerAngles.y, turnOrientation, ref turnSmoothSpeed, turnTime);

        // Aplicar movimentação multiplicando pela velocidade do player:
        var moveDir = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;

        // Rotacionar a direção do player
        transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);

        if (sprintAction.triggered && !isSprinting)
            StartCoroutine(nameof(Sprint));

        // Mover com character controller quando estiver no chão e com o rigidbody quando estiver no ar
        if (isGrounded && cc.enabled)
        {
            cc.Move(moveDir * (moveSpeed * Time.deltaTime));
        }
        else
        {
            rb.AddForce(moveDir * (moveSpeed * 1.5f));
        }
    }

    private IEnumerator Jump()
    {
        hasJumped = true;
        SwitchMovements(movementSystems.rb);
        Debug.Log("Jump");
        rb.AddForce(0f, jumpForce, 0f);
        StartCoroutine(nameof(Land));
        yield return null;
    }

    private IEnumerator Land()
    {
        yield return new WaitForSeconds(0.05f);
        yield return new WaitUntil(() => isGrounded);
        hasJumped = false;
        isFalling = false;
        yield return new WaitUntil(() => moveInput is not { x: 0f, y: 0f });
        SwitchMovements(movementSystems.cc);
    }

    private IEnumerator Sprint()
    {
        isSprinting = true;
        moveSpeed = walkMoveSpeed * sprintMoveModifier;
        yield return new WaitUntil(() => sprintAction.WasReleasedThisFrame());
        isSprinting = false;
        moveSpeed = walkMoveSpeed;
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        moveSpeed = walkMoveSpeed * dodgeMoveSpeedModifier;
        Debug.Log("Dodge");
        yield return new WaitForSeconds(dodgeDuration);
        moveSpeed = walkMoveSpeed;
        isDodging = false;
    }

    private void SwitchMovements(movementSystems ms)
    {
        switch (ms)
        {
            case movementSystems.cc when !cc.enabled:
                turnTime = walkTurnTime;
                cc.enabled = true;
                rb.isKinematic = true;
                col.enabled = false;
                break;
            case movementSystems.rb when rb.isKinematic:
                turnTime = walkTurnTime * jumpTurnModifier;
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