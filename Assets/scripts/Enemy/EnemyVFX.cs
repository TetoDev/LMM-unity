using UnityEngine;

public class EnemyVFX : MonoBehaviour
{
 
    public ParticleSystem blood; // Reference to the jump dust particle system


    void createblood()
    {
        blood.Play(); // Play the dust particle system
    }
}