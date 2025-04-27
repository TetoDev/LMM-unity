using UnityEngine;

public class EnemyStalk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private bool following;
    [SerializeField] EnemyFindPlayer findPlayer; // Reference to the EnemyFindPlayer script
    [SerializeField] public float maxDistance = 30f; // Distance to check for the player
    [SerializeField] public float minDistance = 1f; // Minimum distance to check for the player
    [SerializeField] public float speed = 5f; // Speed of the enemy
    [SerializeField] EnemyHealth enemyHealth; // Reference to the EnemyHealth script

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        following = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.IsDead()) return; // If the enemy is dead, do not update movement
        if (Mathf.Abs(findPlayer.playerPos.x - transform.position.x) < minDistance) {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop moving if the player is too close
            anim.SetBool("isMoving", false); // Set the animation to idle
            following = false; // Set following to false
            return; // Exit the method if the player is too close
        }
        if (findPlayer.distance < maxDistance) {
            Vector2 direction = (findPlayer.playerPos - transform.position).normalized; // Calculate the direction to the player
            rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y); // Move the enemy towards the player
            anim.SetBool("isMoving", true); // Set the animation to moving
            following = true; // Set following to true
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop moving if the player is out of range
            anim.SetBool("isMoving", false); // Set the animation to idle
            following = false; // Set following to false
        }
    }

    public bool IsFollowing() {
        return following; // Return the current following state of the enemy
    }
}
