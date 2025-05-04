using UnityEngine;
using System.Collections.Generic;

public class PlayerSFX : MonoBehaviour
{
    public AudioClip jumpSound; // Sound effect for jumping
    public AudioClip attackSound; // Sound effect for attacking
    public AudioClip dashSound; // Sound effect for dashing
    public AudioClip hitSound; // Sound effect for getting hit

    public void handleSFX(bool jump, bool dash)
    {
        if (jump) // If the player is jumping
        {
            PlayJumpSound(); // Play the jump sound effect
        }
        if (dash) // If the player is dashing
        {
            PlayDashSound(); // Play the dash sound effect
        }
    }

    public void PlayJumpSound()
    {
        SoundFX.instance.PlaySound(jumpSound, transform, 0.5f); // Play the jump sound effect
    }

    public void PlayAttackSound()
    {
        SoundFX.instance.PlaySound(attackSound, transform, 0.5f); // Play the attack sound effect
    }

    public void PlayDashSound()
    {
        SoundFX.instance.PlaySound(dashSound, transform, 0.5f); // Play the dash sound effect
    }

    public void PlayHitSound()
    {
        SoundFX.instance.PlaySound(hitSound, transform, 0.5f); // Play the hurt sound effect
    }
}