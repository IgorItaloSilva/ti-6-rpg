using System;
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

    private CinemachineFreeLook cinemachine;

    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction cursorToggleAction;
    private Camera mainCam;

    [Header("Parametros: ")] 
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private LayerMask groundLayers;
    private const float turnSmoothTime = 0.015f, jumpForce = 500f, jumpDrag = 0f, groundDrag = 0f, jumpSpeedModifier = 30f;
    private float turnSmoothSpeed;
    private Vector2 moveInput;
    private bool isGrounded = true;

    private void Awake()
    {
        if (playerMovement)
        {
            Destroy(this); // Deletar novo objeto caso playerMovement já tenha sido instanciado
        }
        else
        {
            playerMovement = this; // Instanciar PlayerMovement caso não exista
        }

        mainCam = Camera.main;
        cinemachine = mainCam.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();

        HandleInput();
    }

    private void HandleInput()
    {
        // Definir input do player:
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        cursorToggleAction = playerInput.actions.FindAction("CursorToggle");
    }

    private void Update()
    {
        // Ler valores de movimento do InputActionReference
        moveInput = moveAction.ReadValue<Vector2>().normalized;
        
        // Raycast para baixo para checar se o player está no chão
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.01f, groundLayers);

        // Retornar drag para o padrão quando o player cair no chão
        if (isGrounded)
        {
            rb.drag = groundDrag;
            //Debug.Log("Grounded");
        }
        
        // Checar magnitude do input para aplicar movimentação
        if ((moveInput.magnitude > 0.1 && isGrounded))
            Move();
        
        // Pular quando o player apertar o botão e estiver no chão
        if (jumpAction.triggered && isGrounded && moveInput is not {x: 0f, y: 0f})
            Jump();
        if(Keyboard.current.lKey.wasPressedThisFrame)
            GameEventsManager.instance.playerEvents.PlayerDied();
        if(Keyboard.current.kKey.wasPressedThisFrame)
            DataPersistenceManager.instance.SaveGame();

        
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
            Mathf.SmoothDampAngle(transform.eulerAngles.y, turnOrientation, ref turnSmoothSpeed, turnSmoothTime);
        rb.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);

        // Aplicar movimentação multiplicando pela velocidade do player:
        var moveDir = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;
        var moveVelocity = moveDir * moveSpeed;

        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z); // Aplicar diretamente ao velocity evita que o cinemachine tenha uma convulsão.
    }

    private void Jump()
    {
        Debug.Log("Jump");
        rb.AddForce(0f, jumpForce, 0f);
        rb.drag = jumpDrag; // Diminuir o drag enquanto o player pula para acelerar a queda.
        isGrounded = false;
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