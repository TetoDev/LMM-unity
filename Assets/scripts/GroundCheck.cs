using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private BoxCollider2D bc; // Reference to the BoxCollider2D component

    private bool grounded;
    private float lastGroundedTime = 0f; // Time when the player was last grounded
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc = GetComponent<BoxCollider2D>(); // Get the BoxCollider2D component attached to the enemy
        if (bc == null) {
            Debug.LogError("No BoxCollider2D found on the enemy GameObject!"); // Log an error if no collider is found
        }
        grounded = false; // Initialize grounded to false
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitGround = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.05f, LayerMask.GetMask("Ground"));
        

        grounded = hitGround.collider != null; // Update grounded status based on ground detection
        if (grounded) lastGroundedTime = Time.time;
    }

    public bool IsGrounded()
    {
        return grounded; // Return the grounded status
    }

    public float GetLastGroundedTime()
    {
        return lastGroundedTime; // Return the time when the player was last grounded
    }
}
