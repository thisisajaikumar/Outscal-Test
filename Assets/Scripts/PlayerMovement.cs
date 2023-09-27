using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Setting")]
    [SerializeField, Range(0.1f, 10f)] float moveSpeed = 5.0f;
    [SerializeField, Range(0.1f, 10f)] float moveableJumpForce = 8.0f, idleJumpForce = 5.0f;

    [SerializeField] bool isGrounded;
    [SerializeField] LayerMask groundLayer;
    [SerializeField, Range(0.1f, 10f)] float detectionDistance = 0.75f;
    public bool isHitEnemy { get; private set; }
    [SerializeField] LayerMask enemyLayer;

    private enum PlayerState { Idle, Walk, Run, Jump, Fall }
    private PlayerState currentState = PlayerState.Idle;
    private Animator animator;


    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, detectionDistance, groundLayer);
        isHitEnemy = Physics2D.Raycast(transform.position, Vector2.down, detectionDistance, enemyLayer);

        float horizontalInput = Input.GetAxis("Horizontal");
        float jumpForce = isGrounded ? (horizontalInput == 0 ? idleJumpForce : moveableJumpForce) : 0f;

        HandleMovement(horizontalInput, jumpForce);
        UpdatePlayerState(horizontalInput, jumpForce);
    }

    private void HandleMovement(float horizontalInput, float jumpForce)
    {
        sr.flipX = horizontalInput < 0;

        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void UpdatePlayerState(float horizontalInput, float jumpForce)
    {
        PlayerState newState;

        if (isGrounded)
        {
            newState = (horizontalInput == 0) ? PlayerState.Idle : (Input.GetKey(KeyCode.LeftShift) ? PlayerState.Run : PlayerState.Walk);
        }
        else
        {
            newState = (rb.velocity.y > 0) ? PlayerState.Jump : PlayerState.Fall;
        }

        PlayAnimation(newState);
    }


    public void SetLocation(Vector2 pos)
    {
        this.transform.position = pos;
    }

    private void PlayAnimation(PlayerState newState)
    {
        if (newState == currentState) { return; }

        switch (newState)
        {
            case PlayerState.Idle:
                animator.Play("Idle");
                break;
            case PlayerState.Walk:
                animator.Play("Walk");
                break;
            case PlayerState.Run:
                animator.Play("Run");
                break;
            case PlayerState.Jump:
                animator.Play("Jump");
                break;
            case PlayerState.Fall:
                animator.Play("Fall");
                break;
        }

        currentState = newState;
    }
}