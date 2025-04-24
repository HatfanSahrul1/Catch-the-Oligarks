using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public static Movement Instance;
    [Header("Input")]
    [SerializeField] private InputAction move;
    [SerializeField] private InputAction jump;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;

    public LayerMask groundLayer;


    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    void OnEnable()
    {
        move.Enable();
        jump.Enable();
    }

    void Awake(){
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck transform belum diatur!");
        }
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
    void FixedUpdate()
    {
        if(GameStateManager.Instance.GetCurrentState() == GameStateEnum.Play){
            HandleMovement();
            HandleJump();
        }
    }

    private void HandleMovement()
    {
        float moveInput = move.ReadValue<float>();
        if (moveInput > 0)
        {
            moveInput = 1;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput < 0)
        {
            moveInput = -1;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        rb.velocity = new Vector2(moveInput * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (jump.IsPressed() && isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public bool IsGround(){
        return isGrounded;
    }
}
