using UnityEngine;
using System.Collections.Generic;

public class EnemySFX : MonoBehaviour
{
    private SoundFX soundFX; // Reference to the SoundFX script
    public AudioClip jumpSound; // Sound effect for jumping
    public AudioClip attackSound; // Sound effect for attacking
    public AudioClip deathSound; // Sound effect for dashing
    public AudioClip hitSound; // Sound effect for getting hit

    public void PlayJumpSound()
    {
        SoundFX.instance.PlaySound(jumpSound, transform, 0.5f); // Play the jump sound effect
    }

    public void PlayAttackSound()
    {
        SoundFX.instance.PlaySound(attackSound, transform, 0.5f); // Play the attack sound effect
    }

    public void PlayDeathSound()
    {
        SoundFX.instance.PlaySound(deathSound, transform, 0.5f); // Play the dash sound effect
    }

    public void PlayHitSound()
    {
        SoundFX.instance.PlaySound(hitSound, transform, 0.5f); // Play the hurt sound effect
    }
}