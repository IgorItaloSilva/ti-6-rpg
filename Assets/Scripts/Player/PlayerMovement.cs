using System;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Singleton publico do PlayerMovement
    public static PlayerMovement playerMovement;

    [Header("Referências: ")] [SerializeField]
    private Rigidbody rb;

    private CinemachineFreeLook cinemachine;

    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private Camera mainCam;
    private Animator animator;

    [Header("Parametros: ")] [SerializeField] private float moveSpeed = 10f, sprintMoveSpeed = 15f;

    [SerializeField] private LayerMask groundLayers;

    private const float turnSmoothTime = 0.02f,
        jumpForce = 1500f,
        jumpDrag = 0f,
        groundDrag = 0f,
        jumpSpeedModifier = 30f;

    private float turnSmoothSpeed;
    private Vector2 moveInput;
    private bool isGrounded = true, combatMode = false;

    private void Awake()
    {
        CreateSingleton();

        mainCam = Camera.main;
        cinemachine = mainCam.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();

        HandleInput();
    }

    private void CreateSingleton()
    {
        if (playerMovement)
        {
            Destroy(this); // Deletar novo objeto caso playerMovement já tenha sido instanciado
        }
        else
        {
            playerMovement = this; // Instanciar PlayerMovement caso não exista
        }
    }

    private void HandleInput()
    {
        // Definir input do player:
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        sprintAction = playerInput.actions.FindAction("Sprint");
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
        }
        else
        {
            rb.AddForce(0f, -(jumpForce / 100), 0f);
        }

        // Checar magnitude do input para aplicar movimentação
        if ((moveInput.magnitude > 0.1 && isGrounded))
            Move();

        // Pular quando o player apertar o botão e estiver no chão
        if (jumpAction.triggered && isGrounded && moveInput is not { x: 0f, y: 0f })
            Jump();

#if UNITY_EDITOR
        if (Keyboard.current.cKey.wasPressedThisFrame)
            GameManager.gm.ToggleCursor();
#endif
    }

    private void ToggleCombatMode()
    {
        combatMode = !combatMode;
    }

    private void SetCombatMode(bool value)
    {
        combatMode = value;
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

        // Alternar entre velocidade de corrida e de caminhada
        var moveVelocity = moveDir * (sprintAction.inProgress ? sprintMoveSpeed : moveSpeed);

        // Aplicar diretamente ao velocity evita que o cinemachine tenha uma convulsão.
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z); 
    }

    private void Jump()
    {
        Debug.Log("Jump");
        rb.AddForce(0f, jumpForce, 0f);
        rb.drag = jumpDrag; // Diminuir o drag enquanto o player pula para acelerar a queda.
        isGrounded = false;
    }
}