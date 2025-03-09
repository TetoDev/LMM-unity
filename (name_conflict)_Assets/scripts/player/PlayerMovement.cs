using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private bool direction;
    private bool move;
    private bool ascending;
    private bool jump;
    private Vector3 originalScale;
    [SerializeField] private float speed = 5.0f;
    private float jumpForce = 5.0f;
    private bool isGrounded;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private int lastAttack;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        originalScale = transform.localScale;
        isGrounded = false;
        move = false;
        ascending = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = IsGrounded();

        HandleInput();
        PlayerMove();
        HandleAnimation();

        UpdateDirection();
    }

    private void PlayerMove () {
        if (move)
        {
            rb.linearVelocity = new Vector2((direction ? 1 : -1) * speed, rb.linearVelocity.y);
        }

        if (jump)
        {
            Jump();
            jump = false;
        }

        ascending = rb.linearVelocity.y > 0.01f;

    }

    private void HandleInput () {
        // Handle Movement
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Z)) {
            move = true;
            direction = false;
        }
        else if (Input.GetKey(KeyCode.D)) {
            move = true;
            direction = true;
        } else {
            move = false;
        }
        jump = Input.GetKey(KeyCode.Space) && isGrounded;


        // Handle Attacks
        if (Input.GetKey(KeyCode.F)) {
            Attack();
        }
    }

    private void Attack () {
        if (Time.time - lastAttack > 0.1f) {
            lastAttack = (int) Time.time;
        }
    }

    private void HandleAnimation () {
        anim.SetBool("IsRunning", move);
        anim.SetBool("IsJumping", ascending && !isGrounded);
        anim.SetBool("IsFalling", !ascending && !isGrounded);
        anim.SetBool("IsGrounded", isGrounded);

    }
    private void UpdateDirection () {
        transform.localScale = new Vector3(direction ? originalScale.x : -originalScale.x, originalScale.y, originalScale.z);
    }


    private void Jump () {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private bool IsGrounded () {
        RaycastHit2D hit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

}
