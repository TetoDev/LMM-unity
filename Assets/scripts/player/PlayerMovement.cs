using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool direction;
    private bool move;
    private bool jump;
    private bool attack;
    private bool ascending;
    private bool dead;
    private bool hit;
    private float lastHitTime = 0f; // Time of the last hit taken
    [SerializeField] private float hitCooldown = 2f; // Cooldown time before the player can be hit again
    private bool dash;
    private float dashStartTime; // Time when the dash started
    private float dashTime = 0.2f; // Duration of the dash
    private float dashCooldown = 1.0f; // Cooldown time before the player can dash again
    [SerializeField] private float speed = 7.0f;
    [SerializeField] private float jumpForce = 20.0f;
    [SerializeField] private float frictionAmount = 0.5f; // Amount of friction applied when not moving 
    [SerializeField]  private float health = 100.0f; // Player's health
    [SerializeField] private float maxHealth = 100.0f; // Player's maximum health
    [SerializeField] private GroundCheck groundCheck; // Reference to the GroundCheck script
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerChangeDirection playerDirection; // Reference to the PlayerChangeDirection script
    [SerializeField] public PlayerVFX handleVFX; // Reference to the PlayerVFX script
    [SerializeField] public PlayerSFX handleSFX; // Reference to the PlayerSFX script
    private bool isGrounded; // Check if the player is grounded
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private Camera cam;
    private Vector2 mousePos;

    void Start()
    {
        direction = true;
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        anim = GetComponent<Animator>(); // Get the Animator component
        sr = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        bc = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component

        isGrounded = false;
        move = false;

        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        isGrounded = groundCheck.IsGrounded(); // Check if the player is grounded

        HandleInput();
        PlayerUpdate();

        ascending = rb.linearVelocity.y > 0.1f;

        HandleAnimation();

        if(move) playerDirection.ChangeDirection(direction);
        
        handleVFX.HandleVFX(jump); // Call the HandleVFX method from PlayerVFX script
        handleSFX.handleSFX(jump, dash); // Call the handleSFX method from PlayerSFX script

        jump = false;
        dash = false; 
    }

    private void PlayerUpdate()
    {   
        if (dead) return; // Ignore if dead

        if (hit && Time.time - lastHitTime > hitCooldown)
        {
            hit = false; // Reset hit state after cooldown
        }

        // Dashing logic
        if (dash) // Check if dash is available
        {

            Vector2 dashDirection = new Vector2( mousePos.x - rb.position.x, mousePos.y - rb.position.y ); // Dash in the initial direction
            rb.AddForce(dashDirection.normalized * speed * 100f, ForceMode2D.Impulse); // Apply dash force
            return;
        } 

        if (move)
        {
            float targetSpeed = speed * (direction ? 1 : -1); // Set target speed based on direction
            float speedDifference = targetSpeed - rb.linearVelocity.x; // Calculate speed difference
            float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? speed : speed*2f; // Set acceleration rate based on target speed
            float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelerationRate, 1) * Mathf.Sign(speedDifference); // Calculate movement

            rb.AddForce(movement * Vector2.right, ForceMode2D.Impulse); // Apply force to the player
        }

        //Friction logic
        if (groundCheck.GetLastGroundedTime() > 0f && !move) {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocityX), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.linearVelocity.x); // Get the sign of the velocity
            rb.AddForce(-amount * Vector2.right, ForceMode2D.Impulse); // Apply friction force to the player
        }

        

        if (jump)
        {
            Jump();
        }
    }

    private void HandleInput()
    {   
        Vector3 mousePos3d = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector2(mousePos3d.x, mousePos3d.y);


        if (dead || dash) return; // Ignore if dead or dashing
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            move = true;
            direction = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move = true;
            direction = true;
        }
        else
        {
            move = false;
        }

        if (Input.GetKey(KeyCode.LeftShift)){
            if (Time.time - dashStartTime > dashCooldown) // Check if dash is available
            {
                dashStartTime = Time.time; // Reset dash start time
                dash = true; // Set dash state
            }
        }

        jump = Input.GetKey(KeyCode.Space) && isGrounded;
        attack = Input.GetMouseButton(0); // Click to attack
    }

    private void HandleAnimation()
    {
        if (jump) {
            anim.SetTrigger("Jump");
        }
        anim.SetBool("Running", move);
        anim.SetBool("Ascending", ascending);
        
        anim.SetBool("Grounded", isGrounded);
        if (attack) {
            playerAttack.Attack(mousePos); // Call the attack method from PlayerAttack script
        }
        
    }

    private void Jump()
    {
        rb.linearVelocityY = jumpForce; // Apply jump force
    }



    public void TakeDamage(float damage)
    {   
        if (dead || hit) return; // Ignore if already dead or hit
        
        health -= damage;

        if (health <= 0)
        {
            Die();
        } else {
            // Play hurt animation
            hit = true;
            lastHitTime = Time.time; // Update last hit time
            anim.SetTrigger("Hit");
        }
    }

    private void Die()
    {
        anim.SetTrigger("Dead");
        dead = true;
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public float GetHealth()
    {
        return health;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public Vector2 GetVelocity()
    {
        return rb.linearVelocity;
    }
    public bool IsDashing()
    {
        return dash;
    }
}
