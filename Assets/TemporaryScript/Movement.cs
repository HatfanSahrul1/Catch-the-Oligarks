using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
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
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float moveInput = move.ReadValue<float>();
        if (moveInput > 0)
            moveInput = 1;
        else if (moveInput < 0)
            moveInput = -1;
        else
            moveInput = 0;

        rb.velocity = new Vector2(moveInput * moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
    }

    private void HandleJump()
    {
        if (jump.IsPressed() && isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
