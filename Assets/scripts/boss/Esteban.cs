using System;
using UnityEngine;

public class Esteban : MonoBehaviour
{
    [SerializeField] private EnemyFindPlayer findPlayer;
    [SerializeField] private EnemyCloseAttack enemyAttack;
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyDirection direction;
    [SerializeField] private EnemyCastSpell castSpell;
    [SerializeField] private Collider2D attackCol;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float idealDistance = 7f; // Ideal distance to maintain from the player
    [SerializeField] private float attackDistance = 5f; // Distance to attack the player

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

    private bool isAttacking = false;
    private bool isCasting = false;
    private bool isDead = false;
    private float attackCooldown = 1.0f; // Cooldown time between attacks
    private float lastAttackTime = 0f; // Time of the last attack
    private bool boosted = false;
    private bool attackMode = false;
    private float attackModeDuration = 5f; // Duration of the attack mode    
    private float attackModeStartTime = 0f; // Time when the attack mode started
    private float attackModeCooldown = 5f; // Cooldown time before the enemy can enter attack mode again 
    private float spellCooldown = 3f; // Cooldown time between spells
    private float lastSpellTime = 0f; // Time of the last spell cast



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        attackCol.enabled = false; // Disable the attack collider at the start
        castSpell.DisableAutoSpell(); // Disable auto spell casting at the start
        castSpell.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || health.IsStunned()) return;

        if (health.GetHealth() <= health.GetMaxHealth()/2 && !boosted){
            boosted = true;
            speed = speed *1.2f;
            enemyAttack.SetAttackDamage(enemyAttack.GetAttackDamage() * 2.0f); // Increase attack damage by 200%
            anim.SetBool("boosted", true); // Set the boosted animation
            enemyAttack.SetAttackCollider(attackCol); // Set the attack boosted collider
        }

        if (anim.GetFloat("CastActive") > 0f) {
            if (Time.time >= lastSpellTime + spellCooldown) {
                for (int i = -1; i < 2; i++){
                    castSpell.CastSpell(new Vector3(findPlayer.playerPos.x + i*3.3f*(direction.GetDirection()?1:-1),transform.position.y + 0.5f, transform.position.z), false); // Cast spell at the player
                }
                isCasting = false;
                lastSpellTime = Time.time; // Update the last spell time
            }
            
        }

        isAttacking = enemyAttack.IsAttacking() || isCasting; // Check if the enemy is attacking

        if (isAttacking) {
            stopMoving(); // Stop moving if attacking
            return;
        }

        if (attackMode) {
            if (Time.time >= attackModeStartTime + attackModeDuration) {
                attackMode = false; // Exit attack mode after the duration
            } else {
                approach(); // Move towards the player while in attack mode
                return;
            }
        }

        if (!boosted) {
             if (findPlayer.distance < idealDistance) {
                stepBack(); // Move away from the player if too close
            }
             else if (findPlayer.distance > idealDistance) {
                approach(); // Stop moving if within ideal distance
            }

            if (Math.Abs(findPlayer.distance - idealDistance) < 0.5f && !attackMode) {
                    stopMoving(); // Stop moving if within ideal distance
                    if (Time.time - lastSpellTime  > spellCooldown) {
                        spellAttack(); // Cast a spell
                    }
                    
                    
            }

            if (findPlayer.distance < attackDistance && Time.time >= attackModeStartTime + attackModeCooldown) {
                    attackMode = true; // Enter attack mode if within attack distance
                    attackModeStartTime = Time.time; // Record the time when attack mode started

                
            }
        } else {
            approach();
        
            if (findPlayer.distance > attackDistance && Time.time >= lastSpellTime + spellCooldown) {
                spellAttack(); // Cast a spell
            }
        }
}

    private void spellAttack() {
        anim.SetTrigger("cast");
        isCasting = true; // Set attacking state to true
    }

    private void approach() {
        rb.linearVelocityX = findPlayer.playerRelativePos.normalized.x * speed * 1.7f; // Move towards the player
        anim.SetBool("isMoving", true); // Set the moving animation
    }

    private void stepBack() {
        rb.linearVelocityX = -findPlayer.playerRelativePos.normalized.x * speed; // Move away from the player
        anim.SetBool("isMoving", true); // Set the moving animation
    }

    private void stopMoving() {
        rb.linearVelocity = Vector2.zero; // Stop moving
        anim.SetBool("isMoving", false); // Stop the moving animation
    }
}
