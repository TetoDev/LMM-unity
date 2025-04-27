using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerChangeDirection : MonoBehaviour
{

    private Vector3 originalScale; // Original scale of the player
    private bool locked;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalScale = transform.localScale; // Store the original scale of the player
        locked = false; // Initialize locked state to false
    }

    public void ChangeDirection(bool direction)
    {
        if (locked) return; // If locked, do not change direction
        // Flip the player's scale based on direction
        transform.localScale = new Vector3(originalScale.x * (direction ? 1 : -1), originalScale.y, originalScale.z);
    }

    public void Lock(){
        locked = true;
    }

    public void Unlock(){
        locked = false;
    }
    public bool IsLocked(){
        return locked;
    }
}
