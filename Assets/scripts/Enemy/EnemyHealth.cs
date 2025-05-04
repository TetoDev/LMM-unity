using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health = 100.0f; // Enemy's health
    [SerializeField] private float maxHealth = 100.0f; // Enemy's maximum health
    [SerializeField] private float stunTime = 0.5f; // Time the enemy is stunned after being hit
    [SerializeField] private float destroyTime = 2.0f; // Time before the enemy is destroyed after death
    [SerializeField] private int maxHitsUntilUnstun = 100; // Maximum hits before the enemy unstuns
    private Animator anim;
    private Rigidbody2D rb; // Reference to the enemy's Rigidbody2D component
    private Collider2D bodyCollider; // Reference to the enemy's collider
    private float lastHitTime = 0f; // Time of the last hit taken
    private bool dead;
    private int hitCount = 0; // Count of hits taken
    private float lastHitReductionTime = 0f; // Time of the last hit reduction
    private bool stunnable; // Flag to check if the enemy can be stunned

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize health if needed
        health = maxHealth;
        anim = GetComponent<Animator>(); // Get the Animator component attached to the enemy
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the enemy
        bodyCollider = GetComponent<Collider2D>(); // Get the Collider2D component attached to the enemy
        if (bodyCollider == null) {
            Debug.LogError("No Collider2D found on the enemy GameObject!"); // Log an error if no collider is found
        }

        dead = false; // Initialize dead state to false
        stunnable = true; // Initialize stunnable state to true
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastHitTime < stunTime) {
            anim.SetBool("hit", false); // Reset the hit animation if within cooldown period
        }

        if (Time.time - lastHitReductionTime > 1f) {
            hitCount = hitCount > 0 ? hitCount -1 : 0 ; // Reset the hit count after 1 second
            lastHitReductionTime = Time.time; // Update the last hit reduction time
        }

        stunnable = hitCount < maxHitsUntilUnstun; // Set stunable to true if no hits have been taken   
        if (!stunnable) {
            anim.SetBool("hit",false);
        }
    }

    private void Die () {
        anim.SetTrigger("dead"); // Trigger the death animation
        dead = true; // Set the dead state to true
        rb.gravityScale = 0; // Disable gravity for the enemy
        rb.linearVelocity = Vector2.zero; // Stop the enemy's movement
        bodyCollider.enabled = false; // Disable the enemy's collider
         // Ignore collision with the player layer
        Destroy(gameObject, destroyTime); // Destroy the enemy GameObject after 1 second
    }

    public void TakeDamage(float damage)
    {
        if (dead) return; // Ignore if already dead

        
        if (Time.time - lastHitTime < 0.3f) return; // Ignore if hit too soon after the last hit
        hitCount++; // Increment the hit count
        health -= damage; // Subtract the damage from the enemy's health
        if (health < 0.0f) {
            Die(); // Call the Die method
            health = 0.0f; // Ensure health does not go below zero
        }
        if (stunnable){
            anim.SetBool("hit", true); // Trigger the hit animation
        }
        lastHitTime = Time.time; // Update the last hit time
    }

    public float GetHealth() {
        return health; // Return the current health of the enemy
    }
    public float GetMaxHealth() {
        return maxHealth; // Return the maximum
    }
    public float GetStunTime() {
        return stunTime; // Return the stun time
    }
    public bool IsDead() {
        return dead;
    }
    public bool IsStunned() {
        return Time.time - lastHitTime < stunTime && stunnable; // Check if the enemy is stunned based on the last hit time
    }
}
