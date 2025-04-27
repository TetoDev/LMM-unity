using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyFindPlayer findPlayer;
    [SerializeField] private EnemyDirection direction;
    [SerializeField] private Collider2D hitBox;

    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float attackCooldown = 2.5f;
    [SerializeField] private float attack1Range = 1f;
    [SerializeField] private float attack2Range = 5f;
    [SerializeField] private float attack2Duration = 1f;
    [SerializeField] private float damage = 20f;

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;

    private Vector2 attack2Direction;
    
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isStunned = false;

    private float lastAttack = 0f; // Time of the last attack

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        rb.gravityScale = 0; // Set gravity scale to 0 for flying enemies
    }

    // Update is called once per frame
    void Update()
    {
        isStunned = health.IsStunned(); // Check if the enemy is stunned
        isDead = health.IsDead(); // Check if the enemy is dead

        flyTowardsPlayer(); // Call the method to fly towards the player
        attackPlayer(); // Call the method to attack the player
        InflictDamage(damage);

        if (isAttacking && attack2Direction != Vector2.zero) // If the enemy is attacking and has a direction
        {
            col.enabled = false; // Disable the collider to prevent damage during the attack
            if (anim.GetFloat("AttackActive") > 0f) // If the attack animation is active
            {
                rb.linearVelocity = attack2Direction * speed * 4; // Move in the attack direction
            }
            else
            {
                rb.linearVelocity = 0.3f * speed * -attack2Direction; // Move in the attack direction
            }
            
        }
        

        if (isDead) {
            rb.gravityScale = 4; // Set gravity scale to 0 if the enemy is dead
        }
    }

    private void flyTowardsPlayer()
    {
        if (isDead || isStunned || isAttacking || findPlayer.distance < 0.4f) return; // If the enemy is dead or stunned, do not move

        
        rb.linearVelocity = findPlayer.playerRelativePos.normalized * speed; // Move towards the player
    }

    private void attackPlayer()
    {
        if (isDead || isStunned || isAttacking || Time.time - lastAttack < attackCooldown) return; // If the enemy is dead or stunned, do not attack

        if (findPlayer.distance < attack1Range) // If the player is within attack range 1
        {
            lastAttack = Time.time; // Update the last attack time
            isAttacking = true; // Set attacking state to true
            anim.SetTrigger("attack"); // Trigger the attack animation
            Invoke("resetAttack", attackCooldown); // Reset the attack state after cooldown
            direction.Disable(); // Disable the direction script to prevent flipping during attack
        }
        else if (findPlayer.distance > attack1Range && findPlayer.distance < attack2Range) // If the player is within attack range 2
        {
            lastAttack = Time.time; // Update the last attack time
            attack2Direction = findPlayer.playerRelativePos.normalized; // Get the direction to the player
            isAttacking = true; // Set attacking state to true
            anim.SetTrigger("attack2"); // Trigger the attack animation
            Invoke("resetAttack", attack2Duration); // Reset the attack state after cooldown
            direction.Disable(); // Disable the direction script to prevent flipping during attack
        }
    }
    private void resetAttack()
    {
        isAttacking = false; // Reset the attacking state
        attack2Direction = Vector2.zero; // Reset the attack direction
        direction.Enable(); // Enable the direction script to allow flipping again
        col.enabled = true; // Enable the collider after the attack
        rb.linearVelocity = Vector2.zero; // Stop the enemy's movement after the attack
    }

    private void InflictDamage(float damage)
    {
        if (anim.GetFloat("AttackActive") > 0f) // If the attack animation is active
        {
            ContactFilter2D filter = new ContactFilter2D(); // Create a new ContactFilter2D object
            filter.SetLayerMask(LayerMask.GetMask("Player")); // Set the layer mask to detect players
            filter.useTriggers = true; // Use triggers for the collider detection

            Collider2D[] colliders = new Collider2D[10]; // Array to store detected colliders
            int playersHit = Physics2D.OverlapCollider(hitBox, filter, colliders); // Detect players in the attack range

            for (int i = 0; i < playersHit; i++)
            {
                PlayerMovement player = colliders[i].GetComponent<PlayerMovement>();
                if (player != null)
                {
                    player.TakeDamage(damage); // Call the TakeDamage method on the player
                }
            }
        }
    }
}
