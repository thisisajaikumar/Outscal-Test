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

        // Debug.DrawRay(transform.position, Vector2.down * detectionDistance, Color.red);

        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0)
        {
            sr.flipX = false;
        }
        else if (horizontalInput < 0)
        {
            sr.flipX = true;
        }

        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

        if (isGrounded || isHitEnemy)
        {
            float jumpForce;

            if (horizontalInput == 0)
            {
                PlayAnimation(PlayerState.Idle);
                jumpForce = idleJumpForce;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    PlayAnimation(PlayerState.Run);
                }
                else
                {
                    PlayAnimation(PlayerState.Walk);
                }

                jumpForce = moveableJumpForce;
            }

            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                PlayAnimation(PlayerState.Jump);
            }
            else
            {
                PlayAnimation(PlayerState.Fall);
            }
        }
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