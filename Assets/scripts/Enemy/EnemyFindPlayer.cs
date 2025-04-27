using UnityEngine;

public class EnemyFindPlayer : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    public PlayerMovement playerScript; // Reference to the PlayerMovement script
    [SerializeField] public float maxDistance = 30f; // Distance to check for the player
    public float distance; // Distance to the player
    public Vector2 playerVel;
    public Vector3 playerPos;
    public Vector2 playerRelativePos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find the player object
        
        if (player == null) {
            distance = 0f;
            playerVel = Vector2.zero; // Reset player velocity if player is not found
            playerPos = Vector3.zero; // Reset player position if player is not found
            playerRelativePos = Vector3.zero; // Reset player relative position if player is not found
            playerScript = null; // Reset player script if player is not found
            return; // If player is not found, exit the method
        }

        playerScript = player.GetComponent<PlayerMovement>(); // Get the PlayerMovement script from the player object

        playerPos = playerScript.GetPosition(); // Get the player's position
        distance = Vector2.Distance(playerPos, transform.position); // Calculate the distance to the player
        playerVel = playerScript.GetVelocity(); // Get the player's velocity
        playerRelativePos = playerPos - transform.position; // Calculate the player's relative position to the enemy
    }
}
