using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private bool attacking;
    private bool attackDirection; // true for right, false for left
    private Vector3 originalScale; // Original scale of the player
    private bool hasAttacked; // Flag to check if the player was attacking
    [SerializeField] private Collider2D attackCollider;
    [SerializeField] private float damage = 10f;
    [SerializeField] private PlayerChangeDirection playerDirection; // Reference to the PlayerChangeDirection script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>(); // Get the Animator component attached to the player
        if (anim == null) {
            Debug.LogError("No Animator found on the player GameObject!"); // Log an error if no animator is found
        }

        attacking = false; // Initialize attacking state to false
        hasAttacked = false; // Initialize wasAttacking state to false
        attackCollider.enabled = false; // Disable the attack collider at the start

        originalScale = transform.localScale; // Store the original scale of the player
    }

    // Update is called once per frame
    void Update()
    {
        if (attacking) {
            if (anim.GetFloat("AttackActive") > 0f) {
                attackCollider.enabled = true; // Enable the attack collider when the attack animation is active
                DealDamage(); // Call the DealDamage method to check for players in the attack range
                hasAttacked = true; // Set wasAttacking to true when the attack animation is active
            } else {
                attackCollider.enabled = false; // Disable the attack collider when the attack animation is not active
                if (hasAttacked) {
                    attacking = false; // Set attacking to false when the attack animation is finished
                    hasAttacked = false; // Reset wasAttacking state
                }
                // Debug.Log("Attack finished"); // Log when the attack animation is finished
            }
        } else {
            playerDirection.Unlock(); // Unlock the player direction after the attack animation is done
        }
    }

    private void DealDamage () {
        ContactFilter2D filter = new ContactFilter2D(); // Create a new ContactFilter2D object
        filter.useTriggers = true; // Use triggers for the collider detection
        
        Collider2D[] colliders = new Collider2D[10]; // Array to store detected colliders
        int enemiesHit = Physics2D.OverlapCollider(attackCollider, filter, colliders); // Detect players in the attack range

        // Debug.Log("Enemies hit: " + enemiesHit); // Log the number of players hit

        for (int i = 0; i < enemiesHit; i++)
        {
            EnemyHealth enemy = colliders[i].GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // Call the TakeDamage method on the enemy
            }
        }
    }

    public void Attack(Vector2 mousePos){
        anim.SetTrigger("Attack"); // Trigger the attack animation
        attacking = true;
        attackDirection = mousePos.x > transform.position.x; // Determine the attack direction based on mouse position
        playerDirection.ChangeDirection(attackDirection); // Call the ChangeDirection method to flip the player sprite
        playerDirection.Lock();
    }

    public void SetDamage(float damage) {
        this.damage = damage; // Set the damage value
    }
    public float GetDamage() {
        return damage; // Return the damage value
    }

    public bool IsAttacking() {
        return attacking; // Set the attacking state
    }
}
