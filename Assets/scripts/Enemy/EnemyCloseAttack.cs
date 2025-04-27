using UnityEngine;

public class EnemyCloseAttack : MonoBehaviour
{


    [SerializeField] private float attackRange = 1.5f; // Range of the attack
    [SerializeField] private LayerMask playerLayers; // Layers to detect players
    [SerializeField] private float attackCooldown = 1.0f; // Cooldown time before the enemy can attack again
    [SerializeField] private float attackDamage = 10f; // Damage dealt by the attack
    [SerializeField] Collider2D attackCollider; // Collider used for the attack
    private float lastAttackTime; // Time of the last attack
    private Animator anim; // Reference to the Animator component
    [SerializeField] EnemyFindPlayer findPlayer; // Reference to the EnemyFindPlayer script
    [SerializeField] EnemyHealth enemyHealth; // Reference to the EnemyHealth script
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastAttackTime = 0f;
        anim = GetComponent<Animator>(); // Get the Animator component from the enemy GameObject
    }

    // Update is called once per frame
    void Update()
    {
        if (findPlayer.distance < attackRange && Time.time >= lastAttackTime + attackCooldown && !enemyHealth.IsDead() && !enemyHealth.IsStunned())  {
            Attack(); // Call the Attack method if the player is within range and cooldown is over
        }
        if (anim.GetFloat("AttackActive")> 0f) {
            attackCollider.enabled = true; // Enable the attack collider when the attack animation is active
            DealDamage(); // Call the DealDamage method to check for players in the attack range
        } else {
            attackCollider.enabled = false; // Disable the attack collider when the attack animation is not active
            
        }
        
    }

    private void DealDamage (){
        ContactFilter2D filter = new ContactFilter2D(); // Create a new ContactFilter2D object
        // filter.SetLayerMask(playerLayers); // Set the layer mask to detect players
        filter.useTriggers = true; // Use triggers for the collider detection
        
        Collider2D[] colliders = new Collider2D[10]; // Array to store detected colliders
        int playersHit = Physics2D.OverlapCollider(attackCollider, filter, colliders); // Detect players in the attack range

        // Debug.Log("Players hit: " + playersHit); // Log the number of players hit

         for (int i = 0; i < playersHit; i++)
        {
            PlayerMovement player = colliders[i].GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(attackDamage); // Call the TakeDamage method on the player
            }
        }

    }

    private void Attack()
    {
        if (anim.GetBool("hit")) return;
        lastAttackTime = Time.time; // Update the last attack time
        anim.SetTrigger("attack"); // Trigger the attack animation
    }
    
}
