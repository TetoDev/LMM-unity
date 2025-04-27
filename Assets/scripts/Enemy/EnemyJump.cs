using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    [SerializeField] EnemyStalk stalk; // Reference to the EnemyStalk script
    [SerializeField] GroundCheck groundCheck; // Reference to the GroundCheck script
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private BoxCollider2D bc; // Reference to the BoxCollider2D component
    private Animator anim;
    [SerializeField] float jumpForce = 5f; // Jump force of the enemy

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        bc = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component
        anim = GetComponent<Animator>(); // Get the Animator component
        if (rb == null) {
            Debug.LogError("No Rigidbody2D found on the enemy GameObject!"); // Log an error if no Rigidbody2D is found
        }
        if (bc == null) {
            Debug.LogError("No BoxCollider2D found on the enemy GameObject!"); // Log an error if no BoxCollider2D is found
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stalk.IsFollowing()) return; // Check if the enemy is following the player
        

        if (groundCheck.IsGrounded()) {
            if (CheckForWalls()){
            Jump(jumpForce); // Call the Jump method if the enemy is facing a wall
            }
        }
        

    }

    public void Jump(float jumpForce)
    {
        anim.SetTrigger("jump"); // Trigger the jump animation
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Reset the vertical velocity
    }

    public bool CheckForWalls () {
        
        // Check if the enemy is facing a wall
        RaycastHit2D wallInFront = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.right, 1f, LayerMask.GetMask("Ground"));
        return wallInFront.collider != null;
        
    }
}
