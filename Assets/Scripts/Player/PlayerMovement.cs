using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Singleton publico do PlayerMovement
    public static PlayerMovement playerMovement;

    [Header("Referências: ")] [SerializeField]
    private Rigidbody rb;

    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction cursorToggleAction;
    private Camera mainCam;

    [Header("Parametros: ")] [SerializeField]
    private float moveSpeed = 10f;

    private const float turnSmoothTime = 0.015f, jumpForce = 500f, jumpDragModifier = 4f, jumpSpeedModifier = 30f;
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

        // Checar magnitude do input para aplicar movimentação
        if ((moveInput.magnitude > 0.1))
            Move();

        // Pular quando o player apertar o botão e estiver no chão
        if (jumpAction.triggered && isGrounded)
            Jump();

        if (cursorToggleAction.triggered)
            GameManager.gm.ToggleCursor();
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
        rb.drag /= jumpDragModifier; // Diminuir o drag enquanto o player pula para acelerar a queda.
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        // Quando o player cair no chão:
        if (!isGrounded && other.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            rb.drag *= jumpDragModifier; // Voltar o drag ao normal para o player não deslizar demais.
        }
    }
}