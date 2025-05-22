using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public ParticleSystem dust;
    public ParticleSystem blood; // Reference to the jump dust particle system
    [SerializeField] private PlayerChangeDirection playerDirection; // Reference to the PlayerChangeDirection script
    [SerializeField] private GroundCheck groundCheck; // Reference to the GroundCheck script
    [SerializeField] private PlayerMovement isDashing; // Reference to the PlayerMovement script

    void createDust()
    {
        dust.Play(); // Play the dust particle system
    }
    void createblood()
    {
        blood.Play(); // Play the dust particle system
    }

    public void HandleVFX(bool jump)
    {
        if (jump) // If the player is jumping
        {
            dust.Play(); // Stop the dust particle system
        }
        if (isDashing.IsDashing()) // If the player is dashing
        {
            dust.Play(); // Stop the dust particle system
        }
        if (playerDirection.hasChangedDirection() && groundCheck.IsGrounded()) // If the player has changed direction
        {
            createDust(); // Create dust effect
        }
    }
}