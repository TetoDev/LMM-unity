using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    [SerializeField] private EnemyFindPlayer findPlayer; // Reference to the EnemyFindPlayer script
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashTime = 0.5f;
    private float lastDashTime = 0f; // Time when the last dash occurred
    [SerializeField] private float dashCooldown = 30f; // Cooldown time for dashing
    [SerializeField] private float minimumDistance = 10f; // Minimum distance to the player to initiate a dash
    private bool isDashing = false; // Flag to check if the enemy is currently dashing
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Animator anim; // Reference to the Animator component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        anim = GetComponent<Animator>(); // Get the Animator component
        if (rb == null) {
            Debug.LogError("No Rigidbody2D found on the enemy GameObject!"); // Log an error if no Rigidbody2D is found
        }
        if (anim == null) {
            Debug.LogError("No Animator found on the enemy GameObject!"); // Log an error if no Animator is found
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDashing && Time.time - lastDashTime > dashCooldown) { // Check if the enemy is not currently dashing and the cooldown period has elapsed
            if (findPlayer.playerScript != null) {
                if (findPlayer.distance < minimumDistance) { // Check if the player is within dash range and not already dashing
                    Dash(dashForce); // Call the Dash method with the specified dash force
                }
            } 
        } else {
            // If the enemy is dashing, wait for the dash time to end
            if (Time.time - lastDashTime > dashTime) {
                isDashing = false; // Reset the dashing flag after the dash time has elapsed
                anim.SetBool("dash", false); // Reset the dash animation
            }
        }
        
    }
    

    public void Dash(float dashForce)
    {
        anim.SetBool("dash", true); // Trigger the dash animation
        isDashing = true; // Set the dashing flag to true
        lastDashTime = Time.time; // Record the time of the dash
        rb.linearVelocity = new Vector2( findPlayer.playerRelativePos.normalized.x * dashForce, 0); // Set the velocity of the enemy to dash towards the player
    }
}
