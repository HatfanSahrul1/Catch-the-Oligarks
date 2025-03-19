using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    public float moveSpeed = 5f; 
    public float jumpForce = 10f; 
    
    private Rigidbody2D rb;
    private bool isGrounded; 

    public LayerMask groundLayer;

    
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

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

        float moveInput = Input.GetAxisRaw("Horizontal");
        
        // if(Input.GetKey(KeyCode.D)) moveInput = 1.0f;
        // else if(Input.GetKey(KeyCode.A)) moveInput = -1.0f;
        
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

}
