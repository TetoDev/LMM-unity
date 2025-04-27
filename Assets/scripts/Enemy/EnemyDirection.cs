using UnityEngine;

public class EnemyDirection : MonoBehaviour
{

    private bool direction; // true for right, false for left
    private Vector3 originalScale; // Original scale of the enemy
    [SerializeField] float maxDistance = 30f; // Distance to check for the player
    [SerializeField] EnemyFindPlayer findPlayer; // Reference to the EnemyFindPlayer script
    [SerializeField] EnemyHealth enemyHealth; // Reference to the EnemyHealth script

    void Start()
    {
        originalScale = transform.localScale; // Store the original scale of the enemy
        direction = false; // Initialize direction to right

    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.IsDead()) return; // If the enemy is dead, do not update direction
        // Check the player's position relative to the enemy's position
        if (findPlayer.playerPos.x > transform.position.x && 
            findPlayer.distance < maxDistance)
        {
            direction = true; // Player is to the right of the enemy
        }
        else
        {
            direction = false; // Player is to the left of the enemy
        }
        
        transform.localScale = new Vector3(originalScale.x * (direction ? 1 : -1), originalScale.y, originalScale.z); // Flip the enemy's scale based on direction
    }

    public bool GetDirection()
    {
        return direction; // Return the current direction of the enemy
    }

    public void Disable () {
        enabled = false; // Disable this script
    }

    public void Enable () {
        enabled = true; // Enable this script
    }
}
