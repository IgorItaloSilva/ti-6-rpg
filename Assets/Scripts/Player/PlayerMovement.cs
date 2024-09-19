using System;
using System.Collections;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Singleton publico do PlayerMovement
    public static PlayerMovement playerMovement;

    [Header("Referências: ")] 
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CharacterController cc;
    [SerializeField] private CapsuleCollider col;

    private CinemachineFreeLook cinemachine;

    [SerializeField] private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dodgeAction;
    private InputAction sprintAction;

    private Camera mainCam;
    private Animator animator;

    [Header("Parametros: ")] [SerializeField]
    private float moveSpeed = walkMoveSpeed, 
        jumpForce = 7500f;

    [SerializeField] private LayerMask groundLayers;

    private const float turnSmoothTime = 0.02f,
        walkMoveSpeed = 10f,
        sprintMoveSpeed = 15f,
        dodgeMoveSpeed = 50f,
        dodgeDuration = 0.2f,
        airDrag = 0f,
        groundDrag = 0f;

    private float turnSmoothSpeed;
    private Vector2 moveInput;
    private bool isGrounded = true, combatMode = false;

    private void Awake()
    {
        CreateSingleton();

        mainCam = Camera.main;
        cinemachine = mainCam.transform.parent.gameObject.GetComponent<CinemachineFreeLook>();

        ActionsWrapper();

        Physics.gravity *= 2;
    }

    private void CreateSingleton()
    {
        if (playerMovement)
            Destroy(this); // Deletar novo objeto caso playerMovement já tenha sido instanciado
        else
            playerMovement = this; // Instanciar PlayerMovement caso não exista
    }

    private void ActionsWrapper()
    {
        // Definir input do player:
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        jumpAction = playerInput.actions.FindAction("Dodge");
        sprintAction = playerInput.actions.FindAction("Sprint");
    }

    private void Update()
    {
        // Ler valores de movimento do InputActionReference
        moveInput = moveAction.ReadValue<Vector2>().normalized;

        // Checar magnitude do input para aplicar movimentação
        if ((moveInput.magnitude > 0.1))
            Move();

        // Raycast para baixo para checar se o player está no chão
        isGrounded = Physics.Raycast(transform.position, Vector3.down, cc.height / 2 + cc.stepOffset + 0.1f, groundLayers);
        
        // Pular quando o player apertar o botão e estiver no chão
        if(isGrounded)
            if(jumpAction.triggered)
                Jump();
            if()

#if UNITY_EDITOR
        if (Keyboard.current.cKey.wasPressedThisFrame)
            GameManager.gm.ToggleCursor();
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            combatMode = !combatMode;
            Debug.Log("Combat Mode: " + combatMode);
        }
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

        // Aplicar movimentação multiplicando pela velocidade do player:
        var moveDir = Quaternion.Euler(0f, turnOrientation, 0f) * Vector3.forward;

        // Mover com character controller quando estiver no chão e com o rigidbody quando estiver no ar
        if (isGrounded)
        {
            transform.rotation = Quaternion.Euler(0f, smoothedTurnOrientation, 0f);
            cc.Move(moveDir * (moveSpeed * Time.deltaTime));
        }
        else
        {
            rb.AddForce(moveDir * (moveSpeed * 2));
        }
    }

    private void Jump()
    {
        Debug.Log("Jump");
        rb.AddForce(0f, jumpForce, 0f);
    }

    private IEnumerator InAir()
    {
        isGrounded = false;
        rb.isKinematic = false;
        rb.velocity = cc.velocity;
        cc.enabled = false;
        yield return new WaitUntil(() => isGrounded);
        cc.enabled = true;
        rb.isKinematic = true;
        cc.SimpleMove(rb.velocity);
    }

    private IEnumerator Dodge()
    {
        moveSpeed = dodgeMoveSpeed;
        Debug.Log("Dodge");
        yield return new WaitForSeconds(dodgeDuration);
        moveSpeed = walkMoveSpeed;
    }
}